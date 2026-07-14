# External Services and Adapters

Copy these files into your `ExternalServices` and `Adapters` folders. This code forms your Anti-Corruption Layer (ACL).

## External Services (The Messy Legacy APIs)

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

// ExternalServices/ILegacyRestaurantApiClient.cs
namespace FoodOrderingApi.ExternalServices
{
    public interface ILegacyRestaurantApiClient
    {
        LegacyRestaurantResponse? GetRestaurantFromLegacySystem(int restaurantId);
    }
}

// ExternalServices/LegacyRestaurantApiClient.cs
namespace FoodOrderingApi.ExternalServices
{
    public class LegacyRestaurantApiClient : ILegacyRestaurantApiClient
    {
        public LegacyRestaurantResponse? GetRestaurantFromLegacySystem(int restaurantId)
        {
            if (restaurantId == 999) return null;
            if (restaurantId == 500)
            {
                return new LegacyRestaurantResponse
                {
                    Rest_Id = "500", Rest_Nm = "Closed Restaurant",
                    Active_Flag = "Y", Open_Flag = "0", Serving_Now = "NO"
                };
            }
            return new LegacyRestaurantResponse
            {
                Rest_Id = restaurantId.ToString(), Rest_Nm = "Paradise Biryani",
                Active_Flag = "Y", Open_Flag = "1", Serving_Now = "YES"
            };
        }
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

// ExternalServices/ILegacyPaymentGatewayClient.cs
namespace FoodOrderingApi.ExternalServices
{
    public interface ILegacyPaymentGatewayClient
    {
        LegacyPaymentGatewayResponse ProcessPayment(int customerId, decimal amount, string paymentMethod);
    }
}

// ExternalServices/LegacyPaymentGatewayClient.cs
using System;

namespace FoodOrderingApi.ExternalServices
{
    public class LegacyPaymentGatewayClient : ILegacyPaymentGatewayClient
    {
        public LegacyPaymentGatewayResponse ProcessPayment(int customerId, decimal amount, string paymentMethod)
        {
            if (string.Equals(paymentMethod, "Declined", StringComparison.OrdinalIgnoreCase))
            {
                return new LegacyPaymentGatewayResponse
                {
                    txn_ref = string.Empty, resp_cd = "05", resp_msg = "DECLINED", amt = amount.ToString("0.00")
                };
            }
            return new LegacyPaymentGatewayResponse
            {
                txn_ref = $"PG-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
                resp_cd = "00", resp_msg = "APPROVED", amt = amount.ToString("0.00")
            };
        }
    }
}
```

## Adapters (The Translators)

```csharp
// Adapters/ILegacyRestaurantAdapter.cs
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Adapters
{
    public interface ILegacyRestaurantAdapter
    {
        RestaurantInfo? GetRestaurantInfo(int restaurantId);
    }
}

// Adapters/LegacyRestaurantAdapter.cs
using FoodOrderingApi.ExternalServices;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Adapters
{
    public class LegacyRestaurantAdapter : ILegacyRestaurantAdapter
    {
        private readonly ILegacyRestaurantApiClient legacyRestaurantApiClient;

        public LegacyRestaurantAdapter(ILegacyRestaurantApiClient legacyRestaurantApiClient)
        {
            this.legacyRestaurantApiClient = legacyRestaurantApiClient;
        }

        public RestaurantInfo? GetRestaurantInfo(int restaurantId)
        {
            LegacyRestaurantResponse? legacyResponse = legacyRestaurantApiClient.GetRestaurantFromLegacySystem(restaurantId);
            if (legacyResponse is null) return null;

            if (!int.TryParse(legacyResponse.Rest_Id, out int parsedRestaurantId))
                throw new InvalidOperationException("Invalid restaurant id returned by legacy system.");

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

// Adapters/ILegacyPaymentAdapter.cs
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Adapters
{
    public interface ILegacyPaymentAdapter
    {
        PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod);
    }
}

// Adapters/LegacyPaymentAdapter.cs
using FoodOrderingApi.ExternalServices;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Adapters
{
    public class LegacyPaymentAdapter : ILegacyPaymentAdapter
    {
        private readonly ILegacyPaymentGatewayClient legacyPaymentGatewayClient;

        public LegacyPaymentAdapter(ILegacyPaymentGatewayClient legacyPaymentGatewayClient)
        {
            this.legacyPaymentGatewayClient = legacyPaymentGatewayClient;
        }

        public PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod)
        {
            LegacyPaymentGatewayResponse legacyResponse = legacyPaymentGatewayClient.ProcessPayment(customerId, amount, paymentMethod);

            return new PaymentResult
            {
                IsSuccessful = legacyResponse.resp_cd == "00",
                PaymentStatus = legacyResponse.resp_msg,
                TransactionId = legacyResponse.txn_ref
            };
        }
    }
}
```
