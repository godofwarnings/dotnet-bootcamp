# EF Core Migrations

Migrations in Entity Framework Core provide a way to incrementally update the database schema to keep it in sync with the application's data model while preserving existing data in the database.

## Key Commands
- **`Add-Migration <Name>`**: This command generates migration code based on changes in your model classes and `DbContext`. `InitialCreate` is often used as the standard name for the very first migration.
  - Generates files in a `Migrations/` folder.
  - These files contain two important methods:
    - `Up()`: Code to apply the changes (e.g., create tables, add columns).
    - `Down()`: Code to reverse the changes (e.g., drop tables, remove columns).
- **`Update-Database`**: This command reads the generated migration code and executes it against the actual database. If the database does not exist, it will create it.

## DbContext and DbSet
- **`DbContext`**: The core class that manages database connections and is responsible for coordinating Entity Framework functionality for data access.
- **`DbSet<T>`**: Represents a collection of entities in the context that can be queried from the database. Each `DbSet` typically maps to a single table (e.g., `public DbSet<Student> Students { get; set; }` maps to the `Students` table).

## Typical Workflow
1. Modify Model Class (e.g., add a property to `Student.cs`).
2. Run `Add-Migration AddedPhoneNumber`.
3. Run `Update-Database` to push the new column to SQL Server.
