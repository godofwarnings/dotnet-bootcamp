# Dependency Injection (DI)

Dependency Injection (DI) is a way of giving an object the things it needs from the outside instead of letting it build those things itself. In .NET and ASP.NET Core, this is built into the framework: you register services once, the container creates them, injects them where needed, and disposes them at the right time.

The big payoff is looser coupling, easier testing, simpler replacement of implementations, and cleaner Web API code.

## 1. The Core Concept (Inversion of Control)
If a class creates its own dependencies, it becomes tightly tied to specific implementations. Hard-coded dependencies are problematic because replacing an implementation requires changing the consumer, configuration gets scattered, and the code becomes difficult to unit test.

**Simple Analogy (The Restaurant Kitchen):**
- **Without DI:** Every chef buys their own pans, knives, and ingredients before cooking.
- **With DI:** The kitchen supplies the tools and ingredients, and the chef focuses on cooking.

That is the core idea of DI: separate "doing the work" from "obtaining the tools."

### Main advantages of using DI
- **Loose coupling:** Your class depends on an abstraction (like an `interface`), not a concrete implementation.
- **Easier testing:** You can swap real services for fakes or mocks.
- **Easier maintenance:** Changing one implementation does not require rewriting all consumers.
- **Centralized configuration:** Service creation is registered in one place, usually app startup.
- **Lifecycle management:** The framework can create and dispose services automatically.

## 2. DI in ASP.NET Core

The normal flow is:
1. Register services in `Program.cs`.
2. ASP.NET Core stores them in the service container.
3. When a controller or service is created, the framework inspects its constructor.
4. It automatically supplies the requested dependencies (Constructor Injection).

### Example
Take a look at the provided examples in `Code Examples/Day 7 - Web API/Dependency Injection/`:
- [IStudentService.cs](../../Code%20Examples/Day%207%20-%20Web%20API/Dependency%20Injection/IStudentService.cs) is the abstraction.
- [StudentService.cs](../../Code%20Examples/Day%207%20-%20Web%20API/Dependency%20Injection/StudentService.cs) is the implementation.
- [DI_Program.cs](../../Code%20Examples/Day%207%20-%20Web%20API/Dependency%20Injection/DI_Program.cs) shows the registration (`builder.Services.AddScoped<IStudentService, StudentService>();`).
- [StudentsController.cs](../../Code%20Examples/Day%207%20-%20Web%20API/Dependency%20Injection/StudentsController.cs) consumes the service via Constructor Injection.

## 3. Service Lifetimes

Microsoft defines three main lifetimes in ASP.NET Core:

- `AddTransient`: A new instance is created every time it is requested.
  > **Analogy:** Disposable coffee cup. New one each time.
- `AddScoped`: One instance is created per client HTTP request.
  > **Analogy:** One meal ticket per customer visit. Shared during that visit only.
- `AddSingleton`: One shared instance for the whole application lifetime.
  > **Analogy:** The restaurant building itself. One for everyone, all day.

### Important rule about lifetimes
Do **not** resolve a scoped service directly from a singleton, because that effectively makes the scoped service behave like a singleton and can cause incorrect state across requests.
- Controllers $\rightarrow$ scoped/transient services: **fine**.
- Scoped service $\rightarrow$ singleton service: **fine**.
- Singleton $\rightarrow$ scoped: **not fine** unless you create an explicit scope properly.

> [!WARNING]
> **The Middleware Constructor Anti-Pattern**
> In ASP.NET Core, standard middleware is created once per application lifetime (effectively acting as a Singleton). If you inject a **Scoped** service into the constructor of a middleware, it will be captured and behave like a singleton for the rest of the application's life. This is a classic "Lifecycle Mismatch" error.
> 
> **The Fix:** Inject scoped services into the `InvokeAsync` method instead of the constructor, so they are resolved per HTTP request.

## 4. EF Core with Dependency Injection

Entity Framework Core integrates seamlessly into this model. You configure your `DbContext` via DI so that it can be injected into your services or repositories. 
Below is the complete step-by-step process for integrating EF Core into an ASP.NET Core Web API using DI.

### What you are building

You need to create:

- `Product` model
- `IProductService` with:
  - `GetProducts()`
  - `GetProductById(int id)`
- `ProductService` implementation using EF Core
- DI registration in `Program.cs`
- `ProductsController` using constructor injection
- Test both APIs in Postman

We will use the simplest and most standard EF Core approach using SQL Server LocalDB in Visual Studio.

### Step-by-step in Visual Studio

#### Step 1: Create the project

In Visual Studio:

1. Click `Create a new project`.
2. Choose `ASP.NET Core Web API`.
3. Name it something like `ProductApiEF`.
4. Click `Next`.
5. Choose the latest .NET version available.
6. Keep `Use controllers` checked.
7. Click `Create`.

#### Step 2: Install the required NuGet packages

In Visual Studio:

1. Right-click the project.
2. Click `Manage NuGet Packages`.
3. Install these packages:
   - `Microsoft.EntityFrameworkCore.SqlServer`
   - `Microsoft.EntityFrameworkCore.Tools`

Why? EF Core needs the SQL Server provider to talk to SQL Server, and the tools package is used for migrations.

#### Step 3: Create the folders

Create these folders in the project:

- `Models`
- `Data`
- `Services`
- `Controllers` (already exists in most Web API templates)

#### Step 4: Create the Product model

Create file: `Models/Product.cs`

```csharp
namespace ProductApiEF.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;
    }
}
```

What this does:

- `ProductId` becomes the primary key by EF convention.
- Each object becomes one row in the `Products` table.

#### Step 5: Create the DbContext

Create file: `Data/ApplicationDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using ProductApiEF.Models;

namespace ProductApiEF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
```

Why this is important:

- EF Core requires a `DbContext` subclass.
- Microsoft documents that the context should expose a constructor accepting `DbContextOptions` so configuration from `AddDbContext` can be passed into it.
- `DbSet` represents the `Products` table.

#### Step 6: Create the service interface

Create file: `Services/IProductService.cs`

```csharp
using ProductApiEF.Models;

namespace ProductApiEF.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product? GetProductById(int productId);
    }
}
```

This is the contract. Your controller will depend on this interface, not on a concrete class directly. That is the core DI idea in ASP.NET Core.

#### Step 7: Create the EF implementation

Create file: `Services/ProductService.cs`

```csharp
using ProductApiEF.Data;
using ProductApiEF.Models;

namespace ProductApiEF.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public Product? GetProductById(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId);
        }
    }
}
```

What is happening here:

- `ProductService` depends on `ApplicationDbContext`.
- ASP.NET Core injects that context through the constructor.
- `GetProducts()` returns all products.
- `GetProductById()` returns one product or `null`.

#### Step 8: Add a connection string

Open `appsettings.json` and add:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ProductDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "AllowedHosts": "*"
}
```

This tells EF Core where your SQL Server database should be created.

#### Step 9: Configure DI and EF in Program.cs

Replace your `Program.cs` with this:

```csharp
using Microsoft.EntityFrameworkCore;
using ProductApiEF.Data;
using ProductApiEF.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

Why this works:

- `AddDbContext(...)` registers your EF context in DI.
- ASP.NET Core DI supports registering services and then injecting them into controllers.
