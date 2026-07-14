# Anti-Corruption Layer (ACL)

When modernizing or integrating with legacy or external systems, those systems often have data models, conventions, or communication protocols that are messy, confusing, or do not align with your domain model.

An **Anti-Corruption Layer (ACL)** is an architectural pattern that sits between your clean domain and the external system. Its job is to translate and adapt data back and forth, ensuring that the ugly, confusing language of the external system does not "corrupt" your internal application logic.

## The Problem
Imagine an external legacy payment gateway returns a response like this:
```json
{
  "txn_ref": "PG-99881",
  "resp_cd": "00",
  "resp_msg": "APPROVED",
  "amt": "620.00"
}
```
If you allow this JSON structure to leak into your `OrderService`, your business logic becomes tightly coupled to the legacy system's cryptic field names (`resp_cd == "00"`).

## The Solution
Create an adapter that acts as the ACL.

1. **External DTO:** Define a class that matches the ugly external system (`LegacyPaymentGatewayResponse`).
2. **Clean Internal Model:** Define a class that makes sense for your domain (`PaymentResult` with `IsSuccessful`, `TransactionId`).
3. **Adapter:** Create a service (`LegacyPaymentAdapter`) that calls the external system, receives the ugly DTO, and translates it into the clean internal model.

Your core services (like `OrderService`) now only depend on the clean `PaymentResult` model and the `ILegacyPaymentAdapter` interface. They are completely shielded from the legacy system's implementation details.

### Reference
[DDD Practitioners - Anti-Corruption Layer](https://ddd-practitioners.com/home/glossary/bounded-context/bounded-context-relationship/anticorruption-layer/)
