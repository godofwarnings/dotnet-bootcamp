# Models and DTOs (Day 11)

This file contains the internal domain models and Data Transfer Objects for the Training Enrollment application.

### `Models/Course.cs`
```csharp
namespace TrainingEnrollmentSolution.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public decimal Fee { get; set; }

        public int DurationInDays { get; set; }
    }
}
```

### `Models/Enrollment.cs`
```csharp
namespace TrainingEnrollmentSolution.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime EnrolledAt { get; set; }
    }
}
```

### `DTOs/CreateEnrollmentRequest.cs`
```csharp
namespace TrainingEnrollmentSolution.DTOs
{
    public class CreateEnrollmentRequest
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
    }
}
```
