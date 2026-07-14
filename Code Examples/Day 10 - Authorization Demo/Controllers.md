# Controllers (Day 10)

This file contains the ASP.NET Core API controllers handling authentication and role-based endpoints for the Authorization Demo application.

### `Controllers/AuthController.cs`
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication15.DTOs;
using WebApplication15.Services;

namespace WebApplication15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var response = authService.Login(request);

            if (response == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            return Ok(response);
        }
    }
}
```

### `Controllers/AdminController.cs`
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication15.DTOs;
using WebApplication15.Services;

namespace WebApplication15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly DemoDataStore dataStore;

        public AdminController(DemoDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            return Ok(new
            {
                message = $"Welcome {User.Identity?.Name}. You can access the admin dashboard.",
                cohortCount = dataStore.Cohorts.Count,
                trainerCount = dataStore.Trainers.Count
            });
        }

        [HttpGet("cohorts")]
        public IActionResult GetCohorts()
        {
            return Ok(dataStore.GetCohorts());
        }

        [HttpPost("cohorts")]
        public IActionResult AddCohort([FromBody] CreateCohortRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var cohort = dataStore.AddCohort(request);
            return Ok(cohort);
        }

        [HttpDelete("cohorts/{id:int}")]
        public IActionResult DeleteCohort(int id)
        {
            var deleted = dataStore.RemoveCohort(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Cohort not found."
                });
            }

            return NoContent();
        }

        [HttpGet("trainers")]
        public IActionResult GetTrainers()
        {
            return Ok(dataStore.GetTrainers());
        }

        [HttpPost("trainers")]
        public IActionResult AddTrainer([FromBody] CreateTrainerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var trainer = dataStore.AddTrainer(request);
                return Ok(trainer);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("trainers/{id:int}")]
        public IActionResult DeleteTrainer(int id)
        {
            var deleted = dataStore.RemoveTrainer(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Trainer not found."
                });
            }

            return NoContent();
        }
    }
}
```

### `Controllers/TrainerController.cs`
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication15.Services;

namespace WebApplication15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Trainer")]
    public class TrainerController : ControllerBase
    {
        private readonly DemoDataStore dataStore;

        public TrainerController(DemoDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            var username = User.Identity?.Name ?? string.Empty;

            return Ok(new
            {
                message = $"Welcome {username}. You can access the trainer dashboard.",
                sessionCount = dataStore.GetUpcomingSessions(username).Count(),
                attendanceCount = dataStore.GetAttendance(username).Count()
            });
        }

        [HttpGet("sessions")]
        public IActionResult GetTrainerSessions()
        {
            var username = User.Identity?.Name ?? string.Empty;
            return Ok(dataStore.GetUpcomingSessions(username));
        }

        [HttpGet("attendance")]
        public IActionResult GetAttendance()
        {
            var username = User.Identity?.Name ?? string.Empty;
            return Ok(dataStore.GetAttendance(username));
        }
    }
}
```

### `Controllers/SecureController.cs`
```csharp
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SecureController : ControllerBase
    {
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var username =
                User.Identity?.Name ??
                User.FindFirst(ClaimTypes.Name)?.Value ??
                User.FindFirst("username")?.Value ??
                string.Empty;

            var fullName =
                User.FindFirst("fullName")?.Value ??
                User.FindFirst("FullName")?.Value ??
                string.Empty;

            var role =
                User.FindFirst(ClaimTypes.Role)?.Value ??
                User.FindFirst("role")?.Value ??
                string.Empty;

            return Ok(new
            {
                username,
                fullName,
                role,
                message = "You are authenticated."
            });
        }
    }
}
```

### `Controllers/PublicController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPublicData()
        {
            return Ok(new
            {
                message = "This is public data. Anyone can access this."
            });
        }
    }
}
```
