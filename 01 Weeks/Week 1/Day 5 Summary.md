# Day 5 Summary

**Date:** 3rd July 2026
**Instructor:** Instructor

## Topics Covered
- [Authentication vs Authorization](../../Concepts/Architecture/Authentication%20vs%20Authorization.md)
- [Caching and CDN](../../Concepts/Architecture/Caching%20and%20CDN.md)
- [System Design - Hospital](../../Concepts/Architecture/System%20Design%20-%20Hospital.md)
- [Race Conditions and Locking](../../Concepts/Database/Race%20Conditions%20and%20Locking.md)
- [Database Design Best Practices](../../Concepts/Database/Database%20Design%20Best%20Practices.md)
- [HTTP APIs](../../Concepts/Web%20Development/HTTP%20APIs.md)

## Key Takeaways
- **Authentication** identifies users, while **Authorization** verifies their permissions.
- **System Design** involves breaking down users, use cases, and workflows, then mapping them to architectural layers (Search Services, Availability Services, API Gateways, Message Queues).
- To maintain high performance, generate complex states dynamically and heavily cache read-queries (like a list of doctors) via Redis, while pushing computationally heavy notifications to async Message Queues.
- **Race conditions** like double-booking occur when concurrent requests read a state before either commits. We solve this not in the UI, but by wrapping inserts in transactions and using database mechanisms like **unique constraints** and **row-level locking** (`SELECT FOR UPDATE`).
- Databases should use Enums for status integrity and flag columns (`IsDeleted`) for soft-deletes to maintain full audit traceability.
- **HTTP APIs** map standard client operations (GET, POST, PUT, DELETE) to Application logic. Responses use **JSON** for standard parsing and standard HTTP status codes for programmatic interoperability.
