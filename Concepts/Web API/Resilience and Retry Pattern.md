# Resilience and Retry Pattern

When building distributed systems or microservices, network calls can fail temporarily (transient errors). A Retry Pattern allows the application to handle these transient failures gracefully by reattempting the operation.

## Key Principles

1. **Identify Transient Failures**: Only retry on specific, temporary exceptions (e.g., `TimeoutException`, HTTP 503, HTTP 429). Do NOT retry on permanent failures (e.g., `ArgumentException`, HTTP 404, HTTP 400).
2. **Maximum Retries**: Always set a hard limit (e.g., 3 retries) so the system doesn't hang infinitely.
3. **Delay/Backoff**: Introduce a delay (e.g., `Thread.Sleep` or `Task.Delay`) between retries so you don't bombard an already struggling service.

## Anti-Pattern: Retrying in Global Middleware
Do not implement a global retry loop inside an ASP.NET Core middleware. Doing so means **all** requests to your API are subjected to the retry logic, which can mask errors and unnecessarily delay responses. 

Instead, implement retry logic at the **boundary** where you call the specific downstream service (e.g., inside an `IFeeService` wrapper).

## The Polly Library
In .NET, **Polly** is the industry standard library for resilience. It provides fluent policies for:
- Retry (with exponential backoff)
- Circuit Breaker
- Timeout
- Bulkhead Isolation
- Fallback

Instead of writing manual `while` loops, Polly allows you to define policies declaratively. See the Knowledge Backlog for a deeper dive into Polly.
