# Day 9 Summary
**Date:** 9th July 2026

## Core Topics Covered

### 1. Software Development Life Cycle (SDLC)
- Discussed the promotion path from Requirements -> Tech Doc -> Coding -> PR -> Testing -> INT -> UAT -> PPE -> PROD. [SDLC](../../Concepts/Architecture/Software%20Development%20Life%20Cycle.md)
- Emphasized using wireframes instead of high-fidelity images during development.

### 2. Authentication & Authorization
- Explored Role-Based Access Control (RBAC). [Authentication and Authorization](../../Concepts/Web%20API/Authentication%20and%20Authorization.md)
- Demonstrated a complete JWT Token implementation with `AppUser`, `IAuthService`, and controllers secured by `[Authorize(Roles = "...")]`. [Auth System Setup](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Auth%20System%20Setup.md)
- Mapped out dashboard access flows (Admin vs Trainer vs Student).

### 3. Data Transfer Objects (DTO)
- Detailed explanation of why DTOs are critical at the API boundary. [Data Transfer Objects (DTO)](../../Concepts/Web%20API/Data%20Transfer%20Objects%20(DTO).md)
- DTOs protect the internal model, prevent over-posting, define clear contracts, and handle validation naturally.
- Used the "Company Office" analogy: DTOs are the reception forms, Models are the warehouse boxes.

### 4. Product Management System (End-to-End Example)
- Built a layered ASP.NET Core API using In-Memory collections. [Data Persistence Strategies](../../Concepts/Web%20API/Data%20Persistence%20Strategies.md), [Core System](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Product%20Management%20System%20(Core).md), [Models & DTOs](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Product%20Management%20System%20(Models%20&%20DTOs).md)
- **Resilience**: Implemented a Resilient Store Wrapper with Retry and an In-Memory Circuit Breaker. [Resilience & Idempotency](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Product%20Management%20System%20(Resilience%20&%20Idempotency).md)
- **Idempotency**: Implemented an `Idempotency-Key` mechanism using `ConcurrentDictionary` and request hashing to prevent duplicate POST creations. [Resilience & Idempotency](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Product%20Management%20System%20(Resilience%20&%20Idempotency).md)
- **Options Pattern**: Configured resilience and store simulation settings strongly typed via `appsettings.json`. [Persistence & Options](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Product%20Management%20System%20(Persistence%20&%20Options).md)
- **Controllers & Middleware**: Handled exceptions and cross-cutting concerns effectively. [Middleware & Controllers](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Product%20Management%20System%20(Middleware%20&%20Controllers).md)
- **Services**: Handled core business logic bridging DTOs and Data Stores. [Services & Diagnostics](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Product%20Management%20System%20(Services%20&%20Diagnostics).md)
- **UI**: Added an HTML management interface with client-side live searching and sorting. [Static Files](../../Concepts/Web%20API/Static%20Files.md), [Course Gallery UI](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Course%20Gallery%20UI.md), [UI & Live Search](../../Code%20Examples/Day%209%20-%20Product%20Management%20System/Product%20Management%20System%20(UI%20&%20Live%20Search).md)

### 5. Dependency Injection Nuances
- **Postman Testing**: Highlighted that Postman treats every request as a new client, so `AddScoped` services will reset state. Used `AddSingleton` to maintain in-memory state across multiple Postman calls. [Dependency Injection](../../Concepts/Web%20API/Dependency%20Injection.md)
- **Middleware Lifecycle**: Emphasized the anti-pattern of injecting scoped services into middleware constructors. Always inject into `InvokeAsync`.
