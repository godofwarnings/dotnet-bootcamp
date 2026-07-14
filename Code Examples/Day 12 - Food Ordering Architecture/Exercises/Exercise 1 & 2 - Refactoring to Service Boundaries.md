# Exercise 1 & 2 - Refactoring to Service Boundaries

## Requirement
You are given a messy `CancelOrderService` that does too much. Refactor it into clear service boundaries.

## Student Tasks
1. Read the messy service.
2. Highlight every responsibility.
3. Group similar responsibilities.
4. Create separate services.
5. Keep `CancelOrderService` as the orchestrator.

## Solution

### The Messy Original Service
```csharp
using FoodOrderingApi.DTOs;
using FoodOrderingApi.Models;

namespace FoodOrderingApi.Services
{
    public class CancelOrderService : ICancelOrderService
    {
        private static readonly List<ExistingOrder> orders = new List<ExistingOrder>
        {
            new ExistingOrder { OrderId = 9001, CustomerId = 101, TotalAmount = 620, Status = "OrderPlaced", OrderedAt = DateTime.UtcNow.AddMinutes(-10) },
            // ... (other orders)
        };

        public CancelOrderResponse CancelOrder(int orderId, CancelOrderRequest request)
        {
            // 1. Validation
            if (orderId <= 0) throw new ArgumentException("Invalid order id.");
            if (request.CustomerId <= 0) throw new ArgumentException("Invalid customer id.");
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("Cancellation reason is required.");

            // 2. Data Access (Order Lookup)
            ExistingOrder? order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null) throw new InvalidOperationException("Order not found.");

            // 3. Business Rules (Cancellation Policy)
            if (order.CustomerId != request.CustomerId) throw new UnauthorizedAccessException("Customer is not allowed to cancel this order.");
            if (order.Status == "Cancelled") throw new ApplicationException("Order is already cancelled.");
            if (order.Status == "Delivered") throw new ApplicationException("Delivered orders cannot be cancelled.");
            if (order.Status == "OutForDelivery") throw new ApplicationException("Order cannot be cancelled after it is out for delivery.");
            if ((DateTime.UtcNow - order.OrderedAt).TotalMinutes > 30) throw new ApplicationException("Order cannot be cancelled after 30 minutes.");

            // 4. Refund Logic
            decimal refundAmount = 0;
            if (order.Status == "OrderPlaced") refundAmount = order.TotalAmount;
            else if (order.Status == "FoodPreparing") refundAmount = order.TotalAmount * 0.50m;

            string refundStatus = "NoRefund";
            if (refundAmount > 0)
            {
                refundStatus = refundAmount > 1000 ? "RefundRequiresManualApproval" : "RefundProcessed";
            }

            // 5. State Mutation
            string previousStatus = order.Status;
            order.Status = "Cancelled";

            // 6. Side Effects
            string notificationStatus = "NotificationSent";
            Console.WriteLine($"Cancellation notification sent to CustomerId: {request.CustomerId} for OrderId: {orderId}");

            string reportingStatus = "CancellationReportUpdated";
            Console.WriteLine($"Cancellation report updated for OrderId: {orderId}");

            Console.WriteLine($"Audit log created. OrderId: {orderId}, Reason: {request.Reason}");

            return new CancelOrderResponse
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                PreviousStatus = previousStatus,
                CurrentStatus = order.Status,
                RefundAmount = refundAmount,
                RefundStatus = refundStatus,
                NotificationStatus = notificationStatus,
                ReportingStatus = reportingStatus,
                CancelledAt = DateTime.UtcNow
            };
        }
    }
}
```

### The Refactored Solution (Orchestrator + Domain Services)

By separating ownership, the orchestrator delegates specific rules to specialized services. 
Below is the thin Orchestrator. The full separated structure includes `IOrderLookupService`, `ICancellationPolicyService`, `IRefundService`, `ICancellationNotificationService`, `ICancellationReportingService`, and `IAuditService`.

```csharp
using System;
using FoodOrderingApi.DTOs;
using FoodOrderingApi.Models;
using FoodOrderingApi.Repositories;

namespace FoodOrderingApi.Services
{
    public class CancelOrderService : ICancelOrderService
    {
        private readonly IOrderLookupService orderLookupService;
        private readonly ICancellationPolicyService cancellationPolicyService;
        private readonly IRefundService refundService;
        private readonly ICancellationNotificationService cancellationNotificationService;
        private readonly ICancellationReportingService cancellationReportingService;
        private readonly IAuditService auditService;
        private readonly IOrderRepository orderRepository;

        public CancelOrderService(
            IOrderLookupService orderLookupService,
            ICancellationPolicyService cancellationPolicyService,
            IRefundService refundService,
            ICancellationNotificationService cancellationNotificationService,
            ICancellationReportingService cancellationReportingService,
            IAuditService auditService,
            IOrderRepository orderRepository)
        {
            this.orderLookupService = orderLookupService;
            this.cancellationPolicyService = cancellationPolicyService;
            this.refundService = refundService;
            this.cancellationNotificationService = cancellationNotificationService;
            this.cancellationReportingService = cancellationReportingService;
            this.auditService = auditService;
            this.orderRepository = orderRepository;
        }

        public CancelOrderResponse CancelOrder(int orderId, CancelOrderRequest request)
        {
            // 1. Validation
            if (orderId <= 0) throw new ArgumentException("Invalid order id.");
            if (request is null) throw new ArgumentException("Request body is required.");
            if (request.CustomerId <= 0) throw new ArgumentException("Invalid customer id.");
            if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("Cancellation reason is required.");

            // 2. Order Lookup
            ExistingOrder? order = orderLookupService.GetOrderById(orderId);
            if (order is null) throw new InvalidOperationException("Order not found.");

            // 3. Enforcement Policy
            cancellationPolicyService.EnsureOrderCanBeCancelled(order, request.CustomerId);

            // 4. Refund Rules
            RefundDecision refundDecision = refundService.CalculateRefund(order);

            // 5. Update State
            string previousStatus = order.Status;
            DateTime cancelledAt = DateTime.UtcNow;

            order.Status = OrderStatuses.Cancelled;
            order.CancelledAt = cancelledAt;
            orderRepository.Update(order);

            // 6. Side Effects
            string notificationStatus = cancellationNotificationService.SendCancellationNotification(request.CustomerId, orderId);
            string reportingStatus = cancellationReportingService.UpdateCancellationReport(orderId);
            auditService.WriteCancellationAudit(orderId, request.CustomerId, request.Reason, previousStatus, order.Status);

            // 7. Response Building
            return new CancelOrderResponse
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                PreviousStatus = previousStatus,
                Status = order.Status,
                RefundAmount = refundDecision.RefundAmount,
                RefundStatus = refundDecision.RefundStatus,
                NotificationStatus = notificationStatus,
                ReportingStatus = reportingStatus,
                CancelledAt = cancelledAt
            };
        }
    }
}
```
