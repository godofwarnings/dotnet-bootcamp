## Question

How do I add the resilient retry/timeout/circuit-breaker code to my current fee service?

## Answer

Short answer:

- **Do not paste `ResilientFeeService` inside `FeeService`**
- Keep `FeeService` as the **basic flaky service**
- Add `ResilientFeeService` as a **wrapper around `IFeeService`**
- Also, since you already want retry logic in the resilient layer, **remove retry logic from `FeeService`** to avoid **double retries**

If you keep retries in both places:

- `FeeService` retries
- then `ResilientFeeService` retries again

That becomes messy fast.

---

## Recommended structure

### 1) IFeeService.cs

```csharp
namespace WebApplication13.Services
{
    public interface IFeeService
    {
        Task<decimal> GetPendingFeeAsync(int studentId);
    }
}
```

---

### 2) IResilientFeeService.cs

```csharp
namespace WebApplication13.Services
{
    public interface IResilientFeeService
    {
        Task<decimal> GetPendingFeeWithRetryAndTimeoutAsync(int studentId, int maxRetryAttempts = 3);
    }
}
```

---

### 3) CircuitBreakerState.cs

```csharp
namespace WebApplication13.Services
{
    public enum CircuitBreakerState
    {
        Closed,
        Open,
        HalfOpen
    }
}
```

---

### 4) FeeService.cs

This becomes the **raw service** that may fail.

```csharp
using System.Threading;

namespace WebApplication13.Services
{
    public class FeeService : IFeeService
    {
        private static int requestCount = 0;

        public async Task<decimal> GetPendingFeeAsync(int studentId)
        {
            // Simulate a little service delay.
            await Task.Delay(300);

            return GetPendingFeeCore(studentId);
        }

        private decimal GetPendingFeeCore(int studentId)
        {
            int currentRequest = Interlocked.Increment(ref requestCount);

            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student id.");
            }

            if (studentId == 999)
            {
                throw new Exception("Student does not exist.");
            }

            // Simulate a temporary failure.
            // Only every 3rd request succeeds.
            if (currentRequest % 3 != 0)
            {
                throw new TimeoutException("Temporary timeout from Fee Service.");
            }

            return studentId switch
            {
                1 => 5000m,
                2 => 3000m,
                3 => 0m,
                _ => 1000m
            };
        }
    }
}
```

---

### 5) ResilientFeeService.cs

This is where the **retry + timeout + circuit breaker** logic belongs.

```csharp
namespace WebApplication13.Services
{
    public class ResilientFeeService : IResilientFeeService
    {
        private readonly IFeeService feeService;
        private readonly object stateLock = new();

        private int failureCount = 0;
        private CircuitBreakerState circuitState = CircuitBreakerState.Closed;
        private DateTime circuitOpenedTime;

        private const int failureThreshold = 3;
        private static readonly TimeSpan openDuration = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan requestTimeout = TimeSpan.FromSeconds(2);
        private static readonly TimeSpan retryDelay = TimeSpan.FromSeconds(1);

        public ResilientFeeService(IFeeService feeService)
        {
            this.feeService = feeService;
        }

        public async Task<decimal> GetPendingFeeWithRetryAndTimeoutAsync(int studentId, int maxRetryAttempts = 3)
        {
            if (maxRetryAttempts <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetryAttempts), "maxRetryAttempts must be greater than 0.");
            }

            EnsureCircuitAllowsExecution();

            for (int attempt = 1; attempt <= maxRetryAttempts; attempt++)
            {
                try
                {
                    Console.WriteLine($"Circuit State: {GetCircuitState()}");
                    Console.WriteLine($"Calling FeeService. Attempt: {attempt}");

                    Task<decimal> feeTask = feeService.GetPendingFeeAsync(studentId);
                    Task timeoutTask = Task.Delay(requestTimeout);

                    Task completedTask = await Task.WhenAny(feeTask, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        throw new TimeoutException("Fee Service took too long to respond.");
                    }

                    decimal pendingFee = await feeTask;

                    ResetCircuit();

                    Console.WriteLine("FeeService succeeded. Circuit is Closed.");

                    return pendingFee;
                }
                catch (TimeoutException ex)
                {
                    Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");

                    RegisterFailure();

                    if (GetCircuitState() == CircuitBreakerState.Open)
                    {
                        throw new Exception("Circuit opened. Fee Service is unavailable.", ex);
                    }

                    if (attempt == maxRetryAttempts)
                    {
                        throw;
                    }

                    await Task.Delay(retryDelay);
                }
            }

            throw new Exception("Retry failed unexpectedly.");
        }

        private void EnsureCircuitAllowsExecution()
        {
            lock (stateLock)
            {
                if (circuitState != CircuitBreakerState.Open)
                {
                    return;
                }

                if (DateTime.UtcNow - circuitOpenedTime < openDuration)
                {
                    throw new Exception("Circuit is open. Fee Service is temporarily blocked.");
                }

                circuitState = CircuitBreakerState.HalfOpen;
                Console.WriteLine("Circuit moved to Half-Open state.");
            }
        }

        private void RegisterFailure()
        {
            lock (stateLock)
            {
                failureCount++;

                if (failureCount >= failureThreshold)
                {
                    circuitState = CircuitBreakerState.Open;
                    circuitOpenedTime = DateTime.UtcNow;

                    Console.WriteLine("Circuit opened because failure threshold was reached.");
                }
            }
        }

        private void ResetCircuit()
        {
            lock (stateLock)
            {
                failureCount = 0;
                circuitState = CircuitBreakerState.Closed;
            }
        }

        private CircuitBreakerState GetCircuitState()
        {
            lock (stateLock)
            {
                return circuitState;
            }
        }
    }
}
```

