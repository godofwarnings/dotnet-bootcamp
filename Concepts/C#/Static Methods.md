---
tags: [csharp, methods, static]
aliases: [Static]
---
# Static Methods

In C#, a `static` method belongs to the **class itself**, not to an object instance. 

- **Static Method:** Call with the class name (e.g., `Program.PrintDeliveries()`) or directly from another static method within the same class.
- **Instance Method:** Belongs to an object, so it can only be called through an object instance (e.g., `someProgram.PrintDeliveries()`).

## Why the `Main` method requires static helpers
The entry point of a C# console application, `Main()`, is always declared as `static`. 

If you try to call a non-static method directly from `Main()`, the compiler will complain. It is basically saying, "Call an object method," but no object was created.

### Example: The Problem
```csharp
public class Program
{
    public void PrintDeliveries(decimal orderAmount) // Instance method
    {
        // ...
    }

    public static void Main()
    {
        // ERROR: Cannot access non-static method from static context
        PrintDeliveries(1000); 
    }
}
```

### The Solution (Make it Static)
If a helper method does not depend on any instance fields or properties, the cleanest choice is to make it static.

```csharp
public class Program
{
    // Now it belongs to the class
    public static void PrintDeliveries(decimal orderAmount) 
    {
        // ...
    }

    public static void Main()
    {
        // Works!
        PrintDeliveries(1000); 
    }
}
```

### Alternative Solution (Instantiate)
If you *must* have an instance method, you need to create an object first:

```csharp
public static void Main()
{
    Program program = new Program(); 
    program.PrintDeliveries(1000); // Call through the instance
}
```

---
## See Also
- [Classes and Objects](Classes%20and%20Objects.md)
