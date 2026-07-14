# Service Boundaries Refactoring

After identifying the code smells in the original monolithic `OrderService`, the responsibilities were extracted into specialized domain services. The `OrderService` was then refactored to act strictly as an **Orchestrator**.

## The Refactored OrderService (Orchestrator)

Notice how the `OrderService` no longer contains hard-coded pricing logic or checks like `if (restaurantId == 999)`. Instead, it delegates these checks to the specialized services.

```csharp
using FoodOrderingApi.DTOs;
using FoodOrderingApi.Events;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICustomerService customerService;
        private readonly IRestaurantService restaurantService;
        private readonly IMenuService menuService;
        private readonly IPaymentService paymentService;
        private readonly IDeliveryService deliveryService;
        private readonly IEventBus eventBus;

        private static int nextOrderId = 9001;
        private static readonly Dictionary<string, PlaceOrderResponse> processedOrders = new();

        public OrderService(
            ICustomerService customerService,
            IRestaurantService restaurantService,
            IMenuService menuService,
            IPaymentService paymentService,
            IDeliveryService deliveryService,
            IEventBus eventBus)
        {
            this.customerService = customerService;
            this.restaurantService = restaurantService;
            this.menuService = menuService;
            this.paymentService = paymentService;
            this.deliveryService = deliveryService;
            this.eventBus = eventBus;
        }

        public PlaceOrderResponse PlaceOrder(PlaceOrderRequest request, string idempotencyKey)
        {
            if (processedOrders.TryGetValue(idempotencyKey, out PlaceOrderResponse? existingResponse))
            {
                return existingResponse;
            }

            // 1. Validate Customer
            bool customerExists = customerService.CustomerExists(request.CustomerId);
            if (!customerExists)
            {
                throw new InvalidOperationException("Customer not found.");
            }

            // 2. Validate Restaurant
            bool restaurantExists = restaurantService.RestaurantExists(request.RestaurantId);
            if (!restaurantExists)
            {
                throw new InvalidOperationException("Restaurant not found.");
            }

            bool restaurantOpen = restaurantService.IsRestaurantOpen(request.RestaurantId);
            if (!restaurantOpen)
            {
                throw new ApplicationException("Restaurant is currently closed.");
            }

            // 3. Validate Menu Items & Calculate Pricing
            decimal totalAmount = 0;
            foreach (var item in request.Items)
            {
                MenuItem? menuItem = menuService.GetMenuItemById(item.MenuItemId);
                if (menuItem == null)
                {
                    throw new InvalidOperationException("One or more menu items were not found.");
                }

                if (!menuItem.IsAvailable)
                {
                    throw new ApplicationException("One or more menu items are currently unavailable.");
                }

                totalAmount += menuItem.Price * item.Quantity;
            }

            // 4. Process Payment
            PaymentResult paymentResult = paymentService.ProcessPayment(request.CustomerId, totalAmount);
            if (!paymentResult.IsSuccessful)
            {
                throw new ApplicationException("Payment failed.");
            }

            int orderId = nextOrderId++;

            // 5. Assign Delivery
            DeliveryAssignment deliveryAssignment = deliveryService.AssignDeliveryPartner(
                orderId,
                request.RestaurantId,
                request.DeliveryAddress);

            // 6. Build Response
            PlaceOrderResponse response = new PlaceOrderResponse
            {
                OrderId = orderId,
                CustomerId = request.CustomerId,
                RestaurantId = request.RestaurantId,
                Status = "OrderPlaced",
                TotalAmount = totalAmount,
                PaymentStatus = paymentResult.PaymentStatus,
                TransactionId = paymentResult.TransactionId,
                DeliveryPartnerName = deliveryAssignment.DeliveryPartnerName,
                EstimatedDeliveryTimeInMinutes = deliveryAssignment.EstimatedDeliveryTimeInMinutes,
                NotificationStatus = "NotificationQueued",
                CreatedAt = DateTime.UtcNow
            };

            processedOrders[idempotencyKey] = response;

            // 7. Publish Event (Asynchronous Side Effects)
            eventBus.Publish(new OrderPlacedEvent
            {
                OrderId = response.OrderId,
                CustomerId = response.CustomerId,
                RestaurantId = response.RestaurantId,
                TotalAmount = response.TotalAmount,
                PlacedAt = response.CreatedAt
            });

            return response;
        }
    }
}
```

## Example Domain Service (MenuService)

Here is how the pricing and availability logic was isolated into the `MenuService`. This service can now be unit-tested without any knowledge of Orders or Payments.

```csharp
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public interface IMenuService
    {
        MenuItem? GetMenuItemById(int menuItemId);
    }

    public class MenuService : IMenuService
    {
        private static readonly List<MenuItem> menuItems = new List<MenuItem>
        {
            new MenuItem { MenuItemId = 1, ItemName = "Chicken Biryani", Price = 250, IsAvailable = true },
            new MenuItem { MenuItemId = 2, ItemName = "Paneer Butter Masala", Price = 220, IsAvailable = true },
            new MenuItem { MenuItemId = 3, ItemName = "Mutton Biryani", Price = 350, IsAvailable = false },
            new MenuItem { MenuItemId = 4, ItemName = "Butter Naan", Price = 40, IsAvailable = true }
        };

        public MenuItem? GetMenuItemById(int menuItemId)
        {
            return menuItems.FirstOrDefault(m => m.MenuItemId == menuItemId);
        }
    }
}
```
