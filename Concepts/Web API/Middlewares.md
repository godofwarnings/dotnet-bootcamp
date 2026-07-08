# Middlewares in ASP.NET Core

Middleware in ASP.NET Core is code that sits in the HTTP request pipeline. Each middleware receives the current `HttpContext` plus a reference to the next step in the pipeline. It can either pass the request forward, do some logic, or stop the pipeline early (short-circuiting).

## 1. The Core Concept
Think of middleware like security checkpoints in an airport:
- The **request** is the passenger.
- Each **middleware** is a checkpoint.
- Each checkpoint can:
  - inspect the passenger
  - add information
  - reject the passenger (short-circuit)
  - or let the passenger continue to the next checkpoint

These checkpoints are arranged in order in `Program.cs`.

### Request Delegation
A **request delegate** is the thing ASP.NET Core uses to represent "the next step that can handle this HTTP request." When a middleware calls `await next(context);`, it is delegating the handling of the request to the next middleware in the chain.

## 2. What `await next()` Really Means

Each `app.Use(...)` middleware gets a `next` delegate. Calling `await next()` means:
- "Run the rest of the pipeline."
- "Pause this middleware here until the downstream pipeline finishes."
- "When it finishes, come back here and continue with the code after `await next()`".

**Mental model to keep forever:**
- Code **before** `await next()` = request going down the pipeline.
- Code **after** `await next()` = response coming back up the pipeline.

### The Forward and Backward Pass (The Door Analogy)
Imagine three doors to reach a room: Door A, Door B, Door C, and then the final room (the controller).
- **Going in (Forward pass):** Pass Door A $\rightarrow$ Door B $\rightarrow$ Door C $\rightarrow$ enter room.
- **Coming out (Backward pass):** Exit through Door C $\rightarrow$ Door B $\rightarrow$ Door A.

This is why, if you write code before and after `await next()`, the "Before" logs execute sequentially, then the controller hits, and the "After" logs unwind backwards.

## 3. Writing Custom Middlewares

A middleware class must include:
- A public constructor with a parameter of type `RequestDelegate` (`next`).
- A public method named `Invoke` or `InvokeAsync` that returns `Task` and accepts `HttpContext` as the first parameter.

See [HeaderValidationMiddleware.cs](../../Code%20Examples/Day%207%20-%20Web%20API/Middlewares/HeaderValidationMiddleware.cs) for a complete example.
It validates if an `X-Client-Name` header is present. If it's missing, it sets the status code to `400 Bad Request`, writes a response, and `return`s immediately (short-circuiting). If it's present, it calls `await next(context);`.

### Registration
It's a best practice to register your custom middleware using an extension method on `IApplicationBuilder`.
See [HeaderValidationMiddlewareExtensions.cs](../../Code%20Examples/Day%207%20-%20Web%20API/Middlewares/HeaderValidationMiddlewareExtensions.cs) and [Middleware_Program.cs](../../Code%20Examples/Day%207%20-%20Web%20API/Middlewares/Middleware_Program.cs) for exactly how this is done!
