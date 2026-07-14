# Pub/Sub Implementation

This example demonstrates how an in-memory Publish-Subscribe (Event Bus) system decouples asynchronous side-effects from the main transaction flow.

## 1. The Event

Events should contain enough data for the handlers to do their jobs without needing to immediately query the database again.

```csharp
// Events/OrderPlacedEvent.cs
namespace FoodOrderingApi.Events
{
    public class OrderPlacedEvent
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PlacedAt { get; set; }
    }
}
```

## 2. The Interfaces

The `IEventBus` is used by the publisher (e.g., `OrderService`). The `IEventHandler<TEvent>` is implemented by the subscribers.

```csharp
// Services/IEventBus.cs
namespace FoodOrderingApi.Services
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent eventData);
    }
}

// EventHandlers/IEventHandler.cs
namespace FoodOrderingApi.EventHandlers
{
    public interface IEventHandler<TEvent>
    {
        void Handle(TEvent eventData);
    }
}
```

## 3. The In-Memory Event Bus

This simple event bus uses ASP.NET Core's Dependency Injection (`IServiceProvider`) to find all registered handlers for a specific event type and execute them.

```csharp
// Services/InMemoryEventBus.cs
using FoodOrderingApi.EventHandlers;

namespace FoodOrderingApi.Services
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IServiceProvider serviceProvider;

        public InMemoryEventBus(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Publish<TEvent>(TEvent eventData)
        {
            // Find all classes that implement IEventHandler<TEvent>
            IEnumerable<IEventHandler<TEvent>> handlers =
                serviceProvider.GetServices<IEventHandler<TEvent>>();

            foreach (IEventHandler<TEvent> handler in handlers)
            {
                handler.Handle(eventData);
            }
        }
    }
}
```

## 4. The Handlers (Subscribers)

These classes react to the event. Because of the Pub/Sub design, you can add as many of these as you want without ever changing the `OrderService`.

```csharp
// EventHandlers/NotificationOrderPlacedHandler.cs
using FoodOrderingApi.Events;
using System.Diagnostics;

namespace FoodOrderingApi.EventHandlers
{
    public class NotificationOrderPlacedHandler : IEventHandler<OrderPlacedEvent>
    {
        public void Handle(OrderPlacedEvent eventData)
        {
            Debug.WriteLine($"Sending SMS Notification for Order {eventData.OrderId} to Customer {eventData.CustomerId}.");
        }
    }
}

// EventHandlers/ReportingOrderPlacedHandler.cs
using FoodOrderingApi.Events;
using System.Diagnostics;

namespace FoodOrderingApi.EventHandlers
{
    public class ReportingOrderPlacedHandler : IEventHandler<OrderPlacedEvent>
    {
        public void Handle(OrderPlacedEvent eventData)
        {
            Debug.WriteLine($"Updating Sales Dashboard. Total Amount: {eventData.TotalAmount}.");
        }
    }
}

// EventHandlers/LoyaltyPointsOrderPlacedHandler.cs
using FoodOrderingApi.Events;
using System.Diagnostics;

namespace FoodOrderingApi.EventHandlers
{
    public class LoyaltyPointsOrderPlacedHandler : IEventHandler<OrderPlacedEvent>
    {
        public void Handle(OrderPlacedEvent eventData)
        {
            int points = eventData.TotalAmount >= 500m ? 50 : 10;
            Debug.WriteLine($"Loyalty points added for CustomerId: {eventData.CustomerId}, Points: {points}");
        }
    }
}
```

## 5. Dependency Injection Registration

In `Program.cs`, you must register the bus as a Singleton, and all the handlers using the generic interface.

```csharp
// Register the Bus
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// Register all subscribers for OrderPlacedEvent
builder.Services.AddScoped<IEventHandler<OrderPlacedEvent>, ReportingOrderPlacedHandler>();
builder.Services.AddScoped<IEventHandler<OrderPlacedEvent>, NotificationOrderPlacedHandler>();
builder.Services.AddScoped<IEventHandler<OrderPlacedEvent>, LoyaltyPointsOrderPlacedHandler>();
```
