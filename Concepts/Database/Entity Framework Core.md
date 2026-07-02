---
tags: [database, orm, entity-framework, ef-core]
aliases: [Entity Framework Core, EF Core, ORM]
---
# Entity Framework Core

Entity Framework (EF) Core is a full ORM (Object Relational Mapper). Unlike Micro-ORMs like [Dapper](Dapper%20Basics.md), EF Core abstracts away the SQL entirely in most cases. You write C# code (using LINQ), and EF Core translates it into SQL behind the scenes.

## Core Concepts
- **DbContext:** The heart of EF Core. It represents a session with the database and is used to query and save instances of your entities.
- **DbSet<T>:** Represents a collection of all entities in the context that can be queried from the database.

## Example Implementation
See [HospitalService.cs](../../Code%20Examples/Day%203%20-%20Entity%20Framework%20Core/HospitalService.cs) for a basic implementation outlining how models are queried using a `DbContext`.

For example, finding all available doctors using EF Core and LINQ:
```csharp
using HospitalDbContext context = new HospitalDbContext();

// Entity Framework translates this LINQ to a SQL SELECT query automatically.
List<Doctor> availableDoctors = context.Doctors
    .Where(doc => doc.IsAvailable == true)
    .ToList();
```

---
## See Also
- [Dapper Basics](Dapper%20Basics.md)
- [Models](../Architecture/Models.md)
