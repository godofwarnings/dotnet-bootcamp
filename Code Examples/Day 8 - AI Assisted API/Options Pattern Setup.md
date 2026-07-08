## Question

How do I access `retryCount` from `appsettings.json`?

## Answer

The clean/professional way is to use the **Options pattern** and move your retry settings into a typed settings class.

Since your retry logic is in `ResilientFeeService`, that is the best place to read it.

---

## 1) Add settings to `appsettings.json`

```json
{
  "RetrySettings": {
    "MaxRetryAttempts": 3,
    "RetryDelayMilliseconds": 1000,
    "RequestTimeoutMilliseconds": 2000
  }
}
```

---

## 2) Create a settings class

### `Models/Configurations/RetrySettings.cs`

```csharp
namespace WebApplication13.Models.Configurations
{
    public sealed class RetrySettings
    {
        public int MaxRetryAttempts { get; set; } = 3;

        public int RetryDelayMilliseconds { get; set; } = 1000;

        public int RequestTimeoutMilliseconds { get; set; } = 2000;
    }
}
```

---

## 3) Register the settings in `Program.cs`

```csharp
using WebApplication13.Models.Configurations;
using WebApplication13.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<RetrySettings>(
    builder.Configuration.GetSection("RetrySettings"));

builder.Services.AddSingleton<IFeeService, FeeService>();
builder.Services.AddSingleton<IResilientFeeService, ResilientFeeService>();

var app = builder.Build();

app.MapControllers();

app.Run();
```

---

## 4) Update `ResilientFeeService`

Replace the hard-coded retry values with values from configuration.

### `Services/ResilientFeeService.cs`

```csharp
using Microsoft.Extensions.Options;
using WebApplication13.Models.Configurations;

namespace WebApplication13.Services
{
    public class ResilientFeeService : IResilientFeeService
    {
        private readonly IFeeService feeService;
        private readonly RetrySettings retrySettings;
        private readonly object stateLock = new();

        private int failureCount = 0;
        private CircuitBreakerState circuitState = CircuitBreakerState.Closed;
        private DateTime circuitOpenedTime;

        private const int failureThreshold = 3;
        private static readonly TimeSpan openDuration = TimeSpan.FromSeconds(10);

        public ResilientFeeService(
            IFeeService feeService,
            IOptions<RetrySettings> retryOptions)
        {
            this.feeService = feeService;
            retrySettings = retryOptions.Value;
        }

        public async Task<decimal> GetPendingFeeWithRetryAndTimeoutAsync(int studentId)
        {
            if (retrySettings.MaxRetryAttempts <= 0)
            {
                throw new InvalidOperationException("RetrySettings:MaxRetryAttempts must be greater than 0.");
            }

            EnsureCircuitAllowsExecution();

            for (int attempt = 1; attempt <= retrySettings.MaxRetryAttempts; attempt++)
            {
                try
                {
                    Console.WriteLine($"Circuit State: {GetCircuitState()}");
                    Console.WriteLine($"Calling FeeService. Attempt: {attempt}");

                    Task<decimal> feeTask = feeService.GetPendingFeeAsync(studentId);
                    Task timeoutTask = Task.Delay(retrySettings.RequestTimeoutMilliseconds);

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

                    if (attempt == retrySettings.MaxRetryAttempts)
                    {
                        throw;
                    }

                    await Task.Delay(retrySettings.RetryDelayMilliseconds);
                }
            }

            throw new Exception("Retry failed unexpectedly.");
        }
// ... [Methods omitted for brevity: EnsureCircuitAllowsExecution, RegisterFailure, ResetCircuit, GetCircuitState]
    }
}
```

---

## 5) Update the interface if needed

If your current interface already has this method, keep it.

### `Services/IResilientFeeService.cs`

```csharp
namespace WebApplication13.Services
{
    public interface IResilientFeeService
    {
        Task<decimal> GetPendingFeeWithRetryAndTimeoutAsync(int studentId);
    }
}
```

---

## What changed

Before, you had this hard-coded:

```csharp
int maxRetryAttempts = 3;
```

Now it comes from:

```csharp
retrySettings.MaxRetryAttempts
```

So if you change `appsettings.json` to:

```json
{
  "RetrySettings": {
    "MaxRetryAttempts": 5,
    "RetryDelayMilliseconds": 1500,
    "RequestTimeoutMilliseconds": 3000
  }
}
```

your service will use those values automatically.
