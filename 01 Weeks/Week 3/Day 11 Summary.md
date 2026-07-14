# Day 11 Summary (Training Enrollment & Testing)

## Overview
Day 11 focused exclusively on **Testing** in ASP.NET Core, transitioning from basic Unit Testing concepts into practical implementations using `xUnit` and `Moq`. 

The session evolved a `TrainingEnrollmentSolution` codebase. As the codebase grew incrementally, new services (like `CourseCompletionService`) were added, controllers were refactored, and tests were written for each new piece to demonstrate the exact workflow a developer goes through.

## Extracted Concepts & Resources
- [Unit Testing Fundamentals](../../Concepts/Testing/Unit%20Testing%20Fundamentals.md): Details the AAA pattern, Mocking vs. Integration, and how to set up an xUnit project. Also includes the links to the *Art of Unit Testing* book and the *Test Pyramid* article.
- [ASP.NET Core Integration Testing](../../Concepts/Testing/ASP.NET%20Core%20Integration%20Testing.md): Explains how to use `WebApplicationFactory` and the `public partial class Program` trick.
- [Global Resources Index](../../00%20Home/Resources.md): Centralized index tracking the recommended reading.

## Code Examples (Final State)
The extracted codebase represents the final state after all incremental chat additions (i.e., it includes both the initial setup and the later `CourseCompletionService` and Integration Tests fixes).

- [Models and DTOs](../../Code%20Examples/Day%2011%20-%20Training%20Enrollment%20Testing/Models%20and%20DTOs.md)
- [Services](../../Code%20Examples/Day%2011%20-%20Training%20Enrollment%20Testing/Services.md)
- [Controllers](../../Code%20Examples/Day%2011%20-%20Training%20Enrollment%20Testing/Controllers.md)
- [Unit Tests](../../Code%20Examples/Day%2011%20-%20Training%20Enrollment%20Testing/Unit%20Tests.md) (Moq and xUnit)
- [Integration Tests](../../Code%20Examples/Day%2011%20-%20Training%20Enrollment%20Testing/Integration%20Tests.md) (WebApplicationFactory)
