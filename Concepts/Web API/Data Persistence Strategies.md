---
tags: [architecture, persistence, database]
---
# Data Persistence Strategies

When developing without connecting to a real database (like SQL Server or Postgres), there are a few ways to persist and manage data. It is crucial to understand the difference between in-memory collections and persistent file-backed stores.

## 1. In-Memory Stores
Using structures like `ConcurrentDictionary` or `List<T>` inside a Singleton service.

- **Characteristics**: Fast, easy to setup.
- **Limitation**: Any changes made while the application is running (creates, updates, deletes) are completely lost when the application restarts. Memory is like a whiteboard: when the app stops, the whiteboard is wiped clean.

### Seeding Data
If you need usable data every time the app starts but don't care if edits are lost across restarts, you can **seed** data within the constructor of your in-memory store.

```csharp
public InMemoryProductDataStore()
{
    SeedProducts();
}

private void SeedProducts()
{
    // Add default items so the app doesn't start empty
    AddSeedProduct("Laptop", 1499.99m, "Lenovo");
}
```

## 2. JSON File-Backed Stores
If your goal is "I don't want my edits to disappear after a restart" but you still don't want to use a real database, use a JSON file-backed store.

- **Characteristics**: The application reads `products.json` on startup, loads it into memory, and on every create/update/delete, it saves the changes back to `products.json`.
- **Limitation**: Not safe for heavy concurrent production workloads, but perfect for preserving state during development across restarts.

## Clean Architecture Upgrade Path
If you use the Repository Pattern (e.g., `IProductDataStore`), switching strategies is effortless. 

```text
Controller -> Service -> ResilientProductStore -> InMemoryProductDataStore
```
can be cleanly upgraded to:
```text
Controller -> Service -> ResilientProductStore -> JsonProductDataStore
```

The controllers, services, middleware, and DTOs remain exactly the same; only the lowest layer changes.
