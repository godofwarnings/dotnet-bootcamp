# Day 6 Summary

**Date:** 6th July 2026
**Instructor:** Instructor

## Topics Covered
- [EF Core Migrations](../../Concepts/Database/EF%20Core%20Migrations.md)
- [EF Core Data Annotations](../../Concepts/Database/EF%20Core%20Data%20Annotations.md)
- [EF Core Relationships](../../Concepts/Database/EF%20Core%20Relationships.md)
- Visual Studio Scaffolding Quirks (in [ASP.NET Core MVC](../../Concepts/Web%20Development/ASP.NET%20Core%20MVC.md))

## Key Takeaways
- **EF Core Migrations** form a continuous history of schema changes. `Add-Migration` generates the C# code instructions (`Up` and `Down`), while `Update-Database` executes those instructions against the SQL Server.
- A **`DbContext`** manages the database session, and each **`DbSet`** represents a specific table (e.g., `DbSet<Student> Students`).
- **Data Annotations** (like `[Key]`, `[Required]`, `[DataType]`) instruct EF Core on how to shape the database schema and instruct MVC Scaffolding on how to build the UI (e.g. generating date pickers).
- **Entity Relationships** are achieved via Navigation Properties. A 1:Many relationship uses `ICollection` on the principal side and a direct object reference on the dependent side, which EF Core automatically translates to a Foreign Key constraint.
- **Visual Studio Quirks**: When scaffolding an MVC Controller from a model (e.g., `Customer.cs`), Visual Studio automatically pluralizes the Controller and Views (e.g., `CustomersController`). If you try to manually rename it back to singular without renaming all routes and view folders, you will encounter 404 Not Found errors.
