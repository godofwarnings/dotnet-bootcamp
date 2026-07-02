# Nullability and Defaults: `""` vs `null`

In C#, setting a class variable to an empty string (`""` or `string.Empty`) creates a valid string object with zero characters. Not setting it to anything causes the variable to default to `null` (no object exists at all).

| Feature | Empty String (`""`) | Unassigned (`null`) |
| :--- | :--- | :--- |
| **Default Value** | An actual string object of length 0 | `null` |
| **Memory Allocation** | Points to a valid memory address (interned) | Points to nothing (null reference) |
| **Calling Methods** | Safe (e.g., `.Length` returns 0) | Throws a `NullReferenceException` |
| **Compiler Warnings** | None | Triggers `CS8618` if Nullable Reference Types are active |

## 1. Memory and Object Existence
Because C# uses string interning, all empty strings share the exact same memory address, making it incredibly memory efficient. Unassigned strings, being reference types, default to `null`.

## 2. Runtime Behavior (The Null Risk)
With an empty string, you can safely perform operations without checking it first. With `null`, accessing properties crashes the app.

## 3. Nullable Reference Types & Compiler Warnings
Modern C# (C# 8.0+) introduces Nullable Reference Types. If a property is non-nullable (`string Address`), the compiler expects it to be initialized. If not, it throws a warning. You must either initialize it (`= ""`) or explicitly mark it nullable (`string? Address`).

## Best Practices
To check whether a string is missing or empty safely without crashing, use built-in helpers:
- `string.IsNullOrEmpty(variable)`
- `string.IsNullOrWhiteSpace(variable)`
