# Day 13 Summary (Microservices, Angular & Patches)

**Date:** July 15, 2026
**Topics:** Microservices Architecture, Angular Fundamentals, Event Bus & Swagger Testing

## 1. Microservices Architecture
Scaling an application isn't just about blindly replicating a monolithic codebase across servers. This wastes resources on low-demand features while under-provisioning hot ones.
The core driver for Microservices is **independent scalability**. Each feature or service is split so it can scale on its own based on actual load.

### Key Concepts
- **Containerization**: Each service is containerized independently (e.g., using **Docker** or **Podman**). A container packages one service with its dependencies into an isolated unit.
- **Polyglot Persistence & Programming**: Each service can pick the language and database that best fit its workload.
- **Inter-service Communication**: Services communicate over the network — synchronously via REST APIs, and asynchronously via a Message/Service Bus for decoupled, event-driven flows.
- **Virtual Machines vs. Containers**: VMs run a full guest OS on top of a hypervisor. Containers share the host OS's kernel and package only the application + binaries. This makes containers lighter, faster to start, and denser to pack.

*(See Code Examples for the Polyglot Reference Architecture diagram).*

---

## 2. Angular Fundamentals
Angular is a component-based web framework built with **TypeScript** (which adds static typing on top of JS). 

### Tooling & CLI
- **npm**: The package manager for the Node.js/JavaScript ecosystem (analogous to NuGet in .NET).
- **Angular CLI (`ng`)**: Used to serve the app (`ng serve -o`) and scaffold components (`ng generate component <name>`).

### Standalone Components
Modern Angular uses **standalone components** (no root `NgModule`). The app is bootstrapped directly from the root component in `main.ts`, and each component declares its own `imports`.

### Template Concepts
- **Interpolation (`{{ expression }}`)**: One-way binding from component state to view.
- **Property Binding (`[property]="expr"`)**: Binds DOM properties dynamically.
- **Event Binding (`(event)="handler()"`)**: Calls a component method on a DOM event.
- **Two-Way Binding (`[(ngModel)]`)**: The "Banana in a box". Syncs input and property (requires `FormsModule`).
- **Conditional Rendering (`@if` / `@else`)**: Built-in control flow to render blocks based on conditions.
- **Looping (`@for`)**: Built-in control flow for lists. It requires `track` to give each item a stable identity for efficient DOM re-rendering.
- **Pipes (`|`)**: Transforms a value for display without changing the underlying data (e.g. `uppercase`, `currency`, `date`).
- **Template Reference Variables (`#ref`)**: Exposes a DOM element inside the template for direct value reading.

*(See Code Examples for the Angular Practice Project implementations).*

---

## 3. Day 12 Patches
### The InMemoryEventBus Fix
The initial `InMemoryEventBus` implementation had a captive dependency bug because it was resolving scoped handlers directly from a singleton root provider. The fix uses `IServiceScopeFactory` to create a new scope per publish, ensuring scoped services (like `DbContext`) are resolved and disposed correctly.

*(See Code Examples for the corrected implementation).*

### Food Ordering API Swagger Test Cases
A complete manual test suite was documented for the Food Ordering API via Swagger UI. It covers happy paths, validation errors, Not Found (404), Business Conflicts (409), and verifies Idempotency using the `Idempotency-Key` header.

*(See Code Examples for the full test suite json data).*

---

## Code Examples
- [Microservices Architecture](../../Code%20Examples/Day%2013%20-%20Microservices%20and%20Angular/01%20-%20Microservices%20Architecture.md)
- [Angular Template Concepts](../../Code%20Examples/Day%2013%20-%20Microservices%20and%20Angular/02%20-%20Angular%20Template%20Concepts.md)
- [Angular Component Communication](../../Code%20Examples/Day%2013%20-%20Microservices%20and%20Angular/03%20-%20Angular%20Component%20Communication.md)
- [Angular Registration Preview](../../Code%20Examples/Day%2013%20-%20Microservices%20and%20Angular/04%20-%20Angular%20Registration%20Preview.md)
- [InMemoryEventBus Fix](../../Code%20Examples/Day%2013%20-%20Microservices%20and%20Angular/05%20-%20InMemoryEventBus%20Fix.md)
- [Food Ordering API Swagger Test Cases](../../Code%20Examples/Day%2013%20-%20Microservices%20and%20Angular/06%20-%20Food%20Ordering%20API%20Swagger%20Test%20Cases.md)
