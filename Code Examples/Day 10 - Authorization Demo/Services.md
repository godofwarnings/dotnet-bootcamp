# Services (Day 10)

This file contains the authentication service and the in-memory data store for the Authorization Demo application.

### `Services/IAuthService.cs`
```csharp
using WebApplication15.DTOs;

namespace WebApplication15.Services
{
    public interface IAuthService
    {
        LoginResponse? Login(LoginRequest request);
    }
}
```

### `Services/DemoDataStore.cs`
```csharp
using Microsoft.AspNetCore.Identity;
using WebApplication15.DTOs;
using WebApplication15.Models;

namespace WebApplication15.Services
{
    public class DemoDataStore
    {
        private readonly PasswordHasher passwordHasher = new();

        public List<AppUser> Users { get; } = new();
        public List<Cohort> Cohorts { get; } = new();
        public List<TrainerInfo> Trainers { get; } = new();
        public List<TrainingSession> Sessions { get; } = new();
        public List<AttendanceRecord> AttendanceRecords { get; } = new();

        public DemoDataStore()
        {
            SeedUsers();
            SeedBusinessData();
        }

        private void SeedUsers()
        {
            var admin = new AppUser
            {
                UserId = 1,
                Username = "admin",
                FullName = "Admin User",
                Role = "Admin"
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "admin123");

            var trainer = new AppUser
            {
                UserId = 2,
                Username = "trainer",
                FullName = "Trainer User",
                Role = "Trainer"
            };
            trainer.PasswordHash = passwordHasher.HashPassword(trainer, "trainer123");

            var student = new AppUser
            {
                UserId = 3,
                Username = "student",
                FullName = "Student User",
                Role = "Student"
            };
            student.PasswordHash = passwordHasher.HashPassword(student, "student123");

            Users.Add(admin);
            Users.Add(trainer);
            Users.Add(student);
        }

        private void SeedBusinessData()
        {
            Trainers.Add(new TrainerInfo
            {
                Id = 1,
                Username = "trainer",
                FullName = "Trainer User",
                Specialty = "ASP.NET Core"
            });

            Cohorts.Add(new Cohort
            {
                Id = 1,
                Name = "Cohort A",
                StartDate = DateTime.UtcNow.Date.AddDays(7)
            });

            Cohorts.Add(new Cohort
            {
                Id = 2,
                Name = "Cohort B",
                StartDate = DateTime.UtcNow.Date.AddDays(14)
            });

            Sessions.Add(new TrainingSession
            {
                Id = 1,
                Title = "C# Basics",
                CohortName = "Cohort A",
                SessionDate = DateTime.UtcNow.Date.AddDays(1).AddHours(10),
                TrainerUsername = "trainer"
            });

            Sessions.Add(new TrainingSession
            {
                Id = 2,
                Title = "ASP.NET Core Web API",
                CohortName = "Cohort B",
                SessionDate = DateTime.UtcNow.Date.AddDays(3).AddHours(14),
                TrainerUsername = "trainer"
            });

            AttendanceRecords.Add(new AttendanceRecord
            {
                Id = 1,
                CohortName = "Cohort A",
                SessionDate = DateTime.UtcNow.Date.AddDays(-2).AddHours(10),
                PresentCount = 18,
                TotalCount = 20,
                TrainerUsername = "trainer"
            });

            AttendanceRecords.Add(new AttendanceRecord
            {
                Id = 2,
                CohortName = "Cohort B",
                SessionDate = DateTime.UtcNow.Date.AddDays(-1).AddHours(14),
                PresentCount = 22,
                TotalCount = 25,
                TrainerUsername = "trainer"
            });
        }

        public AppUser? FindUser(string username)
        {
            return Users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Cohort> GetCohorts()
        {
            return Cohorts.OrderBy(c => c.StartDate);
        }

        public Cohort AddCohort(CreateCohortRequest request)
        {
            var nextId = Cohorts.Count == 0 ? 1 : Cohorts.Max(c => c.Id) + 1;

            var cohort = new Cohort
            {
                Id = nextId,
                Name = request.Name,
                StartDate = request.StartDate
            };

            Cohorts.Add(cohort);
            return cohort;
        }

        public bool RemoveCohort(int id)
        {
            var cohort = Cohorts.FirstOrDefault(c => c.Id == id);
            if (cohort == null)
            {
                return false;
            }

            Cohorts.Remove(cohort);

            Sessions.RemoveAll(s =>
                s.CohortName.Equals(cohort.Name, StringComparison.OrdinalIgnoreCase));

            AttendanceRecords.RemoveAll(a =>
                a.CohortName.Equals(cohort.Name, StringComparison.OrdinalIgnoreCase));

            return true;
        }

        public IEnumerable<TrainerInfo> GetTrainers()
        {
            return Trainers.OrderBy(t => t.FullName);
        }

        public TrainerInfo AddTrainer(CreateTrainerRequest request)
        {
            if (Users.Any(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A user with this username already exists.");
            }

            var nextUserId = Users.Count == 0 ? 1 : Users.Max(u => u.UserId) + 1;
            var nextTrainerId = Trainers.Count == 0 ? 1 : Trainers.Max(t => t.Id) + 1;

            var user = new AppUser
            {
                UserId = nextUserId,
                Username = request.Username,
                FullName = request.FullName,
                Role = "Trainer"
            };

            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

            var trainer = new TrainerInfo
            {
                Id = nextTrainerId,
                Username = request.Username,
                FullName = request.FullName,
                Specialty = request.Specialty
            };

            Users.Add(user);
            Trainers.Add(trainer);

            return trainer;
        }

        public bool RemoveTrainer(int id)
        {
            var trainer = Trainers.FirstOrDefault(t => t.Id == id);
            if (trainer == null)
            {
                return false;
            }

            Trainers.Remove(trainer);

            var user = Users.FirstOrDefault(u =>
                u.Username.Equals(trainer.Username, StringComparison.OrdinalIgnoreCase)
                && u.Role == "Trainer");

            if (user != null)
            {
                Users.Remove(user);
            }

            Sessions.RemoveAll(s =>
                s.TrainerUsername.Equals(trainer.Username, StringComparison.OrdinalIgnoreCase));

            AttendanceRecords.RemoveAll(a =>
                a.TrainerUsername.Equals(trainer.Username, StringComparison.OrdinalIgnoreCase));

            return true;
        }

        public IEnumerable<TrainingSession> GetUpcomingSessions(string trainerUsername)
        {
            return Sessions
                .Where(s => s.TrainerUsername.Equals(trainerUsername, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.SessionDate);
        }

        public IEnumerable<AttendanceRecord> GetAttendance(string trainerUsername)
        {
            return AttendanceRecords
                .Where(a => a.TrainerUsername.Equals(trainerUsername, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.SessionDate);
        }
    }
}
```

