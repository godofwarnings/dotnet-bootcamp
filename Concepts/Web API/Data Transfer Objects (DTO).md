---
tags: [api, architecture, dto]
---
# Data Transfer Objects (DTO)

Data Transfer Objects (DTOs) are objects that carry data between processes. In ASP.NET Core Web APIs, they define the exact shape of the data that the API expects to receive (Request DTOs) and the exact shape it promises to return (Response DTOs).

## The "Company Office" Analogy

Think of a layered application like a company office:
- **UI** = the front desk form and product table
- **Controller** = the receptionist receiving requests
- **Service** = the manager applying business logic
- **Store / Persistence** = the storage room holding products
- **Middleware** = the hallway security / logging / error guard
- **DTOs** = the paper forms used to communicate with the outside world (e.g. a "Customer Registration Form")

The customer form (DTO) and the internal warehouse record (Model) may contain similar data, but they are **not the same thing**.

Why?
- The customer should only fill fields they are allowed to fill.
- The warehouse may track internal things the customer should never control (like internal status flags, timestamps, or system IDs).

## The Shortest Possible Takeaway

Remember these three rules for the API boundary:
1. Controller receives **DTOs**.
2. Service maps DTOs to the **Model**.
3. Service maps the **Model** back to Response **DTOs**.

```text
DTOs are for the API boundary.
Models are for the inside of the app (domain logic and persistence).
```
