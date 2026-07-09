### `Services/Idempotency/IIdempotencyStore.cs`

```csharp
namespace ProductManagementSystem.Services.Idempotency;

public interface IIdempotencyStore
{
    IdempotencyExecutionResult CheckOrReserve(string idempotencyKey, string requestHash);
    void CompleteReservation(string idempotencyKey, object responseData);
}
```

---

### `Services/Idempotency/IdempotencyRecord.cs`

```csharp
namespace ProductManagementSystem.Services.Idempotency;

public sealed class IdempotencyRecord
{
    public string RequestHash { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public object? ResponseData { get; set; }
}
```

---

### `Services/Idempotency/IdempotencyExecutionResult.cs`

```csharp
namespace ProductManagementSystem.Services.Idempotency;

public sealed class IdempotencyExecutionResult
{
    public bool IsDuplicateWithSamePayload { get; set; }
    public bool IsDuplicateWithDifferentPayload { get; set; }
    public object? ResponseData { get; set; }
}
```

---

### `Services/Idempotency/InMemoryIdempotencyStore.cs`

```csharp
using System.Collections.Concurrent;

namespace ProductManagementSystem.Services.Idempotency;

public sealed class InMemoryIdempotencyStore : IIdempotencyStore
{
    private readonly ConcurrentDictionary<string, IdempotencyRecord> _store = new(StringComparer.Ordinal);
    private readonly object _lock = new();

    public IdempotencyExecutionResult CheckOrReserve(string idempotencyKey, string requestHash)
    {
        lock (_lock)
        {
            if (_store.TryGetValue(idempotencyKey, out var existingRecord))
            {
                if (existingRecord.RequestHash != requestHash)
                {
                    return new IdempotencyExecutionResult
                    {
                        IsDuplicateWithDifferentPayload = true
                    };
                }

                if (existingRecord.IsCompleted)
                {
                    return new IdempotencyExecutionResult
                    {
                        IsDuplicateWithSamePayload = true,
                        ResponseData = existingRecord.ResponseData
                    };
                }

                // If it is same payload but not completed, another thread is processing it right now.
                // In a real system, you might wait. Here, we just reject it.
                throw new InvalidOperationException("A request with this Idempotency-Key is currently being processed.");
            }

            // Reserve it
            _store[idempotencyKey] = new IdempotencyRecord
            {
                RequestHash = requestHash,
                IsCompleted = false
            };

            return new IdempotencyExecutionResult(); // Not a duplicate, safe to proceed
        }
    }

    public void CompleteReservation(string idempotencyKey, object responseData)
    {
        lock (_lock)
        {
            if (_store.TryGetValue(idempotencyKey, out var existingRecord))
            {
                existingRecord.IsCompleted = true;
                existingRecord.ResponseData = responseData;
            }
        }
    }
}
```

---

### `Services/Idempotency/RequestHasher.cs`

```csharp
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ProductManagementSystem.Services.Idempotency;

public static class RequestHasher
{
    public static string ComputeHash(object request)
    {
        var json = JsonSerializer.Serialize(request);
        var bytes = Encoding.UTF8.GetBytes(json);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
```

---

### `Services/Resilience/CircuitBreakerState.cs`

```csharp
namespace ProductManagementSystem.Services.Resilience;

public enum CircuitBreakerState
{
    Closed,
    Open,
    HalfOpen
}
```

---

### `Services/Resilience/CircuitBreakerOpenException.cs`

```csharp
namespace ProductManagementSystem.Services.Resilience;

public sealed class CircuitBreakerOpenException : Exception
{
    public CircuitBreakerOpenException(string message) : base(message)
    {
    }
}
```

---

### `Services/Resilience/ICircuitBreaker.cs`

```csharp
namespace ProductManagementSystem.Services.Resilience;

public interface ICircuitBreaker
{
    void EnsureExecutionAllowed();
    void RecordSuccess();
    void RecordFailure();
}
```

---

### `Services/Resilience/InMemoryCircuitBreaker.cs`

