# Product Management System (Core)

This solution stays strictly inside your boundaries:
- **Domain:** Product Management System only
- **Persistence:** **in-memory collections only**
- **Architecture:** **Model + DTO + Service + Controller + Middleware**
- **Engineering:** **SOLID + Options Pattern + Retry + Circuit Breaker + Idempotency**
- **UI:** **single HTML management page**, minimal, calm, pastel
- **Practicality:** immediately usable as a starter real-world ASP.NET Core app

Think of the app like a small warehouse:
- **Controller** = front desk receptionist taking requests
- **Service** = operations manager applying rules
- **Store** = stock room holding products in memory
- **Middleware** = hallway security/camera/error guard
- **Retry** = redialing after a temporary call glitch
- **Circuit breaker** = electrical fuse that opens when failures pile up
- **Idempotency-Key** = stamped receipt that prevents duplicate creation if the client retries

---

## 1) `ProductManagementSystem.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

---

## 2) `Program.cs`

```csharp
using ProductManagementSystem.Middleware;
using ProductManagementSystem.Options;
using ProductManagementSystem.Persistence;
using ProductManagementSystem.Services;
using ProductManagementSystem.Services.Diagnostics;
using ProductManagementSystem.Services.Idempotency;
using ProductManagementSystem.Services.Resilience;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Keep JSON property names exactly as the C# property names:
        // ID, ProdName, Price, Brand
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services
    .AddOptions<ResilienceOptions>()
    .Bind(builder.Configuration.GetSection(ResilienceOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<StoreSimulationOptions>()
    .Bind(builder.Configuration.GetSection(StoreSimulationOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Core services
builder.Services.AddScoped<IProductService, ProductService>();

// Persistence chain:
// ProductService -> IProductStore (resilient wrapper) -> IProductDataStore (raw in-memory store)
builder.Services.AddSingleton<IProductDataStore, InMemoryProductDataStore>();
builder.Services.AddSingleton<ICircuitBreaker, InMemoryCircuitBreaker>();
builder.Services.AddSingleton<IProductStore, ResilientProductStore>();

// Idempotency must be shared application-wide
builder.Services.AddSingleton<IIdempotencyStore, InMemoryIdempotencyStore>();

// Scoped diagnostic service used by middleware through InvokeAsync
builder.Services.AddScoped<IRequestAuditService, RequestAuditService>();

var app = builder.Build();

// Order matters:
// 1. Logging wraps the whole pipeline
// 2. Exception handling converts exceptions into clean HTTP responses
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
```

---

## 3) `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Resilience": {
    "MaxRetries": 2,
    "RetryDelayMilliseconds": 300,
    "CircuitBreakerFailureThreshold": 3,
    "CircuitBreakerBreakDurationSeconds": 15
  },
  "StoreSimulation": {
    "EnableTransientTimeouts": false,
    "TimeoutEveryNthWrite": 3
  }
}
```
