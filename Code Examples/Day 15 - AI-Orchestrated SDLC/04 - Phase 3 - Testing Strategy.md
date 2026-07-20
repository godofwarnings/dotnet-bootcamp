# 04 - Phase 3 - Testing strategy (unit, integration, automation)
 
The suite uses **xUnit**, split into distinct projects for separation of concerns.
 
| Test type | What it checks | Isolation | Typical tools |
| --- | --- | --- | --- |
| **Unit** | One business-logic component / controller in isolation | Dependencies **mocked** | xUnit + **Moq** |
| **Integration** | Components working together (controller → service → DB); contract & persistence | Real execution paths, minimal mocking | xUnit + `WebApplicationFactory` / test server, direct HTTP calls |
| **UI automation (E2E)** | Real user workflows in the browser (forms, DOM, full leave flow) | Nothing mocked — real browser | **Selenium** + `ChromeDriver` / `GeckoDriver` |
 
- **Unit tests** mock the `DbContext` and external services so failures point at a single method and run fast.
- **Integration tests** issue real HTTP calls to a test server to confirm database persistence and contract compliance in a near-real environment.
- **UI automation** drives an actual browser to assert on rendered elements and end-to-end behavior.
 
> [!TIP] The testing pyramid
> Keep **many** fast unit tests, **fewer** integration tests, and **few** slow E2E/Selenium tests. E2E is the most valuable *and* the most brittle and expensive — reserve it for critical user journeys (e.g. "apply for leave → approve → see status").
 
> [!NOTE] Why separate projects?
> Splitting `*.UnitTests` from `*.IntegrationTests` lets CI run the fast unit suite on every commit and the slower integration/E2E suites on a different cadence (nightly, pre-merge). See **Part 2 → CI/CD Pipeline** for where these run.
