# Week 3: Microservices, Cloud & DevOps

This note summarizes learning from Week 3 of the .NET Bootcamp.

## Daily Summaries
- [Day 11 Summary (Training Enrollment & Testing)](Day%2011%20Summary.md)
- [Day 12 Summary (Clean Architecture, Service Boundaries, and Pub/Sub)](Day%2012%20Summary.md)
- [Day 13 Summary (Microservices, Angular & Food Ordering Patches)](Day%2013%20Summary.md)
- [Day 14 Summary (Angular DI, Routing, Signals, Forms, RxJS)](Day%2014%20Summary.md)
- [Day 15 Summary (AI-Orchestrated SDLC + CI/CD, Azure DevOps & Web Ops)](Day%2015%20Summary.md)

## Key Concepts Mastered
- **Testing & Verification**: 
  - Learned the AAA pattern (Arrange, Act, Assert) for xUnit testing.
  - Used `Moq` for isolated unit tests.
  - Developed end-to-end API tests using `WebApplicationFactory` ([Integration Testing](../../Concepts/Testing/ASP.NET%20Core%20Integration%20Testing.md)).
- **Clean Architecture**:
  - Migrated from monolithic "God Classes" to cleanly decoupled Service Boundaries.
  - Solved tight-coupling using the [Pub-Sub Pattern](../../Concepts/Architecture/Pub-Sub%20Pattern.md).
  - Isolated external dependencies with the [Anti-Corruption Layer (ACL)](../../Concepts/Architecture/Anti-Corruption%20Layer%20%28ACL%29.md).
- **Synchronous vs Asynchronous Work**:
  - Identified side-effects versus core logic to implement [Sync vs Async Operations](../../Concepts/Architecture/Sync%20vs%20Async%20Operations.md).

## Recommended Reading Sequence (Revision)
1. [Unit Testing Fundamentals](../../Concepts/Testing/Unit%20Testing%20Fundamentals.md)
2. [Service Boundaries](../../Concepts/Architecture/Service%20Boundaries.md)
3. [Pub-Sub Pattern](../../Concepts/Architecture/Pub-Sub%20Pattern.md)
4. [Anti-Corruption Layer (ACL)](../../Concepts/Architecture/Anti-Corruption%20Layer%20%28ACL%29.md)
