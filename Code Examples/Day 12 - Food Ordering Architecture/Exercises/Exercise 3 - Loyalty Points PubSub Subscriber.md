# Exercise 3 - Loyalty Points PubSub Subscriber

## Requirement
When `OrderPlacedEvent` is published, add a new subscriber called `LoyaltyPointsOrderPlacedHandler` that adds loyalty points based on the order's total amount.

## Student Tasks
1. Do not modify `OrderService`.
2. Do not modify `InMemoryEventBus`.
3. Create `LoyaltyPointsOrderPlacedHandler`.
4. Register the handler in `Program.cs`.
5. Print the assigned loyalty points to the console.

## Expected Direction
- If the total amount is 500 or above, add 50 loyalty points.
- Otherwise, add 10 loyalty points.
- Console output: `Loyalty points added for CustomerId: 101, Points: 50`

## Solution

### 1. The Handler

```csharp
using FoodOrderingApi.Events;
using System.Diagnostics;

namespace FoodOrderingApi.EventHandlers
{
    public class LoyaltyPointsOrderPlacedHandler : IEventHandler<OrderPlacedEvent>
    {
        public void Handle(OrderPlacedEvent eventData)
        {
            int points = eventData.TotalAmount >= 500m ? 50 : 10;
            
            Console.WriteLine(
                $"Loyalty points added for CustomerId: {eventData.CustomerId}, Points: {points}");
                
            Debug.WriteLine(
                $"Loyalty points added for CustomerId: {eventData.CustomerId}, Points: {points}");
        }
    }
}
```

### 2. The Registration (in `Program.cs`)

Notice how adding this feature requires absolutely zero changes to the core `OrderService`. We simply register a new handler to listen to the existing event.

```csharp
builder.Services.AddScoped<IEventHandler<OrderPlacedEvent>, LoyaltyPointsOrderPlacedHandler>();
```
