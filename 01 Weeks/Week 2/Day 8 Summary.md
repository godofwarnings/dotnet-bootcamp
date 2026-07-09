# Day 8 Summary (Monday, 6th July, 2026)

## Topics Covered
Today was a massive deep-dive into advanced API Resilience, Middleware Lifetimes, and Idempotency.

1. **Middleware Lifetimes (Anti-Pattern):**
   - We learned that injecting a **Scoped** service into the constructor of a conventional **Middleware** causes a lifetime mismatch, forcing the scoped service to behave like a singleton. [Middlewares](../../Concepts/Web%20API/Middlewares.md)
   - The fix is to inject scoped dependencies directly into the `InvokeAsync` method. [Lifecycle Demo](../../Code%20Examples/Day%208%20-%20AI%20Assisted%20API/Lifecycle%20Demo.md)

2. **Resilience & Retry Patterns:**
   - Instead of immediately failing on a `TimeoutException`, we implemented a retry loop inside a `ResilientFeeService` wrapper. [Resilience and Retry Pattern](../../Concepts/Web%20API/Resilience%20and%20Retry%20Pattern.md), [Retry Pattern (Resilient Wrapper)](../../Code%20Examples/Day%208%20-%20AI%20Assisted%20API/Retry%20Pattern%20(Resilient%20Wrapper).md)
   - We learned that retries should **only** apply to transient failures (like Timeouts), never to hard business rule failures (like `ArgumentException`). [Retry Pattern (Basic)](../../Code%20Examples/Day%208%20-%20AI%20Assisted%20API/Retry%20Pattern%20(Basic).md)

3. **Circuit Breaker Pattern:**
   - Discussed the three states of a Circuit Breaker (Closed, Open, Half-Open). [Circuit Breaker Pattern](../../Concepts/Architecture/Circuit%20Breaker%20Pattern.md)
   - We built an in-memory implementation of a circuit breaker to prevent overwhelming a failing downstream fee service.

4. **Idempotency:**
   - Implemented an `Idempotency-Key` mechanism for a `POST /api/certificates/generate` endpoint. [Idempotency](../../Concepts/Web%20API/Idempotency.md)
   - By using a Singleton service with a `Dictionary`, we guaranteed that if a client retries the exact same request due to a network drop, they get the exact same certificate number rather than generating a duplicate. [Idempotency Code](../../Code%20Examples/Day%208%20-%20AI%20Assisted%20API/Idempotency%20Code.md)

5. **Options Pattern:**
   - Refactored hard-coded retry limits (e.g., `MaxRetryAttempts = 3`) into strongly-typed configurations loaded from `appsettings.json` using `IOptions<RetrySettings>`. [Options Pattern](../../Concepts/Web%20API/Options%20Pattern.md), [Options Pattern Setup](../../Code%20Examples/Day%208%20-%20AI%20Assisted%20API/Options%20Pattern%20Setup.md)

6. **Enroll Student Feature:**
   - Implemented a standard many-to-many relationship using a Model + DTO approach. [Enroll Student in Course](../../Code%20Examples/Day%208%20-%20AI%20Assisted%20API/Enroll%20Student%20in%20Course.md)

## Next Steps
- Implement Polly to replace the manual `while` loops for resilience.
- Explore API Versioning.
