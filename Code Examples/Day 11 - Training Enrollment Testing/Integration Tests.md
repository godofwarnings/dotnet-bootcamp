# Integration Tests (Day 11)

This file contains the final state of the Integration Testing setup using `WebApplicationFactory`, which spins up the test server in-memory to test the endpoints end-to-end.

### `TestProject1/CoursesApiTests.cs`
```csharp
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using TrainingEnrollmentSolution.Models;

public class CoursesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public CoursesApiTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllCourses_ShouldReturnOkAndCourseList()
    {
        // Act
        HttpResponseMessage response = await client.GetAsync("/api/courses");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        List<Course>? courses = await response.Content.ReadFromJsonAsync<List<Course>>();

        courses.Should().NotBeNull();
        courses!.Should().NotBeEmpty();
        courses.Should().Contain(course =>
            course.CourseName == "ASP.NET Core Web API");
    }

    [Fact]
    public async Task GetCourseById_WithValidId_ShouldReturnOkAndCourse()
    {
        // Act
        HttpResponseMessage response = await client.GetAsync("/api/courses/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        Course? course = await response.Content.ReadFromJsonAsync<Course>();

        course.Should().NotBeNull();
        course!.CourseId.Should().Be(1);
        course.CourseName.Should().Be("ASP.NET Core Web API");
        course.Fee.Should().Be(15000);
        course.DurationInDays.Should().Be(5);
    }

    [Fact]
    public async Task GetCourseById_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        HttpResponseMessage response = await client.GetAsync("/api/courses/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        string responseText = await response.Content.ReadAsStringAsync();
        responseText.Should().Be("Course not found.");
    }
}
```

### `Program.cs` 
*(Notice the `public partial class Program` at the bottom which is required for integration testing)*

```csharp
using TrainingEnrollmentSolution.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register services
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<ICourseCompletionService, CourseCompletionService>();

var app = builder.Build();

app.MapControllers();

app.Run();

// Required so the TestProject can access WebApplicationFactory<Program>
public partial class Program
{
}
```
