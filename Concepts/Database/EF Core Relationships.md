# EF Core Relationships

Entity Framework Core maps object-oriented relationships between classes into relational database structures (Foreign Keys and Join Tables).

## One-to-Many Relationships (1:N)

The most common relationship is One-to-Many. For example, one `Grade` can have many `Student`s, but a `Student` has only one `Grade`.

```csharp
public class Grade // Principal Entity (The "1" side)
{
    public int GradeId { get; set; }
    public string GradeName { get; set; }

    // Navigation property: One Grade has many Students
    public ICollection<Student> Students { get; set; }
}

public class Student // Dependent Entity (The "Many" side)
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    
    // Foreign Key mapping back to Grade
    public Grade Grade { get; set; }
}
```

- **Navigation Properties**: By declaring `ICollection<Student>`, EF Core understands that a `Grade` will track multiple students.
- **Under the hood**: EF Core will automatically generate a Foreign Key column in the `Students` table pointing to the `GradeId` in the `Grades` table during migration.
