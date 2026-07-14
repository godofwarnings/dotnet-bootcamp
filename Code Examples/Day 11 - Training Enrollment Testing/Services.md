# Services (Day 11)

This file contains the final, combined services for the Training Enrollment application, including the incremental `CourseCompletionService`.

### `Services/ICourseService.cs`
```csharp
using TrainingEnrollmentSolution.Models;

namespace TrainingEnrollmentSolution.Services
{
    public interface ICourseService
    {
        List<Course> GetAllCourses();
        Course? GetCourseById(int courseId);
    }
}
```

### `Services/CourseService.cs`
```csharp
using TrainingEnrollmentSolution.Models;
using TrainingEnrollmentSolution.Services;

public class CourseService : ICourseService
{
    private static readonly List<Course> courses = new List<Course>
    {
        new Course
        {
            CourseId = 1,
            CourseName = "ASP.NET Core Web API",
            Fee = 15000,
            DurationInDays = 5
        },
        new Course
        {
            CourseId = 2,
            CourseName = "Azure DevOps",
            Fee = 12000,
            DurationInDays = 4
        }
    };

    public List<Course> GetAllCourses()
    {
        return courses;
    }

    public Course? GetCourseById(int courseId)
    {
        return courses.FirstOrDefault(c => c.CourseId == courseId);
    }
}
```

### `Services/IEnrollmentService.cs`
```csharp
using TrainingEnrollmentSolution.DTOs;
using TrainingEnrollmentSolution.Models;

namespace TrainingEnrollmentSolution.Services
{
    public interface IEnrollmentService
    {
        Task<Enrollment> CreateEnrollmentAsync(CreateEnrollmentRequest request);
        List<Enrollment> GetAllEnrollments();
    }
}
```

### `Services/EnrollmentService.cs`
```csharp
using TrainingEnrollmentSolution.DTOs;
using TrainingEnrollmentSolution.Models;
using TrainingEnrollmentSolution.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly ICourseService courseService;
    private readonly INotificationService notificationService;

    private static readonly List<Enrollment> enrollments = new List<Enrollment>();
    private static int nextEnrollmentId = 1001;

    public EnrollmentService(
        ICourseService courseService,
        INotificationService notificationService)
    {
        this.courseService = courseService;
        this.notificationService = notificationService;
    }

    public async Task<Enrollment> CreateEnrollmentAsync(CreateEnrollmentRequest request)
    {
        if (request.StudentId <= 0)
        {
            throw new ArgumentException("Invalid student id.");
        }

        if (request.CourseId <= 0)
        {
            throw new ArgumentException("Invalid course id.");
        }

        Course? course = courseService.GetCourseById(request.CourseId);

        if (course == null)
        {
            throw new InvalidOperationException("Course not found.");
        }

        bool alreadyEnrolled = enrollments.Any(e =>
            e.StudentId == request.StudentId &&
            e.CourseId == request.CourseId);

        if (alreadyEnrolled)
        {
            throw new InvalidOperationException("Student is already enrolled in this course.");
        }

        Enrollment enrollment = new Enrollment
        {
            EnrollmentId = nextEnrollmentId,
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            Status = "Enrolled",
            EnrolledAt = DateTime.UtcNow
        };

        nextEnrollmentId++;
        enrollments.Add(enrollment);

        await notificationService.SendEnrollmentNotificationAsync(
            request.StudentId,
            request.CourseId);

        return enrollment;
    }

    public List<Enrollment> GetAllEnrollments()
    {
        return enrollments;
    }
}
```

### `Services/INotificationService.cs`
```csharp
namespace TrainingEnrollmentSolution.Services
{
    public interface INotificationService
    {
        Task SendEnrollmentNotificationAsync(int studentId, int courseId);
    }
}
```

### `Services/NotificationService.cs`
```csharp
using TrainingEnrollmentSolution.Services;

public class NotificationService : INotificationService
{
    public Task SendEnrollmentNotificationAsync(int studentId, int courseId)
    {
        Console.WriteLine($"Notification sent for Student {studentId}, Course {courseId}");
        return Task.CompletedTask;
    }
}
```

### `Services/ICourseCompletionService.cs`
*(Added during incremental evolution)*
```csharp
using TrainingEnrollmentSolution.Models;

namespace TrainingEnrollmentSolution.Services
{
    public interface ICourseCompletionService
    {
        Task<Course> CompleteCourseAsync(int studentId, int courseId);
    }
}
```

### `Services/CourseCompletionService.cs`
*(Added during incremental evolution)*
```csharp
using TrainingEnrollmentSolution.Models;

namespace TrainingEnrollmentSolution.Services
{
    public class CourseCompletionService : ICourseCompletionService
    {
        private readonly ICourseService courseService;
        private readonly INotificationService notificationService;

        public CourseCompletionService(
            ICourseService courseService,
            INotificationService notificationService)
        {
            this.courseService = courseService;
            this.notificationService = notificationService;
        }

        public async Task<Course> CompleteCourseAsync(int studentId, int courseId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student id.");
            }

            if (courseId <= 0)
            {
                throw new ArgumentException("Invalid course id.");
            }

            Course? course = courseService.GetCourseById(courseId);

            if (course == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            await notificationService.SendEnrollmentNotificationAsync(studentId, courseId);

            return course;
        }
    }
}
```
