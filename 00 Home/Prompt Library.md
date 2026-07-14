# Prompt Library

This document stores high-quality, reusable AI prompts that have proven effective during development and refactoring.

## Refactoring to Service Boundaries

When presented with a "God Class" or monolithic service, use this prompt to instruct the AI to refactor it into clean service boundaries.

```text
Take the following code and implement Service Boundaries:

- **CustomerService** → checks customer details
- **RestaurantService** → checks restaurant status
- **MenuService** → checks menu items and prices
- **OrderService** → creates the order
- **PaymentService** → handles payment
- **NotificationService** → sends SMS/email/push notification
- **DeliveryService** → assigns delivery partner

Also explain how one would plan and do this.

[Paste Code Here]
```

## Context Loading

When providing a large codebase or complex reference document to the AI, use this prompt to ensure it understands the context before attempting to answer questions or write code.

```text
Take this reference document and understand it. We will build upon it.

[Paste Reference Document / Codebase Here]
```

## Filtering Out Theory (Action-Oriented)

When the AI provides too much explanation and you just want the code, use this prompt.

```text
I don't care about the theory part. Give me the coding parts only. Let's do the rest of it.
```
