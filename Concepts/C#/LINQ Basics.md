---
tags: [csharp, linq, where, sum, collections]
aliases: [LINQ, Language Integrated Query]
---
# LINQ Basics

**LINQ** (Language Integrated Query) provides a unified syntax to query and manipulate data from different sources (like Collections, Databases, XML) directly from C#.

It makes data filtering and aggregation much cleaner than using manual `foreach` loops.

## Common LINQ Methods

See [Day1_Program.cs](../../Code%20Examples/Day%201%20-%20OOP/Day1_Program.cs) (RunProductExamples) for the concrete implementation.

### Filtering with `.Where()`
Filters a collection based on a condition (a predicate).

```csharp
List<Product> products = GetProducts();

// Filter products whose price is greater than 5000.
// 'product => product.Price > 5000' is a lambda expression.
var expensiveProducts = products.Where(product => product.Price > 5000);
```

### Aggregation with `.Sum()`
Calculates the sum of a sequence of numeric values.

```csharp
// Calculate total inventory value:
// price of each product multiplied by available stock.
decimal totalStockValue = products.Sum(product => product.Price * product.Stock);
```

> [!TIP]
> When using LINQ, make sure to include `using System.Linq;` at the top of your file.

---
## See Also
- [Classes and Objects](Classes%20and%20Objects.md)
