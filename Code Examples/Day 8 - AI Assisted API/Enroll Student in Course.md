## Question

Add **Enroll Student in Course** to my codebase and give me the code.

## Answer

Below is a clean **model + DTO + service + controller** version that fits your current in-memory project.

Keep your existing `Student` model and `StudentService` as they are.

---

## 1) Model: `Course.cs`

```csharp
namespace WebApplication13.Models
{
    public sealed class Course
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
```

---

## 2) Model: `Enrollment.cs`

```csharp
namespace WebApplication13.Models
{
    public sealed class Enrollment
    {
        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public DateTime EnrolledOnUtc { get; set; }

        public string Status { get; set; } = "Enrolled";
    }
}
```

---

## 3) DTO: `EnrollStudentInCourseRequestDto.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace WebApplication13.Dtos.Enrollments
{
    public sealed class EnrollStudentInCourseRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "StudentId must be greater than 0.")]
        public int StudentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be greater than 0.")]
        public int CourseId { get; set; }
    }
}
```

---

## 4) DTO: `EnrollmentResponseDto.cs`

```csharp
namespace WebApplication13.Dtos.Enrollments
{
    public sealed class EnrollmentResponseDto
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public int CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime EnrolledOnUtc { get; set; }
    }
}
```

---

## 5) Service Interface: `ICourseService.cs`

```csharp
using WebApplication13.Models;

namespace WebApplication13.Services
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAllCourses();

        Course? GetCourseById(int id);
    }
}
```

---

## 6) Service: `CourseService.cs`

```csharp
using WebApplication13.Models;

namespace WebApplication13.Services
{
    public sealed class CourseService : ICourseService
    {
        private readonly List<Course> _courses = new()
        {
            new Course { Id = 1, Name = "ASP.NET Core" },
            new Course { Id = 2, Name = "C#" },
            new Course { Id = 3, Name = "SQL Server" }
        };

        public IEnumerable<Course> GetAllCourses()
        {
            return _courses;
        }

        public Course? GetCourseById(int id)
        {
            return _courses.FirstOrDefault(c => c.Id == id);
        }
    }
}
```

---

## 7) Service Interface: `IEnrollmentService.cs`

```csharp
using WebApplication13.Dtos.Enrollments;

namespace WebApplication13.Services
{
    public interface IEnrollmentService
    {
        EnrollmentResponseDto EnrollStudentInCourse(EnrollStudentInCourseRequestDto request);

        EnrollmentResponseDto? GetEnrollment(int studentId, int courseId);

        IEnumerable<EnrollmentResponseDto> GetAllEnrollments();
    }
}
```

---

## 8) Service: `EnrollmentService.cs`

```csharp
using WebApplication13.Dtos.Enrollments;
using WebApplication13.Models;

namespace WebApplication13.Services
{
    public sealed class EnrollmentService : IEnrollmentService
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        private readonly object _syncLock = new();
        private readonly List<Enrollment> _enrollments = new();

        public EnrollmentService(
            IStudentService studentService,
            ICourseService courseService)
        {
            _studentService = studentService;
            _courseService = courseService;
        }

        public EnrollmentResponseDto EnrollStudentInCourse(EnrollStudentInCourseRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.StudentId <= 0)
            {
                throw new ArgumentException("StudentId must be greater than 0.");
            }

            if (request.CourseId <= 0)
            {
                throw new ArgumentException("CourseId must be greater than 0.");
            }

            var student = _studentService.GetStudentById(request.StudentId);

            if (student is null)
            {
                throw new KeyNotFoundException($"Student with id {request.StudentId} was not found.");
            }

            var course = _courseService.GetCourseById(request.CourseId);

            if (course is null)
            {
                throw new KeyNotFoundException($"Course with id {request.CourseId} was not found.");
            }

