# Day 12 Summary

**Date:** 14th July 2026
**Topic:** Clean Architecture, Service Boundaries, and Pub/Sub

## Overview
Today focused on refactoring a monolithic "God Class" into clean, separated components. We learned how to identify code smells where a single service does too much (validation, external calls, db operations), and how to break it down using domain services, Anti-Corruption Layers (ACL), and the Publish-Subscribe pattern.

## Topics Covered

1. **Service Boundaries:**
   - Refactoring is not just moving lines; it is separating ownership.
   - Converted a massive `OrderService` into a clean **Orchestrator** that delegates specific rules to `CustomerService`, `RestaurantService`, `MenuService`, `PaymentService`, and `DeliveryService`.
   - [Service Boundaries](../../Concepts/Architecture/Service%20Boundaries.md)

2. **Sync vs Async Operations:**
   - Synchronous operations are for **decisions** (e.g., checking if a restaurant is open or processing payment).
   - Asynchronous operations are for **side effects** (e.g., sending an SMS or updating a dashboard).
   - [Sync vs Async Operations](../../Concepts/Architecture/Sync%20vs%20Async%20Operations.md)

3. **Publish-Subscribe (Pub/Sub) Pattern:**
   - Decoupled asynchronous side-effects from the main transaction flow.
   - Created an `InMemoryEventBus` and published an `OrderPlacedEvent`.
   - Handlers (Subscribers) react to the event without the publisher knowing about them.
   - [Pub-Sub Pattern](../../Concepts/Architecture/Pub-Sub%20Pattern.md)

4. **Anti-Corruption Layer (ACL):**
   - Shielded our core domain from messy, legacy external systems.
   - Created `LegacyPaymentAdapter` and `LegacyRestaurantAdapter` to translate ugly external JSON/DTOs into clean internal domain models.
   - [Anti-Corruption Layer (ACL)](../../Concepts/Architecture/Anti-Corruption%20Layer%20%28ACL%29.md)

## Code Examples
- [Original Code (Monolithic)](../../Code%20Examples/Day%2012%20-%20Food%20Ordering%20Architecture/01%20-%20Original%20Code%20%28Monolithic%29.md)
- [Service Boundaries Refactoring](../../Code%20Examples/Day%2012%20-%20Food%20Ordering%20Architecture/02%20-%20Service%20Boundaries%20Refactoring.md)
- [Anti-Corruption Layer Implementation](../../Code%20Examples/Day%2012%20-%20Food%20Ordering%20Architecture/03%20-%20Anti-Corruption%20Layer%20Implementation.md)
- [Pub-Sub Implementation](../../Code%20Examples/Day%2012%20-%20Food%20Ordering%20Architecture/04%20-%20Pub-Sub%20Implementation.md)
- [Final Runnable Codebase](../../Code%20Examples/Day%2012%20-%20Food%20Ordering%20Architecture/05%20-%20Final%20Runnable%20Codebase/)

## Exercises Completed
- [Exercise 1 & 2 - Refactoring to Service Boundaries](../../Code%20Examples/Day%2012%20-%20Food%20Ordering%20Architecture/Exercises/Exercise%201%20&%202%20-%20Refactoring%20to%20Service%20Boundaries.md)
- [Exercise 3 - Loyalty Points PubSub Subscriber](../../Code%20Examples/Day%2012%20-%20Food%20Ordering%20Architecture/Exercises/Exercise%203%20-%20Loyalty%20Points%20PubSub%20Subscriber.md)
- [Exercise 4 - Payment Gateway ACL](../../Code%20Examples/Day%2012%20-%20Food%20Ordering%20Architecture/Exercises/Exercise%204%20-%20Payment%20Gateway%20ACL.md)
- [Exercise 5 - Sync vs Async Classification](../../Code%20Examples/Day%2012%20-%20Food%20Ordering%20Architecture/Exercises/Exercise%205%20-%20Sync%20vs%20Async%20Classification.md)
