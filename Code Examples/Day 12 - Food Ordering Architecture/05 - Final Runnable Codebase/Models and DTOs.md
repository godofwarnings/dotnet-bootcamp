# Models and DTOs

Copy these files into your `Models` and `DTOs` folders respectively.

## Models

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

// Models/PaymentResult.cs
namespace FoodOrderingApi.Models
{
    public class PaymentResult
    {
        public bool IsSuccessful { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
    }
}

// Models/RestaurantInfo.cs
namespace FoodOrderingApi.Models
{
    public class RestaurantInfo
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsOpen { get; set; }
        public bool IsServingNow { get; set; }
    }
}

// Models/DeliveryAssignment.cs
namespace FoodOrderingApi.Models
{
    public class DeliveryAssignment
    {
        public string DeliveryPartnerName { get; set; } = string.Empty;
        public int EstimatedDeliveryTimeInMinutes { get; set; }
    }
}
```

## DTOs

```csharp
// DTOs/OrderItemRequest.cs
namespace FoodOrderingApi.DTOs
{
    public class OrderItemRequest
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
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
        public string PaymentStatus { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string DeliveryPartnerName { get; set; } = string.Empty;
        public int EstimatedDeliveryTimeInMinutes { get; set; }
        public string NotificationStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
```