---

### 6) Program.cs

Because `ResilientFeeService` holds circuit-breaker state, register it as a **singleton**.

Also register `FeeService` as singleton so you do **not** create a lifetime mismatch.

```csharp
using WebApplication13.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IFeeService, FeeService>();
builder.Services.AddSingleton<IResilientFeeService, ResilientFeeService>();

var app = builder.Build();

app.MapControllers();

app.Run();
```

---

## How to call it from a controller

```csharp
using Microsoft.AspNetCore.Mvc;
using WebApplication13.Services;

namespace WebApplication13.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeeController : ControllerBase
    {
        private readonly IResilientFeeService resilientFeeService;

        public FeeController(IResilientFeeService resilientFeeService)
        {
            this.resilientFeeService = resilientFeeService;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetFee(int studentId)
        {
            try
            {
                decimal pendingFee = await resilientFeeService.GetPendingFeeWithRetryAndTimeoutAsync(studentId);

                return Ok(new
                {
                    StudentId = studentId,
                    PendingFee = pendingFee
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
```

---

## Why I changed your current code

### 1) I removed retry logic from FeeService
Your original `FeeService` already had retry logic.

If you add `ResilientFeeService` on top of that, you get:

- retry inside `FeeService`
- retry again inside `ResilientFeeService`

That is usually not what you want.

---

### 2) I changed `Thread.Sleep(500)` to `await Task.Delay(...)`
Your method is async:

```csharp
public async Task<decimal> GetPendingFeeAsync(...)
```

So blocking with `Thread.Sleep(...)` is not a good fit there.

Use:

```csharp
await Task.Delay(...)
```

instead.

---

### 3) I kept FeeService simple
Now `FeeService` has one job:

- simulate the downstream fee call
- sometimes fail
- sometimes succeed

And `ResilientFeeService` has the resilience job:

- retry
- timeout
- circuit breaker

That separation is cleaner.

---

## How it works

### FeeService
This is the unstable dependency.

- it increments `requestCount`
- it throws `TimeoutException` most of the time
- only every 3rd request succeeds

---

### ResilientFeeService
This wraps `FeeService`.

#### Retry
If a timeout happens:

- it retries up to `maxRetryAttempts`

#### Timeout
It gives `FeeService` only 2 seconds:

- if the fee task does not finish in time, it throws timeout

#### Circuit breaker
If too many timeouts happen:

- failure count increases
- once it reaches 3 failures
- the circuit becomes `Open`
- new calls are blocked for 10 seconds

After 10 seconds:

- circuit becomes `HalfOpen`
- it allows a trial call
- if that succeeds, circuit goes back to `Closed`

---

## Very important note

This is good as a **test-of-concept**.

For real production code, people usually use:

- `ILogger` instead of `Console.WriteLine`
- cancellation tokens
- Polly for retry/circuit-breaker policies

But for learning and demo purposes, this version is fine.

---

## If you want to keep your original FeeService retry method

You technically can, but then your resilient service should call it with **no internal retries**.

That means your interface would need something like this:

```csharp
Task<decimal> GetPendingFeeAsync(int studentId, int maxRetries = 0);
```

And then the resilient service would call:

```csharp
feeService.GetPendingFeeAsync(studentId, 0);
```

But I do **not** recommend that version for learning, because it mixes responsibilities.

---

## Best version to use

Use this design:

- `FeeService` = raw flaky dependency
- `ResilientFeeService` = retry + timeout + circuit breaker
- controller calls `IResilientFeeService`

---

If you want, I can next give you:

1. a **line-by-line explanation of `ResilientFeeService`**
2. a **broken vs fixed version comparison**
3. a **Polly version of the same solution**
