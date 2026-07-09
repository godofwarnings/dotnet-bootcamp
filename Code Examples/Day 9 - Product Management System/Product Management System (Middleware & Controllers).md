### `Middleware/RequestLoggingMiddleware.cs`

```csharp
using ProductManagementSystem.Services.Diagnostics;

namespace ProductManagementSystem.Middleware;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // IMPORTANT: Scoped services must be injected into InvokeAsync, NOT the constructor.
    // If injected in the constructor, they act as singletons (Middleware Constructor Anti-Pattern).
    public async Task InvokeAsync(HttpContext context, IRequestAuditService requestAuditService)
    {
        var correlationId = Guid.NewGuid().ToString("N");
        
        requestAuditService.BeginAudit(
            correlationId, 
            context.Request.Method, 
            context.Request.Path);

        // Add correlation ID to response headers for the client
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Correlation-ID"] = correlationId;
            return Task.CompletedTask;
        });

        await _next(context);

        requestAuditService.EndAudit(context.Response.StatusCode);
    }
}
```

---

### `Middleware/ExceptionHandlingMiddleware.cs`

```csharp
using System.Text.Json;
using ProductManagementSystem.Services.Diagnostics;
using ProductManagementSystem.Services.Resilience;

namespace ProductManagementSystem.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IRequestAuditService auditService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, auditService.GetCurrentCorrelationId());
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, string correlationId)
    {
        var (statusCode, message) = exception switch
        {
            ArgumentException argEx => (StatusCodes.Status400BadRequest, argEx.Message),
            InvalidOperationException invEx => (StatusCodes.Status409Conflict, invEx.Message),
            CircuitBreakerOpenException cbEx => (StatusCodes.Status503ServiceUnavailable, cbEx.Message),
            TimeoutException timeoutEx => (StatusCodes.Status504GatewayTimeout, timeoutEx.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            // In a real app, log the full stack trace securely here.
            Console.WriteLine($"[ERROR] {correlationId} | {exception.Message}");
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            Error = message,
            CorrelationId = correlationId
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
```

---

### `Controllers/ProductsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.DTOs;
using ProductManagementSystem.Services;

namespace ProductManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllProductsAsync(cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return NotFound(new { Message = $"Product with ID {id} not found." });
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        [FromBody] CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return BadRequest(new { Error = "Idempotency-Key header is required." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _productService.CreateProductAsync(request, idempotencyKey, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = response.ID }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductResponseDto>> Update(
        int id,
        [FromBody] UpdateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _productService.UpdateProductAsync(id, request, cancellationToken);

        if (response is null)
        {
            return NotFound(new { Message = $"Product with ID {id} not found." });
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteProductAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound(new { Message = $"Product with ID {id} not found." });
        }

        return NoContent();
    }
}
```