            lock (_syncLock)
            {
                var isAlreadyEnrolled = _enrollments.Any(e =>
                    e.StudentId == request.StudentId &&
                    e.CourseId == request.CourseId);

                if (isAlreadyEnrolled)
                {
                    throw new InvalidOperationException("Student is already enrolled in this course.");
                }

                var enrollment = new Enrollment
                {
                    StudentId = request.StudentId,
                    CourseId = request.CourseId,
                    EnrolledOnUtc = DateTime.UtcNow,
                    Status = "Enrolled"
                };

                _enrollments.Add(enrollment);

                return MapToResponseDto(enrollment, student, course);
            }
        }

        public EnrollmentResponseDto? GetEnrollment(int studentId, int courseId)
        {
            lock (_syncLock)
            {
                var enrollment = _enrollments.FirstOrDefault(e =>
                    e.StudentId == studentId &&
                    e.CourseId == courseId);

                if (enrollment is null)
                {
                    return null;
                }

                var student = _studentService.GetStudentById(enrollment.StudentId);
                var course = _courseService.GetCourseById(enrollment.CourseId);

                if (student is null || course is null)
                {
                    return null;
                }

                return MapToResponseDto(enrollment, student, course);
            }
        }

        public IEnumerable<EnrollmentResponseDto> GetAllEnrollments()
        {
            lock (_syncLock)
            {
                return _enrollments
                    .Select(e =>
                    {
                        var student = _studentService.GetStudentById(e.StudentId);
                        var course = _courseService.GetCourseById(e.CourseId);

                        if (student is null || course is null)
                        {
                            return null;
                        }

                        return MapToResponseDto(e, student, course);
                    })
                    .Where(e => e is not null)
                    .Cast<EnrollmentResponseDto>()
                    .ToList();
            }
        }

        private static EnrollmentResponseDto MapToResponseDto(
            Enrollment enrollment,
            Student student,
            Course course)
        {
            return new EnrollmentResponseDto
            {
                StudentId = enrollment.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}".Trim(),
                CourseId = enrollment.CourseId,
                CourseName = course.Name,
                Status = enrollment.Status,
                EnrolledOnUtc = enrollment.EnrolledOnUtc
            };
        }
    }
}
```

---

## 9) Controller: `CoursesController.cs`

This helps you test available courses.

```csharp
using Microsoft.AspNetCore.Mvc;
using WebApplication13.Models;
using WebApplication13.Services;

namespace WebApplication13.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {
            var courses = _courseService.GetAllCourses();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Course> GetCourseById(int id)
        {
            var course = _courseService.GetCourseById(id);

            if (course is null)
            {
                return NotFound();
            }

            return Ok(course);
        }
    }
}
```

---

## 10) Controller: `EnrollmentsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using WebApplication13.Dtos.Enrollments;
using WebApplication13.Services;

namespace WebApplication13.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EnrollmentResponseDto>> GetAllEnrollments()
        {
            var enrollments = _enrollmentService.GetAllEnrollments();
            return Ok(enrollments);
        }

        [HttpGet("{studentId:int}/{courseId:int}")]
        public ActionResult<EnrollmentResponseDto> GetEnrollment(int studentId, int courseId)
        {
            var enrollment = _enrollmentService.GetEnrollment(studentId, courseId);

            if (enrollment is null)
            {
                return NotFound();
            }

            return Ok(enrollment);
        }

        [HttpPost]
        public ActionResult<EnrollmentResponseDto> EnrollStudent(
            [FromBody] EnrollStudentInCourseRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var response = _enrollmentService.EnrollStudentInCourse(request);

                return CreatedAtAction(
                    nameof(GetEnrollment),
                    new { studentId = response.StudentId, courseId = response.CourseId },
                    response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
```

---

## 11) `Program.cs` registrations

Add these lines to your existing `Program.cs`.

```csharp
builder.Services.AddSingleton<ICourseService, CourseService>();
builder.Services.AddSingleton<IEnrollmentService, EnrollmentService>();
```

If you want the relevant registration section together, it can look like this:

```csharp
using WebApplication13.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<ICourseService, CourseService>();
builder.Services.AddSingleton<IStudentService, StudentService>();
builder.Services.AddSingleton<IEnrollmentService, EnrollmentService>();

var app = builder.Build();

app.MapControllers();

app.Run();
```

---

## 12) Example request

### `POST /api/enrollments`

```json
{
  "studentId": 1,
  "courseId": 1
}
```

---

## 13) Example success response

```json
{
  "studentId": 1,
  "studentName": "Alice Johnson",
  "courseId": 1,
  "courseName": "ASP.NET Core",
  "status": "Enrolled",
  "enrolledOnUtc": "2026-07-08T12:00:00Z"
}
```

---

## 14) What this gives you

- `GET /api/courses`
- `GET /api/courses/{id}`
- `POST /api/enrollments`
- `GET /api/enrollments`
- `GET /api/enrollments/{studentId}/{courseId}`

---

## 15) Important note

Because this is in-memory:

- duplicate enrollment is prevented while the app is running
- data resets when the app restarts
