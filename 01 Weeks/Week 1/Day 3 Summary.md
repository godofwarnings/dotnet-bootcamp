---
tags: [summary, week1, day3]
---
# Day 3 Summary

**Date:** 1st July 2026
**Instructor:** Abdullah Khan

## Topics Covered
- [Design Patterns](../../Concepts/Architecture/Design%20Patterns.md) (Singleton, Repository, Publisher-Subscriber)
- [Models](../../Concepts/Architecture/Models.md) (POCOs, Data transfer objects)
- [Dapper Basics](../../Concepts/Database/Dapper%20Basics.md) (Micro-ORM, SQL mapping, Repository implementation)
- [Entity Framework Core](../../Concepts/Database/Entity%20Framework%20Core.md) (Full ORM, DbContext, DbSet)
- [SQL Server and Visual Studio](../../Concepts/Database/SQL%20Server%20and%20Visual%20Studio.md) (localdb, shortcuts)
- [Delegates and Action](../../Concepts/C%23/Delegates%20and%20Action.md) (Delegates, Action type)
- [Verbatim Strings vs SQL Parameters](../../Concepts/C%23/Verbatim%20Strings%20vs%20SQL%20Parameters.md) (`@` prefix vs `@Id` parameter)

## Key Takeaways
- The **Repository Pattern** abstracts database logic away from business logic, preventing hundreds of classes from directly hitting the database.
- **Dapper** is a high-performance Micro-ORM used to seamlessly map raw SQL query results into C# objects.
- **Entity Framework Core** is a full ORM that completely abstracts SQL generation behind C# code and LINQ.
- In C#, using `@` before a string creates a verbatim string literal (helpful for multi-line SQL), whereas `@Id` inside a string defines a SQL parameter (helpful for preventing SQL injection).

## Files Created
- `Concepts/Architecture/Design Patterns.md`
- `Concepts/Architecture/Models.md`
- `Concepts/Database/Dapper Basics.md`
- `Concepts/Database/Entity Framework Core.md`
- `Concepts/Database/SQL Server and Visual Studio.md`
- `Concepts/C#/Delegates and Action.md`
- `Concepts/C#/Verbatim Strings vs SQL Parameters.md`
- `Code Examples/Student.cs`
- `Code Examples/StudentRepository.cs`
- `Code Examples/Student_Program.cs`
- `Code Examples/Student_Dapper_Practice.cs`
- `Code Examples/Patient.cs`
- `Code Examples/Doctor.cs`
- `Code Examples/Appointment.cs`
- `Code Examples/HospitalService.cs`
- `Code Examples/Hospital_Program.cs`
- `Code Examples/RetailDb_Scripts.sql`

## Knowledge Gaps / Backlog
- What is the equivalent of the `Action` delegate in other languages (e.g., Python, JS)?
- ADO.NET vs Dapper vs EF Core: When exactly should you use each one in a real-world enterprise project?
- How do `?` and `!` exactly work in variable and function declarations (Nullables and Null-Forgiving)?
- Namespaces vs `using`: How does encapsulation across files work in C#?
- What are all the system databases listed in SQL Server Explorer (master, model, msdb, tempdb)?

## Preview of Next Week / Tomorrow
- Continued practice and deep dive into Entity Framework Core and advanced Data Access layers.
