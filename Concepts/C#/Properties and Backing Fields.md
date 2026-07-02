---
tags: [csharp, encapsulation, properties, fields]
aliases: [Properties, Backing Fields, Encapsulation]
---
# Properties and Backing Fields

## Encapsulation
Encapsulation is about hiding internal state from the outside world. Only the class itself should control the behavior and mutation of its class variables.

## Backing Fields vs Properties
In C#, you often see a pattern of using a private field alongside a public property.

- **Private Field (Backing Field):** The actual storage for the data. (e.g., `private int stock;`)
- **Public Property:** The public way to read or update that value. It acts as a gatekeeper. (e.g., `public int Stock { get; set; }`)

> [!WARNING]
> **Common Mistake: Infinite Recursion**
> If you use the property inside its own `get` or `set`, it keeps calling itself until it causes a `StackOverflowException`.
> 
> **Incorrect Example:**
> ```csharp
> public int Stock { 
>     get { return Stock; } // Calls itself!
>     set { Stock = value; } // Calls itself!
> }
> ```

## Idiomatic C# Approaches

### 1. With Validation (Using a Backing Field)
If you need to validate data before setting it, use a private backing field. It is idiomatic in C# to **throw an exception** for invalid data rather than just printing a message.

```csharp
private int stock;

public int Stock 
{ 
    get => stock; 
    set 
    { 
        if (value < 0) 
            throw new ArgumentOutOfRangeException(nameof(value), "Stock cannot be negative.");
        stock = value;
    }
}
```

### 2. Without Validation (Auto-Property)
If no validation is needed, use an auto-property. The compiler automatically creates a hidden backing field for you.

```csharp
public int Stock { get; set; }
```

### 3. Constructor Validation
If an object should *never* start in an invalid state, you must validate in the constructor as well.

```csharp
public Product(int stock) 
{ 
    if (stock < 0) 
        throw new ArgumentOutOfRangeException(nameof(stock), "Stock cannot be negative.");
    
    Stock = stock;
}

public int Stock { get; private set; } // Can only be set from within the class
```

## Rule of Thumb
- No validation needed $\rightarrow$ use an **auto-property**.
- Validation needed $\rightarrow$ use a **property with a backing field**.
- Object should never be invalid $\rightarrow$ validate in the **constructor** and/or setter.

---
## See Also
- [Classes and Objects](Classes%20and%20Objects.md)
