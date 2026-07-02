---
tags: [csharp, oop, class, object, dry]
aliases: [Classes, Objects]
---
# Classes and Objects

## What is a Class?
A **class** is a **blueprint** or **idea**. We look at physical entities and virtualize them so we can start building the product, application, or system.

It defines:
- **Attributes** (Fields / Data)
- **Properties** (Controlled access to data)
- **Behaviors** (Methods)

> [!TIP]
> **Instructor Advice:** "We look at physical entities and virtualize them so we can start building the product."

## The DRY Principle
**DRY** stands for **Don't Repeat Yourself**. Classes and methods help us encapsulate logic so we don't write the same code multiple times.

## Code Example: Product Class
See [Basic Product Class.cs](../../Code%20Examples/Day%201%20-%20OOP%20Basics/Basic%20Product%20Class.cs) for a concrete example of a class with private fields, a constructor, and methods.

### Key Concepts from Example
- **Constructor:** Runs when a new object is created. It assigns initial values.
- **`this` Keyword:** Refers to the current instance of the class. It helps distinguish between class fields and constructor parameters with the same name.
- **Private Fields:** Data hiding. They cannot be changed directly from outside the class.

## The Entry Point
The `Main` method is the entry point of a C# console application.

---
## See Also
- [Properties and Backing Fields](Properties%20and%20Backing%20Fields.md)
- [NET Bootcamp Home](../../00%20Home/NET%20Bootcamp%20Home.md)
