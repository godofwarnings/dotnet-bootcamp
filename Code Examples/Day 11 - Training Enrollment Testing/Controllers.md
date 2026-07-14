# Controllers (Day 11)

This file contains the ASP.NET Core API controllers handling the Training Enrollment system, including the `GetCourseById` endpoint which was added incrementally to fix the integration tests.

### `Controllers/CoursesController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using TrainingEnrollmentSolution.Services;

namespace TrainingEnrollmentSolution.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courses = courseService.GetAllCourses();
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCourseById(int id)
        {
            var course = courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound("Course not found.");
            }

            return Ok(course);
        }
    }
}
```

### `Controllers/EnrollmentsController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using TrainingEnrollmentSolution.DTOs;
using TrainingEnrollmentSolution.Services;

namespace TrainingEnrollmentSolution.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            this.enrollmentService = enrollmentService;
        }

        [HttpGet]
        public IActionResult GetAllEnrollments()
        {
            var enrollments = enrollmentService.GetAllEnrollments();
            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollment(
            [FromBody] CreateEnrollmentRequest request)
        {
            try
            {
                var enrollment = await enrollmentService.CreateEnrollmentAsync(request);
                return Created("/api/enrollments/" + enrollment.EnrollmentId, enrollment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
```
