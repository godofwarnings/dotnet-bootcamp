# Day 7 Summary

**Date:** 7th July 2026

## Overview
Today marked a deep dive into building ASP.NET Core Web APIs. We focused heavily on the mechanics of the framework, especially Dependency Injection and Middlewares, and how Entity Framework Core integrates cleanly into an API architecture.

## Key Learnings

### 1. Web API Basics
- Learned about `PUT` vs `POST` and the core concept of **idempotency**. See [PUT vs POST.md](../../Concepts/Web%20API/PUT%20vs%20POST.md).

### 2. Dependency Injection (DI)
- Explored the Inversion of Control pattern and why it's critical for testing and decoupling.
- Covered the three main lifecycles: **Transient**, **Scoped**, and **Singleton**.
- Implemented Constructor Injection.
- Reference: [Dependency Injection.md](../../Concepts/Web%20API/Dependency%20Injection.md)
- Code Examples: [Day 7 - Dependency Injection](../../Code%20Examples/Day%207%20-%20Web%20API/Dependency%20Injection/)

### 3. EF Core Web API
- Integrated Entity Framework Core into the Web API using DI (`builder.Services.AddDbContext`).
- Created a `ProductService` interacting with the DB and a `ProductsController` exposing the data.
- Code Examples: [Day 7 - EF Core API](../../Code%20Examples/Day%207%20-%20Web%20API/EF%20Core%20API/)

### 4. Middlewares
- Understood the HTTP Request Pipeline, request delegates, and the true meaning of `await next()`.
- Learned how the pipeline executes with a forward pass and unwinds with a backward pass.
- Built a custom `HeaderValidationMiddleware`.
- Reference: [Middlewares.md](../../Concepts/Web%20API/Middlewares.md)
- Code Examples: [Day 7 - Middlewares](../../Code%20Examples/Day%207%20-%20Web%20API/Middlewares/)

## Backlog / Doubts to Track
- **OpenAPI Support:** What does OpenAPI support do? (Added to backlog for further research).
