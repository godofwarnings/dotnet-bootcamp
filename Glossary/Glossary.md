---
tags: [glossary, definitions]
---
# Glossary

This document contains brief definitions of key technical terms encountered during the course.

- **Circuit Breaker Pattern:** A design pattern used to detect failures and encapsulate the logic of preventing a failure from constantly recurring. It has three states: Closed (healthy), Open (failing, requests blocked), and Half-Open (testing recovery).
- **Idempotency:** The property of certain operations where applying them multiple times has the same effect as applying them once. Crucial for `POST` operations like payments where network retries could result in duplicate transactions. Handled using an `Idempotency-Key` header.
- **Options Pattern:** A pattern in ASP.NET Core that uses classes to provide strongly typed access to groups of related settings from configuration sources like `appsettings.json` via `IOptions<T>`.
- **Polly:** A .NET resilience and transient-fault-handling library that allows developers to express policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner.
- **Transient Failure:** A temporary error, such as a momentary loss of network connectivity or a temporary timeout, which is likely to succeed if retried after a short delay.
