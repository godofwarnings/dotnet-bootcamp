# Events and Handlers (Pub/Sub)

Copy these files into your `Events` and `EventHandlers` folders. This code handles asynchronous side effects without blocking the `OrderService`.

## The Event

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

## The Generic Interface

```csharp
// EventHandlers/IEventHandler.cs
namespace FoodOrderingApi.EventHandlers
{
    public interface IEventHandler<TEvent>
    {
        void Handle(TEvent eventData);
    }
}
```

## The Subscribers

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
            Console.WriteLine($"Sending SMS Notification for Order {eventData.OrderId} to Customer {eventData.CustomerId}.");
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
            Console.WriteLine($"Updating Sales Dashboard. Total Amount: {eventData.TotalAmount}.");
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
            Console.WriteLine($"Loyalty points added for CustomerId: {eventData.CustomerId}, Points: {points}");
        }
    }
}
```
