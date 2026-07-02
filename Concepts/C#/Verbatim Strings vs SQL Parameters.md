---
tags: [csharp, sql, syntax, dapper]
aliases: [SQL Parameters, Verbatim Strings, @ symbol]
---
# Verbatim Strings vs SQL Parameters

When writing database access code in C# using Dapper (or ADO.NET), the `@` symbol is used in two very different ways. It's crucial to understand the distinction.

## 1. C# Verbatim String Literal (`@"..."`)
When placed directly before the opening quote of a string, `@` tells C# to treat the string as verbatim.

```csharp
string query = @"INSERT INTO Student (FirstName, LastName)
                 VALUES (@FirstName, @LastName)";
```
**Why use it?**
- You can write SQL on **multiple lines** cleanly.
- Backslashes don't need escaping.
- It is **purely C# syntax**, not SQL.

## 2. SQL Parameter Placeholder (`@Id`)
When used inside the SQL query string itself, `@` denotes a parameter variable that will be supplied safely by the code.

```csharp
string query = "SELECT * FROM Student WHERE Id = @Id";
Student? student = connection.QueryFirstOrDefault(query, new { Id = id });
```
**Why use it?**
- Dapper maps the `@Id` in the SQL to the `Id = id` in the anonymous object passed.
- It prevents SQL Injection.
- It avoids messy string concatenation.

> [!TIP]
> - `@"..."` is for **writing the query text nicely** in C#.
> - `@Id` is for **sending values into the query safely** in SQL.
