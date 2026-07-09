---
tags: [backlog, tracking]
---
# Knowledge Backlog

This document tracks topics that have been introduced but require deeper study, or interesting side-topics that came up during lectures.

## High Priority
- **Layered Code Distribution:** How does one arrive at the layered code distribution? What is the step-by-step thought process when refactoring a monolith into layers?
- [x] **Data Transfer Objects (DTO):** What exactly is a DTO in simple terms and why do we need it alongside normal Models?
- **ADO.NET vs Dapper vs EF Core:** When exactly should you use each one in a real-world enterprise project?
- **EF Core Migrations Up/Down:** How exactly do the Up and Down methods work, and how do we write custom migrations?
- **EF Core Relationships:** How do 1:1, 1:Many, and Many:Many relations map to actual foreign keys and join tables under the hood?
- **Visual Studio Pluralization:** Why does scaffolding automatically create `CustomersController` and `/customers` for a singular `Customer` model?
- **Database Basics Revision:** Review ER diagrams, schema design, and normalization (1NF, 2NF, 3NF).
- **OpenAPI Support:** What does OpenAPI support do in a Web API project?
- **Polly for Resilience:** How to implement Circuit Breaker, Timeout, and Retry using the Polly library instead of manual while loops.
- **Circuit Breaker Code Examples:** Create practical code examples showing the Open, Closed, and Half-Open states.
- **API Versioning:** How to properly structure multiple versions of a Web API endpoint.

## Medium Priority
- **Primary Constructors:** What are the benefits of primary constructors vs traditional constructors?
- **Namespaces vs Using:** How encapsulation across files works in C#. When do you wrap code in a namespace?
- **Managed vs Unmanaged Objects:** Deep dive into how DB connections (`SqlConnection`) compare to normal C# objects.
- **Nullables and Null-Forgiving:** How do `?` and `!` exactly work in variable and function declarations?
- **Master vs Model Databases:** What are all the system databases listed in SQL Server Explorer (master, model, msdb, tempdb)?
- **MVC Return Types:** Why does returning a `string` from an MVC Action overwrite the entire view (navbar included)?
- **HTTPS in MVC:** Why do we sometimes disable HTTPS support in the MVC visual project?

## Low Priority
- **Delegates in other languages:** What is the equivalent of C#'s `Action` delegate in other languages (e.g., JavaScript, Python)?
- **Component Object Model (COM):** What is COM and how does it relate to the Tesco/NASA contractor reference from lecture?
- **Active References:** How does the runtime track what objects are still in use?

## Low Priority / Side Topics
- **DSA in C#:** Need to learn basic C# syntax to take input, sorting, etc. to do basic DSA smoothly.
- **Google Location Tracking:** Does Google track location without it being enabled (EU regulations context)?
- **Palantir:** What is their primary goal and business model?
- **DeepMind AI Parkour:** Context on AI physical simulations.
