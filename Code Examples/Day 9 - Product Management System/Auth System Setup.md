## Question

How does authorization actually work? I didn't quite get it.

## Answer

Here is an end-to-end example showing role-based authentication and authorization using JWT Tokens, `IAuthService`, and an `AppUser` model.

### 1) `Models/AppUser.cs`

```csharp
namespace WebApplication15.Models
{
    public class AppUser
    {
        public int UserId { get; set; }

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Role { get; set; } = "";
    }
}
```

### 2) `DTOs/LoginRequest.cs`

```csharp
namespace WebApplication15.DTOs
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
```

### 3) `Services/IAuthService.cs`

```csharp
using Microsoft.AspNetCore.Identity.Data;

namespace WebApplication15.Services
{
    public interface IAuthService
    {
        string Login(WebApplication15.DTOs.LoginRequest request); 
    }
}
```

### 4) `Services/AuthService.cs`

```csharp
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication15.Services;
using WebApplication15.Models;
using WebApplication15.DTOs;

public class AuthService : IAuthService
{
    private readonly IConfiguration configuration;

    private static readonly List<AppUser> users =
    [
        new AppUser
        {
            UserId = 1,
            Username = "admin",
            Password = "admin123",
            FullName = "Admin User",
            Role = "Admin"
        },
        new AppUser
        {
            UserId = 2,
            Username = "trainer",
            Password = "trainer123",
            FullName = "Trainer User",
            Role = "Trainer"
        },
        new AppUser
        {
            UserId = 3,
            Username = "student",
            Password = "student123",
            FullName = "Student User",
            Role = "Student"
        }
    ];

    public AuthService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string? Login(LoginRequest request)
    {
        AppUser? user = users.FirstOrDefault(u =>
            u.Username == request.Username &&
            u.Password == request.Password);

        if (user == null)
        {
            return null;
        }

        string token = GenerateJwtToken(user);

        return token;
    }

    private string GenerateJwtToken(AppUser user)
    {
        string secretKey = configuration["JwtSettings:SecretKey"]!;
        string issuer = configuration["JwtSettings:Issuer"]!;
        string audience = configuration["JwtSettings:Audience"]!;
        int expiryInMinutes = configuration.GetValue<int>("JwtSettings:ExpiryInMinutes");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("FullName", user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiryInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

### 5) `Controllers/AuthController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using WebApplication15.DTOs;
using WebApplication15.Services;

namespace WebApplication15.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            string? token = authService.Login(request);

            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new
            {
                Token = token
            });
        }
    }
}
```

### 6) `Controllers/AdminController.cs`

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        public IActionResult GetAdminDashboard()
        {
            return Ok(new
            {
                Message = "Welcome Admin. You can access the admin dashboard."
            });
        }
    }
}
```

### 7) `Controllers/TrainerController.cs`

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Trainer")]
    public class TrainerController : ControllerBase
    {
        [HttpGet("sessions")]
        public IActionResult GetTrainerSessions()
        {
            return Ok(new
            {
                Message = "Welcome Trainer. These are your training sessions."
            });
        }
    }
}
```

### 8) `Controllers/SecureController.cs`

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            string username = User.Identity?.Name ?? "";
            string fullName = User.FindFirst("FullName")?.Value ?? "";
            string role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            return Ok(new
            {
                Username = username,
                FullName = fullName,
                Role = role,
                Message = "You are authenticated."
            });
        }
    }
}
```

### 9) `Controllers/PublicController.cs`

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
                Message = "This is public data. Anyone can access this."
            });
        }
    }
}
```
