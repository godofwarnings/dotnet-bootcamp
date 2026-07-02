---
tags: [database, dapper, orm, micro-orm, csharp]
aliases: [Dapper, Micro ORM]
---
# Dapper Basics

**Dapper** is a Micro-ORM (Object Relational Mapper) for .NET. It extends the `IDbConnection` interface, meaning it works directly with raw `SqlConnection` objects, but automates the boring process of mapping rows to C# objects.

Unlike full ORMs like [Entity Framework Core](Entity%20Framework%20Core.md), Dapper does not generate SQL for you. You write raw SQL, and Dapper maps the results. Because of this, it is incredibly fast.

## Basic Implementation
When writing Dapper code, we usually implement the [Repository Pattern](../Architecture/Design%20Patterns.md). 

See [StudentRepository.cs](../../Code%20Examples/Day%203%20-%20Dapper/StudentRepository.cs) for a full working example of CRUD operations using Dapper.
If you want to test your knowledge, check out [Student_Dapper_Practice.cs](../../Code%20Examples/Day%203%20-%20Dapper/Student_Dapper_Practice.cs).

## Refactoring Tips for Dapper Repositories

### 1. Connection Helper
Instead of repeatedly initializing connections:
```csharp
private SqlConnection CreateConnection()
{
    return new SqlConnection(connectionString);
}
```

### 2. Extract SQL Queries to Constants
Instead of mixing SQL strings in the method logic, declare them as constants at the class level:
```csharp
private const string SelectAllQuery = "SELECT Id, FirstName, LastName, Age, Email, Course FROM Student";
```

### 3. Separation of Concerns
Keep `Main` small. Move display logic (like printing a student object) out of `Main` and into a helper method like `private static void PrintStudent(Student student)`.

---
## See Also
- [Verbatim Strings vs SQL Parameters](../C%23/Verbatim%20Strings%20vs%20SQL%20Parameters.md)
- [Entity Framework Core](Entity%20Framework%20Core.md)
- [ADO.NET Basics](ADO.NET%20Basics.md)
