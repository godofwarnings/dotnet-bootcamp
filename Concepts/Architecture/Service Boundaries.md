# Service Boundaries

In a monolithic architecture or a "God Class", a single service (e.g., `OrderService`) might be responsible for validation, database lookups, pricing, notification, and third-party integrations. This makes the service fragile, hard to test, and difficult to maintain.

**Service Boundaries** is an architectural pattern where responsibilities are separated into distinct domain services, each owning a specific piece of business logic.

## The Role of the Orchestrator
When extracting business logic into specialized services, the original service (like `OrderService`) becomes an **Orchestrator**. 

An orchestrator's job is **not** to perform business rules (like calculating a discount or validating a customer). Instead, its job is to coordinate the flow by calling the domain services in the correct order.

### Example Flow (Food Ordering)
Instead of `OrderService` querying the database to check if a restaurant is open:
1. `OrderService` (Orchestrator) calls `RestaurantService.ValidateRestaurant(id)`.
2. `RestaurantService` owns the logic to determine if the restaurant exists, is active, and is currently open.
3. `OrderService` (Orchestrator) calls `MenuService.ValidateItemsAndCalculateTotal(...)`.
4. `MenuService` owns the pricing logic.

## Benefits
- **Separation of Ownership:** Refactoring is not just moving lines of code; it is separating ownership. If pricing logic changes, you only touch `MenuService`.
- **Testability:** You can easily unit test `MenuService` in isolation without needing to mock delivery or payment dependencies.
- **Readability:** The orchestrator reads like a high-level flowchart of the business process.
