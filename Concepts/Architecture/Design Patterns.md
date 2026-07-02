---
tags: [architecture, patterns, singleton, repository, pubsub]
aliases: [Singleton, Repository Pattern, Publisher-Subscriber]
---
# Design Patterns

Design patterns are typical solutions to commonly occurring problems in software design. They are blueprints you can customize.

## 1. Singleton Pattern
Ensures a class has **only one instance** and provides a global point of access to it.
- **Why?** To prevent inefficient object creation (e.g., if you create a `Date` object in 50 different files, it consumes unnecessary memory).
- **Use case:** Utility classes, logging, configuration managers.

## 2. Repository Pattern
Mediates between the domain and data mapping layers, acting like an in-memory collection of domain objects.
- **Why?** It prevents the "Wrong Way" (where 500 application classes connect directly to MSSQL). This direct access creates a bottleneck and messy code.
- **The Solution (Right Way):** A "Repository Layer" abstracts the database. Application classes call the repository (which has `Insert`, `Update`, `Select`, `Delete` methods), and the repository talks to the database (whether it's MSSQL, MongoDB, etc.). This makes the system easier to maintain and switch data providers.
- **Example:** See [StudentRepository.cs](../../Code%20Examples/Day%203%20-%20Dapper/StudentRepository.cs) for a concrete implementation.

## 3. Publisher-Subscriber (Pub/Sub) Pattern
A messaging pattern where senders (publishers) do not program the messages to be sent directly to specific receivers (subscribers). Instead, messages are published to classes, without knowledge of which subscribers there may be.
- **Examples in daily life:** YouTube subscriptions, Twitter, ATM SMS notifications.
