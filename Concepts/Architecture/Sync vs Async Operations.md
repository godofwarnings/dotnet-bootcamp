# Synchronous vs Asynchronous Operations

When designing APIs, deciding whether an operation should be performed synchronously (blocking the main thread until it finishes) or asynchronously (offloaded to a background process or event bus) is a critical architectural decision.

## The Rule of Thumb
**Synchronous is for decisions. Asynchronous is for side effects.**

### Synchronous Operations (The Critical Path)
If an operation is required to determine the success or failure of the current request, it **must** be synchronous. The order of these operations is strictly conserved.

*Examples during an Order Placement flow:*
- **Check customer exists:** If they don't, the order fails immediately. (Sync)
- **Check restaurant is open:** If it's closed, the order fails immediately. (Sync)
- **Check item availability:** If out of stock, the order fails immediately. (Sync)
- **Process payment:** If payment declines, the order cannot be placed. (Sync)

### Asynchronous Operations (Side Effects)
If an operation is a reaction to a successful decision, and its failure should not roll back the main transaction, it should be asynchronous.

*Examples during an Order Placement flow:*
- **Send SMS/Email Notification:** If the SMS gateway fails, we don't want to cancel a successful food order that has already been paid for. (Async)
- **Update reporting databases:** Analytical data can be eventually consistent. (Async)
- **Assign Loyalty Points:** The user can receive their points a few seconds or minutes later. (Async)

## Implementation
Asynchronous side-effects are typically implemented using the [Publish-Subscribe (Pub/Sub) Pattern](<Pub-Sub Pattern.md>), where the synchronous critical path publishes an event to a queue, and background workers handle the rest.
