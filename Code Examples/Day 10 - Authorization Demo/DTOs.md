# DTOs (Day 10)

This file contains the Data Transfer Objects (DTOs) used to handle requests and responses for the API endpoints in the Authorization Demo application.

### `DTOs/LoginRequest.cs`
```csharp
using System.ComponentModel.DataAnnotations;

namespace WebApplication15.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
```

### `DTOs/LoginResponse.cs`
```csharp
namespace WebApplication15.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
```

### `DTOs/CreateTrainerRequest.cs`
```csharp
using System.ComponentModel.DataAnnotations;

namespace WebApplication15.DTOs
{
    public class CreateTrainerRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string Specialty { get; set; } = string.Empty;
    }
}
```

### `DTOs/CreateCohortRequest.cs`
```csharp
using System.ComponentModel.DataAnnotations;

namespace WebApplication15.DTOs
{
    public class CreateCohortRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
    }
}
```
