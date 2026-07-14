# Exercise 5 - Sync vs Async Classification

## Requirement
Classify each operation during order placement as synchronous or asynchronous. Explain why each operation is sync or async. Discuss which operations decide the current request and which are side effects.

Operations:
- Check customer exists
- Check restaurant is open
- Check item availability
- Process payment
- Send SMS
- Update reporting
- Update restaurant dashboard
- Generate invoice
- Assign delivery partner

## Expected Direction
- Synchronous: check customer exists, check restaurant is open, check item availability, process payment, assign delivery partner.
- Asynchronous: send SMS, update reporting, update restaurant dashboard, generate invoice.
- Teaching point: Sync is for decisions. Async is for side effects.

## Solution

### Synchronous Operations (The Critical Path)
These operations **must** be executed synchronously because they are decisions that determine whether the order can be placed at all. If any of these fail, the entire transaction should be rolled back and the user should receive an error immediately.

1. **Check customer exists:** Without a valid customer, we cannot create an order.
2. **Check restaurant is open:** If the restaurant is closed, they cannot cook the food.
3. **Check item availability:** If the food is out of stock, we cannot sell it.
4. **Process payment:** If the credit card declines, the order is not confirmed.
5. **Assign delivery partner:** We must guarantee a driver is available before telling the customer their order is confirmed.

### Asynchronous Operations (Side Effects)
These operations are reactions to a successfully placed order. They should be offloaded to an Event Bus (Pub/Sub) so they do not block the HTTP response to the user. If they temporarily fail, they can be retried in the background without affecting the user's order.

1. **Send SMS:** The user doesn't need to wait for the telecom provider to respond to see the "Order Placed" screen.
2. **Update reporting:** The analytical dashboard is eventually consistent and doesn't need real-time synchronous updates.
3. **Update restaurant dashboard:** Same as reporting, can handle a few seconds of delay.
4. **Generate invoice:** PDF generation is slow and CPU intensive. It should happen in the background and be emailed to the user later.
