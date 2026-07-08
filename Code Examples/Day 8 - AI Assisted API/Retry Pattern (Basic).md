## Question

Write retry logic for `FeeService` so it retries 3 times, or `n` times, before failing.

## Answer

Use retry **only** for `TimeoutException`.

- `ArgumentException` should fail immediately
- `"Student does not exist"` should fail immediately
- `TimeoutException` is the transient failure to retry

### Interface

```csharp
namespace WebApplication13.Services
{
    public interface IFeeService
    {
        decimal GetPendingFee(int studentId, int maxRetries = 3);
    }
}
```

### Service with retry

```csharp
using System.Threading;

namespace WebApplication13.Services
{
    public class FeeService : IFeeService
    {
        private static int requestCount = 0;

        public decimal GetPendingFee(int studentId, int maxRetries = 3)
        {
            if (maxRetries < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries), "maxRetries cannot be negative.");
            }

            var attempt = 0;

            while (true)
            {
                try
                {
                    attempt++;

                    Console.WriteLine($"Attempt #{attempt} for studentId {studentId}");

                    return GetPendingFeeCore(studentId);
                }
                catch (TimeoutException) when (attempt <= maxRetries)
                {
                    Console.WriteLine($"Timeout on attempt #{attempt}. Retrying...");

                    // Test-of-concept delay before the next retry.
                    Thread.Sleep(500);
                }
            }
        }

        private decimal GetPendingFeeCore(int studentId)
        {
            requestCount++;

            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student id.");
            }

            if (studentId == 999)
            {
                throw new Exception("Student does not exist.");
            }

            if (requestCount % 3 != 0)
            {
                throw new TimeoutException("Temporary timeout from Fee Service.");
            }

            return studentId switch
            {
                1 => 5000,
                2 => 3000,
                3 => 0,
                _ => 1000
            };
        }
    }
}
```

## How this works

```csharp
_feeService.GetPendingFee(1);
```

- default is `maxRetries = 3`
- that means:
  - 1 initial attempt
  - plus up to 3 retries
  - total possible attempts = 4

If you want `n` retries:

```csharp
_feeService.GetPendingFee(1, 5);
```

That gives:

- 1 initial attempt
- plus 5 retries
- total possible attempts = 6

## If you want exactly 3 total attempts instead

Use `maxAttempts` instead of `maxRetries`.

```csharp
namespace WebApplication13.Services
{
    public class FeeService : IFeeService
    {
        private static int requestCount = 0;

        public decimal GetPendingFeeWithAttempts(int studentId, int maxAttempts = 3)
        {
            if (maxAttempts <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxAttempts), "maxAttempts must be greater than 0.");
            }

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    Console.WriteLine($"Attempt #{attempt} for studentId {studentId}");
                    return GetPendingFeeCore(studentId);
                }
                catch (TimeoutException) when (attempt < maxAttempts)
                {
                    Console.WriteLine($"Timeout on attempt #{attempt}. Retrying...");
                    Thread.Sleep(500);
                }
            }

            throw new InvalidOperationException("Unreachable code.");
        }

        private decimal GetPendingFeeCore(int studentId)
        {
            requestCount++;

            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student id.");
            }

            if (studentId == 999)
            {
                throw new Exception("Student does not exist.");
            }

            if (requestCount % 3 != 0)
            {
                throw new TimeoutException("Temporary timeout from Fee Service.");
            }

            return studentId switch
            {
                1 => 5000,
                2 => 3000,
                3 => 0,
                _ => 1000
            };
        }
    }
}
```

## Recommendation

For your **test of concept**, use the first version with:

```csharp
GetPendingFee(int studentId, int maxRetries = 3)
```

It is the simplest.

If you want, I can next give you:

- a **controller example** that calls this service
- an **async version**
- a **Polly-based retry version**


explain the code as well
## Question

Explain the retry logic code as well.

## Answer

Yes — below is the same **retry version** with clearer comments, followed by a plain-English explanation.

---

## Retry version with comments

```csharp
using System.Threading;

namespace WebApplication13.Services
{
    public interface IFeeService
    {
        decimal GetPendingFee(int studentId, int maxRetries = 3);
    }

    public class FeeService : IFeeService
    {
        // Shared across all calls to simulate an unstable external service.
        // Because it is static, it keeps its value between requests.
        private static int requestCount = 0;

        public decimal GetPendingFee(int studentId, int maxRetries = 3)
        {
            // Guard clause: retry count should not be negative.
            if (maxRetries < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries), "maxRetries cannot be negative.");
            }

            var attempt = 0;

            while (true)
            {
                try
                {
                    // Count the current attempt.
                    attempt++;

                    Console.WriteLine($"Attempt #{attempt} for studentId {studentId}");

                    // Try the actual fee lookup.
                    return GetPendingFeeCore(studentId);
                }
                // Retry only for timeout errors, and only while retries remain.
                catch (TimeoutException) when (attempt <= maxRetries)
                {
                    Console.WriteLine($"Timeout on attempt #{attempt}. Retrying...");

                    // Small delay so retries do not happen instantly.
                    Thread.Sleep(500);
                }
            }
        }

        private decimal GetPendingFeeCore(int studentId)
        {
            // Simulates calling an external fee system.
            requestCount++;

            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student id.");
            }

            if (studentId == 999)
            {
                throw new Exception("Student does not exist.");
            }

            // Simulate a temporary failure for most requests.
            // Only every 3rd request succeeds.
            if (requestCount % 3 != 0)
            {
                throw new TimeoutException("Temporary timeout from Fee Service.");
            }

            // Return hard-coded fee values.
            return studentId switch
            {
                1 => 5000,
                2 => 3000,
                3 => 0,
                _ => 1000
            };
        }
    }
}
```