```csharp
using Microsoft.Extensions.Options;
using ProductManagementSystem.Options;

namespace ProductManagementSystem.Services.Resilience;

public sealed class InMemoryCircuitBreaker : ICircuitBreaker
{
    private readonly ResilienceOptions _options;
    private readonly object _stateLock = new();

    private CircuitBreakerState _state = CircuitBreakerState.Closed;
    private int _failureCount = 0;
    private DateTime _nextAttemptTime = DateTime.MinValue;

    public InMemoryCircuitBreaker(IOptions<ResilienceOptions> options)
    {
        _options = options.Value;
    }

    public void EnsureExecutionAllowed()
    {
        lock (_stateLock)
        {
            if (_state == CircuitBreakerState.Closed)
            {
                return;
            }

            if (_state == CircuitBreakerState.Open)
            {
                if (DateTime.UtcNow >= _nextAttemptTime)
                {
                    _state = CircuitBreakerState.HalfOpen;
                    Console.WriteLine("[CIRCUIT] Changed to Half-Open. Allowing one probe request.");
                    return;
                }

                throw new CircuitBreakerOpenException("Circuit is OPEN. Fast-failing request.");
            }
        }
    }

    public void RecordSuccess()
    {
        lock (_stateLock)
        {
            if (_state == CircuitBreakerState.HalfOpen)
            {
                _state = CircuitBreakerState.Closed;
                _failureCount = 0;
                Console.WriteLine("[CIRCUIT] Probe succeeded. Changed to Closed.");
            }
            else if (_state == CircuitBreakerState.Closed)
            {
                _failureCount = 0;
            }
        }
    }

    public void RecordFailure()
    {
        lock (_stateLock)
        {
            if (_state == CircuitBreakerState.HalfOpen)
            {
                _state = CircuitBreakerState.Open;
                _nextAttemptTime = DateTime.UtcNow.AddSeconds(_options.CircuitBreakerBreakDurationSeconds);
                Console.WriteLine($"[CIRCUIT] Probe failed. Changed back to Open until {_nextAttemptTime:T}.");
                return;
            }

            _failureCount++;

            if (_failureCount >= _options.CircuitBreakerFailureThreshold)
            {
                _state = CircuitBreakerState.Open;
                _nextAttemptTime = DateTime.UtcNow.AddSeconds(_options.CircuitBreakerBreakDurationSeconds);
                Console.WriteLine($"[CIRCUIT] Threshold reached ({_failureCount} failures). Changed to Open until {_nextAttemptTime:T}.");
            }
        }
    }
}
```

---

### `Persistence/ResilientProductStore.cs`

```csharp
using Microsoft.Extensions.Options;
using ProductManagementSystem.Models;
using ProductManagementSystem.Options;
using ProductManagementSystem.Services.Resilience;

namespace ProductManagementSystem.Persistence;

/// <summary>
/// Decorator pattern. Wraps the raw IProductDataStore with retry and circuit breaker logic.
/// </summary>
public sealed class ResilientProductStore : IProductStore
{
    private readonly IProductDataStore _innerStore;
    private readonly ICircuitBreaker _circuitBreaker;
    private readonly ResilienceOptions _options;

    public ResilientProductStore(
        IProductDataStore innerStore,
        ICircuitBreaker circuitBreaker,
        IOptions<ResilienceOptions> options)
    {
        _innerStore = innerStore;
        _circuitBreaker = circuitBreaker;
        _options = options.Value;
    }

    public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(() => _innerStore.GetAllAsync(cancellationToken));
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(() => _innerStore.GetByIdAsync(id, cancellationToken));
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(() => _innerStore.CreateAsync(product, cancellationToken));
    }

    public async Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(() => _innerStore.UpdateAsync(product, cancellationToken));
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithResilienceAsync(() => _innerStore.DeleteAsync(id, cancellationToken));
    }

    private async Task<T> ExecuteWithResilienceAsync<T>(Func<Task<T>> action)
    {
        _circuitBreaker.EnsureExecutionAllowed();

        int maxAttempts = _options.MaxRetries + 1;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                T result = await action();
                _circuitBreaker.RecordSuccess();
                return result;
            }
            catch (TimeoutException ex)
            {
                _circuitBreaker.RecordFailure();

                if (attempt == maxAttempts)
                {
                    throw new Exception($"Operation failed after {maxAttempts} attempts due to timeouts.", ex);
                }

                Console.WriteLine($"[RETRY] Timeout on attempt {attempt}. Retrying in {_options.RetryDelayMilliseconds}ms...");
                await Task.Delay(_options.RetryDelayMilliseconds);
            }
            // We do NOT catch ArgumentException or others. Let them fail fast.
        }

        throw new InvalidOperationException("Unreachable code.");
    }
}
```
