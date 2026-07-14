# Controllers and Program

Copy these files into your `Controllers` folder and replace your root `Program.cs`.

## Controllers

```csharp
// Controllers/OrdersController.cs
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
            if (request == null) return BadRequest(new { Message = "Request body is required." });
            if (string.IsNullOrWhiteSpace(idempotencyKey)) return BadRequest(new { Message = "Idempotency-Key header is required." });

            try
            {
                PlaceOrderResponse response = orderService.PlaceOrder(request, idempotencyKey);
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

## Program.cs

This wires up all the dependency injection for the final architecture, including the domain services, the Anti-Corruption Layer, the Event Bus, and all the Pub/Sub handlers.

```csharp
// Program.cs
using FoodOrderingApi.Adapters;
using FoodOrderingApi.EventHandlers;
using FoodOrderingApi.Events;
using FoodOrderingApi.ExternalServices;
using FoodOrderingApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Event Bus Registration (Singleton)
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// Domain Service Registrations (Scoped)
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();

// Pub/Sub Subscriber Registrations (Scoped)
builder.Services.AddScoped<IEventHandler<OrderPlacedEvent>, ReportingOrderPlacedHandler>();
builder.Services.AddScoped<IEventHandler<OrderPlacedEvent>, NotificationOrderPlacedHandler>();
builder.Services.AddScoped<IEventHandler<OrderPlacedEvent>, LoyaltyPointsOrderPlacedHandler>();

// Anti-Corruption Layer Registrations (Scoped)
builder.Services.AddScoped<ILegacyRestaurantApiClient, LegacyRestaurantApiClient>();
builder.Services.AddScoped<ILegacyRestaurantAdapter, LegacyRestaurantAdapter>();
builder.Services.AddScoped<ILegacyPaymentGatewayClient, LegacyPaymentGatewayClient>();
builder.Services.AddScoped<ILegacyPaymentAdapter, LegacyPaymentAdapter>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
```
