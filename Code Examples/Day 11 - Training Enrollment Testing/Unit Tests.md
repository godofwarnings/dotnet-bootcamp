# Unit Tests (Day 11)

This file contains the final, combined Unit Testing project using xUnit, FluentAssertions, and Moq.

### `TestProject1/UnitTest1.cs`
*(Original unit tests for CourseService and EnrollmentService)*
```csharp
using FluentAssertions;
using Moq;
using TrainingEnrollmentSolution.DTOs;
using TrainingEnrollmentSolution.Models;
using TrainingEnrollmentSolution.Services;

public class UnitTest1
{
    [Fact]
    public void GetAllCourses_ShouldReturnAvailableCourses()
    {
        // Arrange
        CourseService courseService = new CourseService();

        // Act
        var courses = courseService.GetAllCourses();

        // Assert
        courses.Should().NotBeNull();
        courses.Should().NotBeEmpty();
        courses.Should().HaveCount(2);

        courses.Should().Contain(course =>
            course.CourseId == 1 &&
            course.CourseName == "ASP.NET Core Web API");

        courses.Should().Contain(course =>
            course.CourseId == 2 &&
            course.CourseName == "Azure DevOps");
    }

    [Theory]
    [InlineData(1, "ASP.NET Core Web API")]
    [InlineData(2, "Azure DevOps")]
    public void GetCourseById_WithValidCourseId_ShouldReturnMatchingCourse(
        int courseId,
        string expectedCourseName)
    {
        // Arrange
        CourseService courseService = new CourseService();

        // Act
        var course = courseService.GetCourseById(courseId);

        // Assert
        course.Should().NotBeNull();
        course!.CourseId.Should().Be(courseId);
        course.CourseName.Should().Be(expectedCourseName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(999)]
    public void GetCourseById_WithInvalidCourseId_ShouldReturnNull(int courseId)
    {
        // Arrange
        CourseService courseService = new CourseService();

        // Act
        var course = courseService.GetCourseById(courseId);

        // Assert
        course.Should().BeNull();
    }

    [Fact]
    public async Task CreateEnrollmentAsync_WithValidRequest_ShouldCreateEnrollment()
    {
        Mock<ICourseService> courseServiceMock = new Mock<ICourseService>();

        courseServiceMock
            .Setup(service => service.GetCourseById(1))
            .Returns(new Course
            {
                CourseId = 1,
                CourseName = "ASP.NET Core Web API",
                Fee = 15000,
                DurationInDays = 5
            });

        Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();

        notificationServiceMock
            .Setup(service => service.SendEnrollmentNotificationAsync(10, 1))
            .Returns(Task.CompletedTask);

        EnrollmentService enrollmentService = new EnrollmentService(
            courseServiceMock.Object,
            notificationServiceMock.Object);

        CreateEnrollmentRequest request = new CreateEnrollmentRequest
        {
            StudentId = 10,
            CourseId = 1
        };

        Enrollment enrollment = await enrollmentService.CreateEnrollmentAsync(request);

        enrollment.StudentId.Should().Be(10);
        enrollment.CourseId.Should().Be(1);
        enrollment.Status.Should().Be("Enrolled");

        notificationServiceMock.Verify(
            service => service.SendEnrollmentNotificationAsync(10, 1),
            Times.Once);
    }
}
```

