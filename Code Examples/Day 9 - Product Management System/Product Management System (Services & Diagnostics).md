### `Services/IProductService.cs`

```csharp
using ProductManagementSystem.DTOs;

namespace ProductManagementSystem.Services;

public interface IProductService
{
    Task<IReadOnlyCollection<ProductResponseDto>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<ProductResponseDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
    
    Task<ProductResponseDto> CreateProductAsync(
        CreateProductRequestDto request, 
        string idempotencyKey,
        CancellationToken cancellationToken = default);
        
    Task<ProductResponseDto?> UpdateProductAsync(
        int id, 
        UpdateProductRequestDto request, 
        CancellationToken cancellationToken = default);
        
    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default);
}
```

---

### `Services/ProductService.cs`

```csharp
using ProductManagementSystem.DTOs;
using ProductManagementSystem.Models;
using ProductManagementSystem.Persistence;
using ProductManagementSystem.Services.Idempotency;

namespace ProductManagementSystem.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductStore _productStore;
    private readonly IIdempotencyStore _idempotencyStore;

    public ProductService(
        IProductStore productStore,
        IIdempotencyStore idempotencyStore)
    {
        _productStore = productStore;
        _idempotencyStore = idempotencyStore;
    }

    public async Task<IReadOnlyCollection<ProductResponseDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productStore.GetAllAsync(cancellationToken);
        
        return products.Select(MapToResponse).ToList();
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productStore.GetByIdAsync(id, cancellationToken);
        
        return product is null ? null : MapToResponse(product);
    }

    public async Task<ProductResponseDto> CreateProductAsync(
        CreateProductRequestDto request, 
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(idempotencyKey);

        var requestHash = RequestHasher.ComputeHash(request);

        // Check idempotency store first
        var idempotencyResult = _idempotencyStore.CheckOrReserve(idempotencyKey, requestHash);

        if (idempotencyResult.IsDuplicateWithSamePayload && idempotencyResult.ResponseData is ProductResponseDto duplicateResponse)
        {
            return duplicateResponse;
        }

        if (idempotencyResult.IsDuplicateWithDifferentPayload)
        {
            throw new InvalidOperationException("A request with this Idempotency-Key was already processed with a different payload.");
        }

        var product = new Product
        {
            ProdName = request.ProdName,
            Price = request.Price,
            Brand = request.Brand
        };

        var created = await _productStore.CreateAsync(product, cancellationToken);
        
        var response = MapToResponse(created);

        // Save result to idempotency store
        _idempotencyStore.CompleteReservation(idempotencyKey, response);

        return response;
    }

    public async Task<ProductResponseDto?> UpdateProductAsync(
        int id, 
        UpdateProductRequestDto request, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = new Product
        {
            ID = id,
            ProdName = request.ProdName,
            Price = request.Price,
            Brand = request.Brand
        };

        var updated = await _productStore.UpdateAsync(product, cancellationToken);
        
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _productStore.DeleteAsync(id, cancellationToken);
    }

    private static ProductResponseDto MapToResponse(Product product)
    {
        return new ProductResponseDto
        {
            ID = product.ID,
            ProdName = product.ProdName,
            Price = product.Price,
            Brand = product.Brand
        };
    }
}
```

---

### `Services/Diagnostics/IRequestAuditService.cs`

```csharp
namespace ProductManagementSystem.Services.Diagnostics;

public interface IRequestAuditService
{
    void BeginAudit(string correlationId, string method, string path);
    void EndAudit(int statusCode);
    string GetCurrentCorrelationId();
}
```

---

### `Services/Diagnostics/RequestAuditService.cs`

```csharp
namespace ProductManagementSystem.Services.Diagnostics;

public sealed class RequestAuditService : IRequestAuditService
{
    private string _correlationId = string.Empty;
    private string _method = string.Empty;
    private string _path = string.Empty;
    private DateTime _startTime;

    public void BeginAudit(string correlationId, string method, string path)
    {
        _correlationId = correlationId;
        _method = method;
        _path = path;
        _startTime = DateTime.UtcNow;

        Console.WriteLine($"[AUDIT START] {_correlationId} | {_method} {_path}");
    }

    public void EndAudit(int statusCode)
    {
        var duration = DateTime.UtcNow - _startTime;
        Console.WriteLine($"[AUDIT END] {_correlationId} | Status: {statusCode} | Duration: {duration.TotalMilliseconds:F1}ms");
    }

    public string GetCurrentCorrelationId()
    {
        return _correlationId;
    }
}
```
