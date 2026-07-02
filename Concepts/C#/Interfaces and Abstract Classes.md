---
tags: [csharp, oop, abstract-classes, interfaces, inheritance]
aliases: [Abstract Classes, Interfaces]
---
# Interfaces and Abstract Classes

## Abstract Classes
An abstract class is a class that cannot be instantiated directly. It is designed to be a base class for other classes to inherit from.

- Can contain **virtual methods** (methods with a default implementation that *can* be overridden).
- Can contain **abstract methods** (methods with *no* implementation that *must* be overridden).
- **Generics** can be used in the parent class.
- The child class usually passes variables which can be handled by the parent class using the `base` keyword.

> [!NOTE]
> **Why doesn't C# provide abstract methods in non-abstract classes?**
> If a class has an abstract method, it is incomplete. Allowing an incomplete class to be instantiated would lead to runtime errors if the abstract method was called.

## Interfaces
Interfaces define a contract. Any class that implements an interface must provide an implementation for all of its members.

### Why use an interface?
- To hide implementation details and expose only the necessary actions (Abstraction).
- To achieve a form of multiple inheritance. C# does **not** support multiple class inheritance (a class cannot inherit from multiple classes), but a class **can** implement multiple interfaces.

### Naming Convention
In C#, interfaces traditionally start with a capital `I` (e.g., `IEnumerable`, `IDisposable`, `IProduct`).

## Code Examples
- **Abstract/Sealed classes:** See [delivery.cs](../../DONE/delivery.cs) for an example of a base `Delivery` class and derived classes like `StandardDelivery` and `FreeDelivery` (sealed).
- **Interface Implementation:** See [ElectronicProduct.cs](../../DONE/ElectronicProduct.cs) for a class inheriting a base class (`Product2`) while simultaneously implementing an interface (`IReturnable`).

---
## See Also
- [Classes and Objects](Classes%20and%20Objects.md)
