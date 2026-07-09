---
tags: [glossary, definitions]
---
# Glossary

This document contains brief definitions of key technical terms encountered during the course.

- **Circuit Breaker Pattern:** A design pattern used to detect failures and encapsulate the logic of preventing a failure from constantly recurring. It has three states: Closed (healthy), Open (failing, requests blocked), and Half-Open (testing recovery).
- **DTO (Data Transfer Object):** A simple object used to transfer data into or out of the application, keeping the API contract separate from the internal domain model.
- **Idempotency:** The property of certain operations where applying them multiple times has the same effect as applying them once. Crucial for `POST` operations like payments where network retries could result in duplicate transactions. Handled using an `Idempotency-Key` header.
- **INT:** Integration Environment. The first shared environment where different developers' code is merged and tested together.
- **JWT (JSON Web Token):** An open standard used to share security information between two parties — a client and a server. Each JWT contains encoded JSON objects, including a set of claims.
- **Options Pattern:** A pattern in ASP.NET Core that uses classes to provide strongly typed access to groups of related settings from configuration sources like `appsettings.json` via `IOptions<T>`.
- **Polly:** A .NET resilience and transient-fault-handling library that allows developers to express policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner.
- **PPE:** Pre-Production Environment. A staging environment that mirrors production as closely as possible for final testing before go-live.
- **SDLC (Software Development Life Cycle):** A process used by the software industry to design, develop, and test high-quality software.
- **Transient Failure:** A temporary error, such as a momentary loss of network connectivity or a temporary timeout, which is likely to succeed if retried after a short delay.
- **UAT:** User Acceptance Testing Environment. The environment where end-users or clients test the software to ensure it meets their business requirements.