---

## What each part does

### 1) `private static int requestCount = 0;`

```csharp
private static int requestCount = 0;
```

- This is being used to **simulate an unreliable external service**
- `static` means the value is shared across all calls
- So:
  - 1st call increments to `1`
  - 2nd call increments to `2`
  - 3rd call increments to `3`
- In your logic, only every 3rd call succeeds

---

### 2) `GetPendingFee(...)`

```csharp
public decimal GetPendingFee(int studentId, int maxRetries = 3)
```

This is the **public method** your controller or caller uses.

- `studentId` = which student you are checking
- `maxRetries = 3` = retry up to 3 times after a timeout

Important:

- this means **1 original attempt + up to 3 retries**
- so maximum total attempts = **4**

---

### 3) Retry validation

```csharp
if (maxRetries < 0)
{
    throw new ArgumentOutOfRangeException(nameof(maxRetries), "maxRetries cannot be negative.");
}
```

This protects your method from bad input.

Example:

- `maxRetries = 3` → valid
- `maxRetries = 0` → valid, no retry
- `maxRetries = -1` → invalid

---

### 4) Attempt counter

```csharp
var attempt = 0;
```

This keeps track of how many tries have happened.

---

### 5) `while (true)`

```csharp
while (true)
```

This means:

- keep trying until:
  - the method succeeds and returns a fee
  - or an exception happens that is **not retried**

The loop does **not** run forever in practice because either:

- `return GetPendingFeeCore(studentId);` ends it
- or a non-timeout exception escapes and stops it

---

### 6) `try` block

```csharp
try
{
    attempt++;
    Console.WriteLine($"Attempt #{attempt} for studentId {studentId}");
    return GetPendingFeeCore(studentId);
}
```

What happens here:

1. increase attempt number
2. log which attempt is happening
3. call the real logic in `GetPendingFeeCore`
4. if it succeeds, immediately return the fee

---

### 7) `catch (TimeoutException) when (attempt <= maxRetries)`

```csharp
catch (TimeoutException) when (attempt <= maxRetries)
```

This is the most important line.

It means:

- only catch **timeout** errors
- only retry while you still have retries left

So this code does **not** retry for:

- `ArgumentException`
- general `Exception`
- any other exception type

That is good because those are not temporary timeout failures.

---

### 8) Delay before retry

```csharp
Thread.Sleep(500);
```

This waits 500 milliseconds before retrying.

Why?

- to avoid retrying immediately
- to simulate a pause between attempts

For a **test of concept**, this is fine.

---

## What `GetPendingFeeCore(...)` does

This method contains the **real fee logic** and the **fake failure simulation**.

---

### A) Invalid student id

```csharp
if (studentId <= 0)
{
    throw new ArgumentException("Invalid student id.");
}
```

If the ID is `0` or negative, the method fails immediately.

This is **not retried**.

---

### B) Student does not exist

```csharp
if (studentId == 999)
{
    throw new Exception("Student does not exist.");
}
```

If student ID is `999`, you simulate a not-found kind of failure.

This is also **not retried**.

---

### C) Simulated timeout

```csharp
if (requestCount % 3 != 0)
{
    throw new TimeoutException("Temporary timeout from Fee Service.");
}
```

This says:

- if request count is not divisible by 3, fail with timeout
- only every 3rd request succeeds

Example:

- request 1 → timeout
- request 2 → timeout
- request 3 → success
- request 4 → timeout
- request 5 → timeout
- request 6 → success

---

### D) Return fee

```csharp
return studentId switch
{
    1 => 5000,
    2 => 3000,
    3 => 0,
    _ => 1000
};
```

If no exception happened, return a hard-coded fee.

Example:

- student `1` → `5000`
- student `2` → `3000`
- student `3` → `0`
- any other student → `1000`

---

## Example flow

### Call

```csharp
var fee = feeService.GetPendingFee(1, 3);
```

### What happens

#### Attempt 1
- `requestCount = 1`
- `1 % 3 != 0`
- throws `TimeoutException`
- retry

#### Attempt 2
- `requestCount = 2`
- `2 % 3 != 0`
- throws `TimeoutException`
- retry

#### Attempt 3
- `requestCount = 3`
- `3 % 3 == 0`
- success
- returns `5000`

---

## Why only `TimeoutException` is retried

Because timeout is treated as a **temporary/transient** failure.

These are good candidates for retry:

- timeout
- temporary network issue
- temporary downstream service problem

These should usually **not** be retried:

- invalid input
- missing record
- bad request
- business rule violation

So your logic is correct for a proof of concept.

---

## One important detail

Your method uses:

```csharp
maxRetries = 3
```

That means:

- 1 initial attempt
- plus 3 retries
- total = 4 tries max

If you wanted **exactly 3 total tries**, you would use `maxAttempts` instead.

---

## Very short summary

### `GetPendingFee`
- controls the retry behavior

### `GetPendingFeeCore`
- does the real work
- simulates failures

### `catch (TimeoutException)`
- retries only temporary timeout failures

### `Thread.Sleep(500)`
- pauses before the next retry

### `requestCount % 3 != 0`
- makes first 2 attempts fail and the 3rd succeed

---

## Small improvement note

For a real ASP.NET Core app, this is okay as a **learning example**, but later you would usually move to:

- `async/await`
- `Task.Delay(...)` instead of `Thread.Sleep(...)`
- possibly a retry library like Polly

---

If you want, I can next give you:

1. a **line-by-line explanation**
2. a **controller that uses this service**
3. an **async retry version**
4. a **version using Polly**
