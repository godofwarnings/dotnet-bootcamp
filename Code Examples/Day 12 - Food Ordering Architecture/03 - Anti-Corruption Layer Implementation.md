# Anti-Corruption Layer (ACL) Implementation

This example demonstrates how an Anti-Corruption Layer shields your core domain (like `RestaurantService` and `PaymentService`) from the messy, confusing structure of external legacy systems.

## 1. External System Models (Ugly)

These classes perfectly model the JSON/XML returned by the external APIs. Notice the cryptic naming conventions (`resp_cd`, `txn_ref`, `Active_Flag`). These should never leak into your core logic.

```csharp
// ExternalServices/LegacyRestaurantResponse.cs
namespace FoodOrderingApi.ExternalServices
{
    public class LegacyRestaurantResponse
    {
        public string Rest_Id { get; set; } = string.Empty;
        public string Rest_Nm { get; set; } = string.Empty;
        public string Active_Flag { get; set; } = string.Empty;
        public string Open_Flag { get; set; } = string.Empty;
        public string Serving_Now { get; set; } = string.Empty;
    }
}

// ExternalServices/LegacyPaymentGatewayResponse.cs
namespace FoodOrderingApi.ExternalServices
{
    public class LegacyPaymentGatewayResponse
    {
        public string txn_ref { get; set; } = string.Empty;
        public string resp_cd { get; set; } = string.Empty;
        public string resp_msg { get; set; } = string.Empty;
        public string amt { get; set; } = string.Empty;
    }
}
```

## 2. Internal Domain Models (Clean)

These are the models your application *wants* to use. They use standard naming conventions and correct types (like `bool` instead of `"Y"`).

```csharp
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
```

## 3. The Adapters (The ACL)

The adapters are responsible for calling the external clients, receiving the ugly responses, and translating them into the clean models.

### Legacy Restaurant Adapter

```csharp
using FoodOrderingApi.ExternalServices;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Adapters
{
    public interface ILegacyRestaurantAdapter
    {
        RestaurantInfo? GetRestaurantInfo(int restaurantId);
    }

    public class LegacyRestaurantAdapter : ILegacyRestaurantAdapter
    {
        private readonly ILegacyRestaurantApiClient legacyRestaurantApiClient;

        public LegacyRestaurantAdapter(ILegacyRestaurantApiClient legacyRestaurantApiClient)
        {
            this.legacyRestaurantApiClient = legacyRestaurantApiClient;
        }

        public RestaurantInfo? GetRestaurantInfo(int restaurantId)
        {
            LegacyRestaurantResponse? legacyResponse =
                legacyRestaurantApiClient.GetRestaurantFromLegacySystem(restaurantId);

            if (legacyResponse is null)
            {
                return null;
            }

            if (!int.TryParse(legacyResponse.Rest_Id, out int parsedRestaurantId))
            {
                throw new InvalidOperationException("Invalid restaurant id returned by legacy system.");
            }

            // Translation happens here
            return new RestaurantInfo
            {
                RestaurantId = parsedRestaurantId,
                RestaurantName = legacyResponse.Rest_Nm,
                IsActive = string.Equals(legacyResponse.Active_Flag, "Y", StringComparison.OrdinalIgnoreCase),
                IsOpen = string.Equals(legacyResponse.Open_Flag, "1", StringComparison.OrdinalIgnoreCase),
                IsServingNow = string.Equals(legacyResponse.Serving_Now, "YES", StringComparison.OrdinalIgnoreCase)
            };
        }
    }
}
```

### Legacy Payment Adapter

```csharp
using FoodOrderingApi.ExternalServices;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Adapters
{
    public interface ILegacyPaymentAdapter
    {
        PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod);
    }

    public class LegacyPaymentAdapter : ILegacyPaymentAdapter
    {
        private readonly ILegacyPaymentGatewayClient legacyPaymentGatewayClient;

        public LegacyPaymentAdapter(ILegacyPaymentGatewayClient legacyPaymentGatewayClient)
        {
            this.legacyPaymentGatewayClient = legacyPaymentGatewayClient;
        }

        public PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod)
        {
            LegacyPaymentGatewayResponse legacyResponse =
                legacyPaymentGatewayClient.ProcessPayment(customerId, amount, paymentMethod);

            // Translation happens here
            bool isSuccessful = legacyResponse.resp_cd == "00";

            return new PaymentResult
            {
                IsSuccessful = isSuccessful,
                PaymentStatus = legacyResponse.resp_msg,
                TransactionId = legacyResponse.txn_ref
            };
        }
    }
}
```

## 4. The Core Service (Protected)

The core `PaymentService` no longer knows anything about `txn_ref` or `resp_cd`. It only deals with the clean `ILegacyPaymentAdapter`.

```csharp
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

        public PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod)
        {
            // Business validations
            if (customerId <= 0) throw new ArgumentException("Invalid customer id.");
            if (amount <= 0) throw new ArgumentException("Payment amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(paymentMethod)) throw new ArgumentException("Payment method is required.");

            // Call the ACL, receive clean data
            return legacyPaymentAdapter.ProcessPayment(customerId, amount, paymentMethod);
        }
    }
}
```
