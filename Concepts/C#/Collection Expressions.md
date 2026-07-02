# Collection Expressions

In modern C#, you can initialize collections using collection expressions `[]` instead of the legacy object initializer syntax `{}`.

```csharp
// Modern (Collection Expressions)
List<int> availableSeats = [2, 0, 3];

// Legacy
List<int> availableSeats = new List<int> { 2, 0, 3 };
```

## Why the change?
- The `{}` syntax is older. Under the hood, the compiler translates it into multiple `.Add()` calls.
- The `[]` syntax (Collection Expressions) is highly optimized by the compiler, often resulting in more efficient memory allocation and avoiding repeated `.Add()` overhead.

## When to use which?
- Use `[]` for arrays, Lists, and Spans.
- Use `{}` for Dictionaries, as `[]` does not yet natively support Dictionary key-value pair initialization in the same way.
