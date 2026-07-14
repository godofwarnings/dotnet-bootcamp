# Unit Testing Fundamentals

## The AAA Pattern
The most common standard for writing unit tests is the **AAA (Arrange-Act-Assert)** pattern:
- **Arrange:** Set up the test data, create the instances, and mock dependencies.
- **Act:** Execute the method you are testing.
- **Assert:** Verify that the output or the state matches what you expect.

## Mocking vs. Integration Testing
- **Unit Testing (with Mocking):** Tests a single unit of work in isolation. If a service depends on a database, file system, or external API, you *mock* those dependencies so the test runs quickly and predictably without touching actual infrastructure. If you don't mock it, it's not a unit test!
- **Integration Testing:** Tests how different parts of the system (or the entire system) work together, often including the real database, file system, or HTTP calls.

## Setting up xUnit in Visual Studio
To create a unit test project within a .NET Solution:
1. Right-click on your current solution -> **Add** -> **New Project**
2. Select **xUnit Test Project**
3. Once added, go to the unit test project -> Right-click -> **Add** -> **Project Reference** and select the main API/Business project.

Required NuGet packages for Mocking and Assertions (usually installed via NuGet Package Manager or CLI):
```bash
dotnet add package Moq
dotnet add package FluentAssertions
```

## Resources & Further Reading
- **Book:** [The Art of Unit Testing](https://www.manning.com/books/the-art-of-unit-testing-third-edition)
- **Article:** [The Practical Test Pyramid](https://martinfowler.com/articles/practical-test-pyramid.html) by Martin Fowler
