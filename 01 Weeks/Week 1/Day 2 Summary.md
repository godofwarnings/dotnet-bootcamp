---
tags: [summary, week1, day2]
---
# Day 2 Summary

**Date:** 30th June 2026
**Instructor:** Instructor

## Topics Covered
- [Properties and Backing Fields](../../Concepts/C%23/Properties%20and%20Backing%20Fields.md) (Encapsulation, auto-properties vs backing fields)
- [Interfaces and Abstract Classes](../../Concepts/C%23/Interfaces%20and%20Abstract%20Classes.md) (Base classes, virtual methods, abstraction)
- [Static Methods](../../Concepts/C%23/Static%20Methods.md) (Instance vs Class methods)
- [Exception Handling](../../Concepts/C%23/Exception%20Handling.md) (Try, Catch, Finally, Custom Exceptions)
- [Async vs Sync](../../Concepts/Async%20Programming/Async%20vs%20Sync.md) (Introduction to Async/Await)
- [Thread Safe Collections](../../Concepts/Async%20Programming/Thread%20Safe%20Collections.md) (ConcurrentQueue)
- [ADO.NET Basics](../../Concepts/Database/ADO.NET%20Basics.md) (Connecting to databases, reading data)

## Key Takeaways
- Use properties with backing fields when validation is required.
- C# requires static methods to be called from a static context (like `Main`).
- Exception handling should go from specific to general.
- `Task.WhenAll` does not spawn threads; it is a coordinator.
- Use `Task.Delay` for I/O bound waits, not `Thread.Sleep`.

## Files Created / Updated
- `Concepts/C#/Properties and Backing Fields.md`
- `Concepts/C#/Interfaces and Abstract Classes.md`
- `Concepts/C#/Static Methods.md`
- `Concepts/C#/Exception Handling.md`
- `Concepts/Async Programming/Async vs Sync.md`
- `Concepts/Async Programming/Task vs Thread.md`
- `Concepts/Async Programming/Blocking vs Non-Blocking.md`
- `Concepts/Async Programming/Thread Safe Collections.md`
- `Concepts/Database/ADO.NET Basics.md`
- `Code Examples/ADO.NET Demo.cs`

## Knowledge Gaps / Backlog
- Garbage Collector generations.
- What are Active references?
- Managed vs Unmanaged objects (Is a file object unmanaged?)
- Deeper dive into LINQ (IEnumerable).

## Preview of Next Week
- Deeper dive into ASP.NET Core and Enterprise APIs.
