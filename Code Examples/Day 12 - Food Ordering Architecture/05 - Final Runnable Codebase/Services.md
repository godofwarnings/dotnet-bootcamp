# Services (Domain and Orchestrator)

Copy these files into your `Services` folder.

## Core Domain Services

```csharp
// Services/ICustomerService.cs
namespace FoodOrderingApi.Services
{
    public interface ICustomerService
    {
        bool CustomerExists(int customerId);
    }
}

// Services/CustomerService.cs
namespace FoodOrderingApi.Services
{
    public class CustomerService : ICustomerService
    {
        public bool CustomerExists(int customerId)
        {
            if (customerId == 404) return false;
            return true;
        }
    }
}

// Services/IRestaurantService.cs
namespace FoodOrderingApi.Services
{
    public interface IRestaurantService
    {
        bool RestaurantExists(int restaurantId);
        bool IsRestaurantOpen(int restaurantId);
    }
}

// Services/RestaurantService.cs
using FoodOrderingApi.Adapters;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ILegacyRestaurantAdapter legacyRestaurantAdapter;

        public RestaurantService(ILegacyRestaurantAdapter legacyRestaurantAdapter)
        {
            this.legacyRestaurantAdapter = legacyRestaurantAdapter;
        }

        public bool RestaurantExists(int restaurantId)
        {
            RestaurantInfo? info = legacyRestaurantAdapter.GetRestaurantInfo(restaurantId);
            return info != null;
        }

        public bool IsRestaurantOpen(int restaurantId)
        {
            RestaurantInfo? info = legacyRestaurantAdapter.GetRestaurantInfo(restaurantId);
            if (info == null) return false;
            return info.IsActive && info.IsOpen && info.IsServingNow;
        }
    }
}

// Services/IMenuService.cs
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public interface IMenuService
    {
        MenuItem? GetMenuItemById(int menuItemId);
    }
}

// Services/MenuService.cs
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
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

// Services/IPaymentService.cs
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public interface IPaymentService
    {
        PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod = "CreditCard");
    }
}

// Services/PaymentService.cs
using FoodOrderingApi.Adapters;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILegacyPaymentAdapter legacyPaymentAdapter;

        public PaymentService(ILegacyPaymentAdapter legacyPaymentAdapter)
        {
            this.legacyPaymentAdapter = legacyPaymentAdapter;
        }

        public PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod = "CreditCard")
        {
            if (customerId <= 0) throw new ArgumentException("Invalid customer id.");
            if (amount <= 0) throw new ArgumentException("Payment amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(paymentMethod)) throw new ArgumentException("Payment method is required.");

            return legacyPaymentAdapter.ProcessPayment(customerId, amount, paymentMethod);
        }
    }
}

// Services/IDeliveryService.cs
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public interface IDeliveryService
    {
        DeliveryAssignment AssignDeliveryPartner(int orderId, int restaurantId, string deliveryAddress);
    }
}

// Services/DeliveryService.cs
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public class DeliveryService : IDeliveryService
    {
        public DeliveryAssignment AssignDeliveryPartner(int orderId, int restaurantId, string deliveryAddress)
        {
            return new DeliveryAssignment
            {
                DeliveryPartnerName = "Rahul Delivery Partner",
                EstimatedDeliveryTimeInMinutes = 35
            };
        }
    }
}
```

## The Event Bus

```csharp
// Services/IEventBus.cs
namespace FoodOrderingApi.Services
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent eventData);
    }
}

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
            IEnumerable<IEventHandler<TEvent>> handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();
            foreach (IEventHandler<TEvent> handler in handlers)
            {
                handler.Handle(eventData);
            }
        }
    }
}
```

## The Orchestrator

```csharp
// Services/IOrderService.cs
using FoodOrderingApi.DTOs;

namespace FoodOrderingApi.Services
{
    public interface IOrderService
    {
        PlaceOrderResponse PlaceOrder(PlaceOrderRequest request, string idempotencyKey);
    }
}

// Services/OrderService.cs
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
            ICustomerService customerService, IRestaurantService restaurantService, IMenuService menuService,
            IPaymentService paymentService, IDeliveryService deliveryService, IEventBus eventBus)
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
            if (processedOrders.TryGetValue(idempotencyKey, out PlaceOrderResponse? existingResponse)) return existingResponse;

            if (request.CustomerId <= 0) throw new ArgumentException("Invalid customer id.");
            if (request.RestaurantId <= 0) throw new ArgumentException("Invalid restaurant id.");
            if (request.Items == null || request.Items.Count == 0) throw new ArgumentException("At least one item is required.");
            if (string.IsNullOrWhiteSpace(request.DeliveryAddress)) throw new ArgumentException("Delivery address is required.");

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0) throw new ArgumentException("Item quantity must be greater than zero.");
            }

            bool customerExists = customerService.CustomerExists(request.CustomerId);
            if (!customerExists) throw new InvalidOperationException("Customer not found.");

            bool restaurantExists = restaurantService.RestaurantExists(request.RestaurantId);
            if (!restaurantExists) throw new InvalidOperationException("Restaurant not found.");

            bool restaurantOpen = restaurantService.IsRestaurantOpen(request.RestaurantId);
            if (!restaurantOpen) throw new ApplicationException("Restaurant is currently closed.");

            decimal totalAmount = 0;
            foreach (var item in request.Items)
            {
                MenuItem? menuItem = menuService.GetMenuItemById(item.MenuItemId);
                if (menuItem == null) throw new InvalidOperationException("One or more menu items were not found.");
                if (!menuItem.IsAvailable) throw new ApplicationException("One or more menu items are currently unavailable.");
                totalAmount += menuItem.Price * item.Quantity;
            }

            PaymentResult paymentResult = paymentService.ProcessPayment(request.CustomerId, totalAmount, request.PaymentMethod);
            if (!paymentResult.IsSuccessful) throw new ApplicationException("Payment failed.");

            int orderId = nextOrderId++;
            DeliveryAssignment deliveryAssignment = deliveryService.AssignDeliveryPartner(orderId, request.RestaurantId, request.DeliveryAddress);

            PlaceOrderResponse response = new PlaceOrderResponse
            {
                OrderId = orderId, CustomerId = request.CustomerId, RestaurantId = request.RestaurantId,
                Status = "OrderPlaced", TotalAmount = totalAmount, PaymentStatus = paymentResult.PaymentStatus,
                TransactionId = paymentResult.TransactionId, DeliveryPartnerName = deliveryAssignment.DeliveryPartnerName,
                EstimatedDeliveryTimeInMinutes = deliveryAssignment.EstimatedDeliveryTimeInMinutes, NotificationStatus = "NotificationQueued",
                CreatedAt = DateTime.UtcNow
            };

            processedOrders.Add(idempotencyKey, response);

            eventBus.Publish(new OrderPlacedEvent
            {
                OrderId = response.OrderId, CustomerId = response.CustomerId, RestaurantId = response.RestaurantId,
                TotalAmount = response.TotalAmount, PlacedAt = response.CreatedAt
            });

            return response;
        }
    }
}
```
