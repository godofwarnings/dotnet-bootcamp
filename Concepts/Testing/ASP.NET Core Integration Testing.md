# ASP.NET Core Integration Testing

In ASP.NET Core, integration testing allows you to test your API endpoints by spinning up the application in-memory, without needing to host it on a real web server like IIS or Kestrel.

## Required NuGet Packages
To run integration tests, the test project needs the following packages:
```bash
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package FluentAssertions
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
```

## The WebApplicationFactory Setup
We use the `WebApplicationFactory<TEntryPoint>` class to bootstrap the application in memory. 

### The `Program.cs` Visibility Fix
For `WebApplicationFactory<Program>` to work properly, your `Program` class must be visible to the test project.
If you are using top-level statements (which is the default in modern .NET), the `Program` class is internal and implicitly generated. 

To fix this, you must add a `public partial class` declaration at the **bottom** of your `Program.cs` file:

```csharp
// ... normal Program.cs setup ...
var app = builder.Build();
app.MapControllers();
app.Run();

// Add this so the test project can see the Program class
public partial class Program
{
}
```

### Example Integration Test Class
```csharp
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

public class CoursesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public CoursesApiTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllCourses_ShouldReturnOk()
    {
        // Act
        HttpResponseMessage response = await client.GetAsync("/api/courses");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```
