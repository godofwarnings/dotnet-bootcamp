---
tags: [architecture, models, poco]
aliases: [Models, Model]
---
# Models

In software architecture (especially when dealing with data access), a **Model** is typically a class that contains only data properties and no business logic or methods. 

> *"A class without stuff, it has only properties. It is called a model." — Instructor Abdullah Khan*

## Purpose
Models (sometimes called POCOs in C# — Plain Old CLR Objects) are used to map database tables to C# objects.

## Example
See the `Patient`, `Doctor`, and `Appointment` models in [Patient.cs](../../Code%20Examples/Day%203%20-%20Entity%20Framework%20Core/Patient.cs), [Doctor.cs](../../Code%20Examples/Day%203%20-%20Entity%20Framework%20Core/Doctor.cs), and [Appointment.cs](../../Code%20Examples/Day%203%20-%20Entity%20Framework%20Core/Appointment.cs), or the `Student` model in [Student.cs](../../Code%20Examples/Day%204%20-%20Layered%20Architecture/Student.cs).

```csharp
public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;
}
```
