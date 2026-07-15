# 05 - InMemoryEventBus Fix

### `InMemoryEventBus` — Fixed Implementation

An in-process event bus that dispatches an event to all registered handlers for that event type, resolving handlers from a **freshly created DI scope** on each publish.

```csharp
using FoodOrderingApi.EventHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderingApi.Services
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IServiceScopeFactory scopeFactory;

        public InMemoryEventBus(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void Publish<TEvent>(TEvent eventData)
        {
            using IServiceScope scope = scopeFactory.CreateScope();

            IEnumerable<IEventHandler<TEvent>> handlers =
                scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();

            foreach (IEventHandler<TEvent> handler in handlers)
            {
                handler.Handle(eventData);
            }
        }
    }
}
```

**Purpose.** Decouple event producers from consumers: producers call `Publish`, and every handler registered for that event type is invoked, without the producer knowing who the handlers are.

**Concepts demonstrated.**
- **Dependency injection & scope management** — `IServiceScopeFactory` creates a new scope per publish so that scoped services (e.g. `DbContext`) are resolved and disposed correctly.
- **Generics** — `Publish<TEvent>` and `IEventHandler<TEvent>` provide type-safe, per-event-type dispatch.
- **Multiple-registration resolution** — `GetServices<T>()` returns **all** handlers registered for the type (as opposed to `GetService<T>()`, which returns one).

**Important syntax.**
- `using IServiceScope scope = ...` — a C# 8 `using` declaration; the scope is disposed at the end of the enclosing method, guaranteeing cleanup.

**Why a scope matters (the fix).**
> [!WARNING]
> Resolving scoped services directly from the root provider (instead of from a created scope) causes a captive-dependency / lifetime bug: scoped services live for the lifetime of the singleton bus and are never disposed per operation. Creating a scope per `Publish` resolves this.

**Best practices.**
- Keep the bus itself a singleton, but always resolve handlers from a per-operation scope.
- Handlers should be idempotent where possible, since dispatch order across handlers is not guaranteed.

**Common mistakes.**
- Injecting `IServiceProvider`/scoped handlers directly into a singleton and reusing them across requests.
- Assuming a return value — this dispatch is `void` (fire-and-forget, synchronous).

**Performance implications.**
- Dispatch is **synchronous**: `Publish` returns only after every handler completes. A slow handler blocks the caller. For heavier workloads, consider an async bus or offloading to a background queue.
- Creating a scope per publish is cheap but non-zero; acceptable for typical request-scoped work.

**Real-world usage.** Suits in-process, single-instance event handling (e.g. raising `OrderPlaced` to trigger notifications). For cross-service or cross-instance delivery, replace with a real message broker (RabbitMQ, Azure Service Bus, Kafka).
