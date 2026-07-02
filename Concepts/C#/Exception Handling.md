---
tags: [csharp, exceptions, try-catch, error-handling]
aliases: [Try Catch, Exceptions]
---
# Exception Handling

Exception handling in C# is done using `try`, `catch`, and `finally` blocks.

## The Structure

```csharp
try
{
    // Code that might throw an exception
}
catch (SpecificException ex)
{
    // Handle specific exceptions first
}
catch (Exception ex)
{
    // Always end with a general catch if you include a specific catch
}
finally
{
    // Code that ALWAYS executes, whether an exception occurred or not
}
```

## Best Practices
- **Catch Specific Exceptions:** Always catch specific exceptions (like `FileNotFoundException` or `ArgumentOutOfRangeException`) before catching the general `Exception`.
- **Always have a fallback:** End with a general `catch (Exception ex)` block to handle unforeseen errors if you are doing specific error handling above it.
- **Finally Block:** Use `finally` to clean up resources (like closing file streams or database connections) because it is guaranteed to execute.

## Custom Exceptions
You can define custom exceptions by inheriting from the `Exception` class. This is useful when you want to throw errors that are specific to your business logic.

```csharp
public class InvalidProductException : Exception
{
    public InvalidProductException(string message) : base(message)
    {
    }
}
```

## Code Examples
- **Custom Exception Definition:** See `InsufficientStockException` inside [Product.cs](../../DONE/Product.cs).
- **Multiple Catch Blocks:** See `RunUserInputExample` inside [Program.cs](../../DONE/Program.cs) for catching `FormatException` before `Exception`.

## Root Cause Analysis (RCA)
When exceptions occur in production, developers perform RCA to identify the underlying issue. Good exception handling (like logging the stack trace and inner exceptions) is critical for effective RCA.