### `Services/AuthService.cs`
```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApplication15.DTOs;
using WebApplication15.Models;

namespace WebApplication15.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly DemoDataStore dataStore;
        private readonly PasswordHasher<AppUser> passwordHasher = new();

        public AuthService(IConfiguration configuration, DemoDataStore dataStore)
        {
            this.configuration = configuration;
            this.dataStore = dataStore;
        }

        public LoginResponse? Login(LoginRequest request)
        {
            var user = dataStore.FindUser(request.Username);

            if (user == null)
            {
                return null;
            }

            var verifyResult = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        private string GenerateJwtToken(AppUser user)
        {
            var secretKey = configuration["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey is missing.");

            var issuer = configuration["JwtSettings:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is missing.");

            var audience = configuration["JwtSettings:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is missing.");

            var expiryInMinutes = configuration.GetValue<int>("JwtSettings:ExpiryInMinutes");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Write BOTH forms of the claims so mixed frontend/backend code still works.
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),

                // Username claims
                new(JwtRegisteredClaimNames.UniqueName, user.Username),
                new(ClaimTypes.Name, user.Username),
                new("username", user.Username),

                // Full name claims
                new("fullName", user.FullName),
                new("FullName", user.FullName),

                // Role claims
                new("role", user.Role),
                new(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
```
