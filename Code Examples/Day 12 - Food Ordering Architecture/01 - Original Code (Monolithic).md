# Original Code (Monolithic)

This represents the original "God Class" implementation of the `OrderService` before applying any Service Boundaries or Anti-Corruption Layers.

Notice how the `OrderService` handles everything:
1. Idempotency checks.
2. Hard-coded restaurant validation (`if (request.RestaurantId == 999)`).
3. Menu item pricing and availability validation.
4. Total amount calculation.

This is a classic example of tight coupling.

## Models and DTOs

```csharp
// Models/MenuItem.cs
namespace FoodOrderingApi.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}

// DTOs/PlaceOrderRequest.cs
namespace FoodOrderingApi.DTOs
{
    public class PlaceOrderRequest
    {
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }
}

// DTOs/OrderItemRequest.cs
namespace FoodOrderingApi.DTOs
{
    public class OrderItemRequest
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}

// DTOs/PlaceOrderResponse.cs
namespace FoodOrderingApi.DTOs
{
    public class PlaceOrderResponse
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int EstimatedDeliveryTimeInMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
```

## The Monolithic OrderService

```csharp
using FoodOrderingApi.DTOs;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public interface IOrderService
    {
        PlaceOrderResponse PlaceOrder(PlaceOrderRequest request, string idempotencyKey);
    }

    public class OrderService : IOrderService
    {
        private static int nextOrderId = 9001;

        private static readonly Dictionary<string, PlaceOrderResponse> processedOrders =
            new Dictionary<string, PlaceOrderResponse>();

        private static readonly List<MenuItem> menuItems = new List<MenuItem>
        {
            new MenuItem { MenuItemId = 1, ItemName = "Chicken Biryani", Price = 250, IsAvailable = true },
            new MenuItem { MenuItemId = 2, ItemName = "Paneer Butter Masala", Price = 220, IsAvailable = true },
            new MenuItem { MenuItemId = 3, ItemName = "Mutton Biryani", Price = 350, IsAvailable = false },
            new MenuItem { MenuItemId = 4, ItemName = "Butter Naan", Price = 40, IsAvailable = true }
        };

        public PlaceOrderResponse PlaceOrder(
            PlaceOrderRequest request,
            string idempotencyKey)
        {
            if (processedOrders.ContainsKey(idempotencyKey))
            {
                return processedOrders[idempotencyKey];
            }

            if (request.CustomerId <= 0)
            {
                throw new ArgumentException("Invalid customer id.");
            }

            if (request.RestaurantId <= 0)
            {
                throw new ArgumentException("Invalid restaurant id.");
            }

            // Hardcoded external dependency simulation
            if (request.RestaurantId == 999)
            {
                throw new InvalidOperationException("Restaurant not found.");
            }

            if (request.RestaurantId == 500)
            {
                throw new ApplicationException("Restaurant is currently closed.");
            }

            if (request.Items == null || request.Items.Count == 0)
            {
                throw new ArgumentException("At least one item is required.");
            }

            if (string.IsNullOrWhiteSpace(request.DeliveryAddress))
            {
                throw new ArgumentException("Delivery address is required.");
            }

            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                {
                    throw new ArgumentException("Item quantity must be greater than zero.");
                }

                MenuItem? menuItem = menuItems.FirstOrDefault(m => m.MenuItemId == item.MenuItemId);

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

            PlaceOrderResponse response = new PlaceOrderResponse
            {
                OrderId = nextOrderId,
                CustomerId = request.CustomerId,
                RestaurantId = request.RestaurantId,
                Status = "OrderPlaced",
                TotalAmount = totalAmount,
                EstimatedDeliveryTimeInMinutes = 35,
                CreatedAt = DateTime.UtcNow
            };

            nextOrderId++;
            processedOrders.Add(idempotencyKey, response);

            return response;
        }
    }
}
```

## The OrdersController

```csharp
using FoodOrderingApi.DTOs;
using FoodOrderingApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public IActionResult PlaceOrder(
            [FromBody] PlaceOrderRequest request,
            [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Request body is required." });
            }

            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                return BadRequest(new { Message = "Idempotency-Key header is required." });
            }

            try
            {
                PlaceOrderResponse response = orderService.PlaceOrder(
                    request,
                    idempotencyKey);

                return Created($"/api/orders/{response.OrderId}", response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }
    }
}
```
