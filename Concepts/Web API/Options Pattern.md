# The Options Pattern

The Options pattern in ASP.NET Core uses classes to provide strongly typed access to groups of related settings. It is the professional way to read configuration values from `appsettings.json`.

## Why use it?
Instead of manually injecting `IConfiguration` into your services and calling `_configuration.GetValue<int>("RetryCount")` everywhere, you bind a specific section of your JSON to a C# class. This provides:
- **Strong Typing**: No magic strings.
- **Validation**: You can validate the configuration at startup.
- **Separation of Concerns**: Services only depend on the specific settings they need, not the entire configuration tree.

## Basic Setup

**1. Define the class**
```csharp
public class RetryOptions
{
    public const string SectionName = "RetrySettings";
    public int MaxRetries { get; set; }
}
```

**2. Add to appsettings.json**
```json
{
  "RetrySettings": {
    "MaxRetries": 3
  }
}
```

**3. Bind in Program.cs**
```csharp
builder.Services.Configure<RetryOptions>(
    builder.Configuration.GetSection(RetryOptions.SectionName));
```

**4. Inject into your service**
```csharp
public class FeeService
{
    private readonly int _maxRetries;

    public FeeService(IOptions<RetryOptions> options)
    {
        _maxRetries = options.Value.MaxRetries;
    }
}
```