### `TestProject1/CourseCompletionServiceTests.cs`
*(Added during incremental evolution to test the CourseCompletionService)*
```csharp
using FluentAssertions;
using Moq;
using TrainingEnrollmentSolution.Models;
using TrainingEnrollmentSolution.Services;

public class CourseCompletionServiceTests
{
    [Fact]
    public async Task CompleteCourseAsync_WithValidInput_ShouldReturnCourse_AndSendNotification()
    {
        // Arrange
        Mock<ICourseService> courseServiceMock = new Mock<ICourseService>();
        Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();

        Course expectedCourse = new Course
        {
            CourseId = 1,
            CourseName = "ASP.NET Core Web API",
            Fee = 15000,
            DurationInDays = 5
        };

        courseServiceMock
            .Setup(service => service.GetCourseById(1))
            .Returns(expectedCourse);

        notificationServiceMock
            .Setup(service => service.SendEnrollmentNotificationAsync(10, 1))
            .Returns(Task.CompletedTask);

        CourseCompletionService courseCompletionService = new CourseCompletionService(
            courseServiceMock.Object,
            notificationServiceMock.Object);

        // Act
        Course result = await courseCompletionService.CompleteCourseAsync(10, 1);

        // Assert
        result.Should().NotBeNull();
        result.CourseId.Should().Be(1);
        result.CourseName.Should().Be("ASP.NET Core Web API");

        courseServiceMock.Verify(service => service.GetCourseById(1), Times.Once);

        notificationServiceMock.Verify(
            service => service.SendEnrollmentNotificationAsync(10, 1),
            Times.Once);
    }

    [Fact]
    public async Task CompleteCourseAsync_WithInvalidStudentId_ShouldThrowArgumentException()
    {
        // Arrange
        Mock<ICourseService> courseServiceMock = new Mock<ICourseService>();
        Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();

        CourseCompletionService courseCompletionService = new CourseCompletionService(
            courseServiceMock.Object,
            notificationServiceMock.Object);

        // Act
        Func<Task> act = async () => await courseCompletionService.CompleteCourseAsync(0, 1);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid student id.");

        courseServiceMock.Verify(
            service => service.GetCourseById(It.IsAny<int>()),
            Times.Never);

        notificationServiceMock.Verify(
            service => service.SendEnrollmentNotificationAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task CompleteCourseAsync_WithInvalidCourseId_ShouldThrowArgumentException()
    {
        // Arrange
        Mock<ICourseService> courseServiceMock = new Mock<ICourseService>();
        Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();

        CourseCompletionService courseCompletionService = new CourseCompletionService(
            courseServiceMock.Object,
            notificationServiceMock.Object);

        // Act
        Func<Task> act = async () => await courseCompletionService.CompleteCourseAsync(10, 0);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid course id.");

        courseServiceMock.Verify(
            service => service.GetCourseById(It.IsAny<int>()),
            Times.Never);

        notificationServiceMock.Verify(
            service => service.SendEnrollmentNotificationAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public async Task CompleteCourseAsync_WhenCourseNotFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Mock<ICourseService> courseServiceMock = new Mock<ICourseService>();
        Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();

        courseServiceMock
            .Setup(service => service.GetCourseById(999))
            .Returns((Course?)null);

        CourseCompletionService courseCompletionService = new CourseCompletionService(
            courseServiceMock.Object,
            notificationServiceMock.Object);

        // Act
        Func<Task> act = async () => await courseCompletionService.CompleteCourseAsync(10, 999);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Course not found.");

        courseServiceMock.Verify(service => service.GetCourseById(999), Times.Once);

        notificationServiceMock.Verify(
            service => service.SendEnrollmentNotificationAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Never);
    }
}

### `TestProject1/CoursesControllerTests.cs`
*(Added during incremental evolution to test mocking the controller)*
```csharp
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TrainingEnrollmentSolution.Controllers;
using TrainingEnrollmentSolution.Models;
using TrainingEnrollmentSolution.Services;

public class CoursesControllerTests
{
    [Fact]
    public void GetAllCourses_ShouldReturnOk_WithJavaCourse()
    {
        // Arrange
        Mock<ICourseService> courseServiceMock = new Mock<ICourseService>();

        List<Course> courses = new List<Course>
        {
            new Course
            {
                CourseId = 3,
                CourseName = "Java",
                Fee = 10000,
                DurationInDays = 6
            }
        };

        courseServiceMock
            .Setup(service => service.GetAllCourses())
            .Returns(courses);

        CoursesController controller = new CoursesController(courseServiceMock.Object);

        // Act
        IActionResult result = controller.GetAllCourses();

        // Assert
        OkObjectResult okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        List<Course> returnedCourses = okResult.Value
            .Should()
            .BeAssignableTo<List<Course>>()
            .Subject;

        returnedCourses.Should().HaveCount(1);
        returnedCourses[0].CourseId.Should().Be(3);
        returnedCourses[0].CourseName.Should().Be("Java");
        returnedCourses[0].Fee.Should().Be(10000);
        returnedCourses[0].DurationInDays.Should().Be(6);

        courseServiceMock.Verify(service => service.GetAllCourses(), Times.Once);
    }
}
```
