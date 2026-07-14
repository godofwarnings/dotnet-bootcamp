# Models (Day 10)

This file contains the internal domain models used in the Authorization Demo application.

### `Models/AppUser.cs`
```csharp
namespace WebApplication15.Models
{
    public class AppUser
    {
        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        // Store a hash instead of the raw password.
        public string PasswordHash { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
```

### `Models/Cohort.cs`
```csharp
namespace WebApplication15.Models
{
    public class Cohort
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
    }
}
```

### `Models/TrainerInfo.cs`
```csharp
namespace WebApplication15.Models
{
    public class TrainerInfo
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Specialty { get; set; } = string.Empty;
    }
}
```

### `Models/TrainingSession.cs`
```csharp
namespace WebApplication15.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string CohortName { get; set; } = string.Empty;

        public DateTime SessionDate { get; set; }

        public string TrainerUsername { get; set; } = string.Empty;
    }
}
```

### `Models/AttendanceRecord.cs`
```csharp
namespace WebApplication15.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }

        public string CohortName { get; set; } = string.Empty;

        public DateTime SessionDate { get; set; }

        public int PresentCount { get; set; }

        public int TotalCount { get; set; }

        public string TrainerUsername { get; set; } = string.Empty;
    }
}
```
