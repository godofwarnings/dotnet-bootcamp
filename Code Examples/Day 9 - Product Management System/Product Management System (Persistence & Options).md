### `Options/ResilienceOptions.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.Options;

public sealed class ResilienceOptions
{
    public const string SectionName = "Resilience";

    [Range(0, 10)]
    public int MaxRetries { get; set; } = 2;

    [Range(10, 5000)]
    public int RetryDelayMilliseconds { get; set; } = 300;

    [Range(1, 100)]
    public int CircuitBreakerFailureThreshold { get; set; } = 3;

    [Range(1, 300)]
    public int CircuitBreakerBreakDurationSeconds { get; set; } = 15;
}
```

---

### `Options/StoreSimulationOptions.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.Options;

public sealed class StoreSimulationOptions
{
    public const string SectionName = "StoreSimulation";

    public bool EnableTransientTimeouts { get; set; } = false;

    [Range(1, 1000)]
    public int TimeoutEveryNthWrite { get; set; } = 3;
}
```

---

### `Persistence/IProductDataStore.cs`

```csharp
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Persistence;

public interface IProductDataStore
{
    Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
```

---

### `Persistence/IProductStore.cs`

```csharp
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Persistence;

public interface IProductStore
{
    Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
```

---

### `Persistence/InMemoryProductDataStore.cs`

```csharp
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using ProductManagementSystem.Models;
using ProductManagementSystem.Options;

namespace ProductManagementSystem.Persistence;

public sealed class InMemoryProductDataStore : IProductDataStore
{
    private readonly ConcurrentDictionary<int, Product> _products = new();
    private readonly IOptions<StoreSimulationOptions> _storeOptions;

    private int _currentId;
    private int _writeCount;

    public InMemoryProductDataStore(IOptions<StoreSimulationOptions> storeOptions)
    {
        _storeOptions = storeOptions;

        SeedProducts();
    }

    public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyCollection<Product> products = _products
            .OrderBy(pair => pair.Key)
            .Select(pair => Clone(pair.Value))
            .ToList();

        return Task.FromResult(products);
    }

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(
            _products.TryGetValue(id, out var product)
                ? Clone(product)
                : null);
    }

    public Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ValidateProduct(product);
        MaybeThrowTransientTimeoutOnWrite();

        var newId = Interlocked.Increment(ref _currentId);

        var stored = new Product
        {
            ID = newId,
            ProdName = product.ProdName.Trim(),
            Price = product.Price,
            Brand = product.Brand.Trim()
        };

        _products[newId] = stored;

        return Task.FromResult(Clone(stored));
    }

    public Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_products.ContainsKey(product.ID))
        {
            return Task.FromResult<Product?>(null);
        }

        ValidateProduct(product);
        MaybeThrowTransientTimeoutOnWrite();

        var updated = new Product
        {
            ID = product.ID,
            ProdName = product.ProdName.Trim(),
            Price = product.Price,
            Brand = product.Brand.Trim()
        };

        _products[product.ID] = updated;

        return Task.FromResult<Product?>(Clone(updated));
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        MaybeThrowTransientTimeoutOnWrite();

        return Task.FromResult(_products.TryRemove(id, out _));
    }

    private void SeedProducts()
    {
        AddSeedProduct("Laptop", 1499.99m, "Lenovo");
        AddSeedProduct("Phone", 899.50m, "Samsung");
        AddSeedProduct("Headphones", 199.99m, "Sony");
    }

    private void AddSeedProduct(string prodName, decimal price, string brand)
    {
        var newId = Interlocked.Increment(ref _currentId);

        _products[newId] = new Product
        {
            ID = newId,
            ProdName = prodName,
            Price = price,
            Brand = brand
        };
    }

    private void ValidateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.ProdName))
        {
            throw new ArgumentException("ProdName is required.");
        }

        if (product.Price <= 0)
        {
            throw new ArgumentException("Price must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(product.Brand))
        {
            throw new ArgumentException("Brand is required.");
        }
    }

    private void MaybeThrowTransientTimeoutOnWrite()
    {
        var options = _storeOptions.Value;

        if (!options.EnableTransientTimeouts || options.TimeoutEveryNthWrite <= 0)
        {
            return;
        }

        var writeNumber = Interlocked.Increment(ref _writeCount);

        if (writeNumber % options.TimeoutEveryNthWrite == 0)
        {
            throw new TimeoutException("Simulated transient timeout in the in-memory product store.");
        }
    }

    private static Product Clone(Product product)
    {
        return new Product
        {
            ID = product.ID,
            ProdName = product.ProdName,
            Price = product.Price,
            Brand = product.Brand
        };
    }
}
```
