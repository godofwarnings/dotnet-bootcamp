# Exercise 4 - Anti-Corruption Layer for External Payment Gateway

## Requirement
A legacy payment gateway returns ugly external data. Do not allow this response shape to enter `OrderService`.

Legacy response:
```json
{
  "txn_ref": "PG-99881",
  "resp_cd": "00",
  "resp_msg": "APPROVED",
  "amt": "620.00"
}
```

Clean internal model:
```csharp
public class PaymentResult
{
    public bool IsSuccessful { get; set; }
    public string PaymentStatus { get; set; } = "";
    public string TransactionId { get; set; } = "";
}
```

## Student Tasks
1. Create `LegacyPaymentGatewayResponse`.
2. Create `LegacyPaymentGatewayClient`.
3. Create `ILegacyPaymentAdapter`.
4. Create `LegacyPaymentAdapter`.
5. Map the legacy response to clean `PaymentResult`.
6. Use the adapter from `PaymentService`.

## Expected Direction
- `txn_ref` maps to `TransactionId`.
- `resp_cd == "00"` maps to `IsSuccessful = true`.
- `resp_msg` maps to `PaymentStatus`.
- `OrderService` should only use clean `PaymentResult`, not legacy fields.

## Solution

### 1. External System (Ugly Models and Client)

```csharp
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

// ExternalServices/LegacyPaymentGatewayClient.cs
namespace FoodOrderingApi.ExternalServices
{
    public interface ILegacyPaymentGatewayClient
    {
        LegacyPaymentGatewayResponse ProcessPayment(int customerId, decimal amount, string paymentMethod);
    }

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

### 2. The ACL (The Adapter)

```csharp
// Adapters/LegacyPaymentAdapter.cs
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
        private readonly ILegacyPaymentGatewayClient client;

        public LegacyPaymentAdapter(ILegacyPaymentGatewayClient client)
        {
            this.client = client;
        }

        public PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod)
        {
            // Call ugly system
            var legacyResponse = client.ProcessPayment(customerId, amount, paymentMethod);

            // Translate to clean system
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

### 3. The Core Domain Service (Protected)

```csharp
// Services/PaymentService.cs
using FoodOrderingApi.Adapters;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public interface IPaymentService
    {
        PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod);
    }

    public class PaymentService : IPaymentService
    {
        private readonly ILegacyPaymentAdapter legacyPaymentAdapter;

        public PaymentService(ILegacyPaymentAdapter legacyPaymentAdapter)
        {
            this.legacyPaymentAdapter = legacyPaymentAdapter;
        }

        public PaymentResult ProcessPayment(int customerId, decimal amount, string paymentMethod)
        {
            return legacyPaymentAdapter.ProcessPayment(customerId, amount, paymentMethod);
        }
    }
}
```
