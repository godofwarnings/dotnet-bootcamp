# Day 8 Summary (Monday, 6th July, 2026)

## Topics Covered
Today was a massive deep-dive into advanced API Resilience, Middleware Lifetimes, and Idempotency.

1. **Middleware Lifetimes (Anti-Pattern):**
   - We learned that injecting a **Scoped** service into the constructor of a conventional **Middleware** causes a lifetime mismatch, forcing the scoped service to behave like a singleton.
   - The fix is to inject scoped dependencies directly into the `InvokeAsync` method.

2. **Resilience & Retry Patterns:**
   - Instead of immediately failing on a `TimeoutException`, we implemented a retry loop inside a `ResilientFeeService` wrapper.
   - We learned that retries should **only** apply to transient failures (like Timeouts), never to hard business rule failures (like `ArgumentException`).

3. **Circuit Breaker Pattern:**
   - Discussed the three states of a Circuit Breaker (Closed, Open, Half-Open).
   - We built an in-memory implementation of a circuit breaker to prevent overwhelming a failing downstream fee service.

4. **Idempotency:**
   - Implemented an `Idempotency-Key` mechanism for a `POST /api/certificates/generate` endpoint.
   - By using a Singleton service with a `Dictionary`, we guaranteed that if a client retries the exact same request due to a network drop, they get the exact same certificate number rather than generating a duplicate.

5. **Options Pattern:**
   - Refactored hard-coded retry limits (e.g., `MaxRetryAttempts = 3`) into strongly-typed configurations loaded from `appsettings.json` using `IOptions<RetrySettings>`.

6. **Enroll Student Feature:**
   - Implemented a standard many-to-many relationship using a Model + DTO approach.

## Next Steps
- Implement Polly to replace the manual `while` loops for resilience.
- Explore API Versioning.
