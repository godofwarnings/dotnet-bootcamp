# Glossary

This document contains definitions for key architectural and technical concepts encountered during the bootcamp.

## General Architecture

**Anti-Corruption Layer (ACL)**: A protective layer (usually adapters/translators) that sits between your core application and an external, legacy, or third-party system. It ensures that the messy, confusing terminology or data shapes of the external system do not leak into and "corrupt" your internal domain logic.

**Orchestrator**: A high-level service or class (e.g., `OrderService`) whose primary job is to coordinate the execution flow by delegating tasks to other specialized domain services. An orchestrator should contain minimal direct business logic (like price calculations).

**Service Boundaries**: The architectural principle of dividing a large system into smaller, specialized domain services where each service has distinct ownership over a specific piece of business logic (e.g., `PaymentService` owns payments, `MenuService` owns pricing).

## Design Patterns

**Publish-Subscribe (Pub/Sub)**: A messaging pattern where senders (publishers) do not send messages directly to specific receivers (subscribers). Instead, events are pushed to an Event Bus, and any number of subscribers can listen and react asynchronously. Used to decouple side-effects from the main synchronous transaction.

## Execution Flow

**Synchronous Operations**: Operations that block the current execution thread until they complete. In API design, these are typically used for **decisions** that are strictly required for the current transaction to succeed (e.g., checking inventory).

**Asynchronous Operations**: Operations that run in the background (often via an Event Bus) and do not block the main execution thread. In API design, these are typically used for **side effects** that do not need to be completed instantly (e.g., sending a notification).

## ASP.NET Core Concepts

**AddScoped**: A Dependency Injection lifetime where an object is created once per client request (HTTP request). *Gotcha*: When using tools like Postman, every time you press "Send", it counts as a brand new request, so scoped services (like in-memory idempotency caches) will be destroyed and recreated.

**AddSingleton**: A Dependency Injection lifetime where an object is created only once for the entire lifetime of the application. It persists across multiple HTTP requests.

**wwwroot**: The default directory in an ASP.NET Core application used to serve static files like HTML, CSS, and JavaScript. Enabled by calling `app.UseStaticFiles()`.
