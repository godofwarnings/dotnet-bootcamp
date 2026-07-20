# Day 14 Summary (Angular DI, Routing, Signals, Forms, RxJS)

**Date:** 16 July 2026

**Topics:** Dependency Injection, Routing & Guards, Signals, Forms (Template/Reactive), HttpClient, RxJS

## Overview

Continues the Angular track from [Day 13](Day%2013%20Summary.md). Covers **services + dependency injection**, **routing** (route/query params, guards, wildcard), **signals** (vs .NET SignalR), **forms** (template-driven and reactive), and **HttpClient + RxJS** — culminating in a Product Catalogue that loads from an API.

> [!NOTE]
> **Environment & sources.** Angular **v22**, standalone components (no NgModules), new control flow (`@if`/`@for`/`@empty`). Code is integrated from two projects: **`potahto/`** (concept exercises) and **`angular-1/`** (the API exercise). Many exercises were preserved as commented-out blocks in `app.ts`/`app.html`; they have been restored, cleaned, and grouped by concept.

> [!WARNING]
> **Tooling gotcha.** Angular CLI (v22) needs **Node ≥ 24.15.0**. The session's shell had 24.12.0, which blocked `ng serve`/`ng build` (a tooling gate, not a code problem).

## Why an Angular Client Over a Plain HTML/Script Client

A hand-written HTML + `<script>` + inline CSS client keeps markup, styling, and behavior tangled with **global scope** and **no module boundaries** — it doesn't stay maintainable. An Angular client is **modular** by construction: **components** (view), **DI services** (shared logic/data), **two-way binding**, **built-in control flow** (`@for … track`), and **CLI tooling** (scaffold/build/tree-shake/serve).

## Testing note — "easier in the backend than the frontend"

The observation holds for a concrete reason: **logic that lives in plain classes is unit-testable without a DOM or a browser.** That's why the maintainable patterns here are also the testable ones:

- **Services** (`CalculatorService`, `ProductService`) are plain classes — test methods directly, or `TestBed.inject(...)` with a mocked `HttpClient`.
- **Reactive forms** keep the model in code — assert `form.valid`/`form.value` with no rendering.
- **Template-driven forms** bury logic in the template — testing needs a rendered component, which is slower and more brittle.

> [!NOTE]
> General rule: push behavior **out of templates and into services/component classes**. Backend/server logic tends to be more testable simply because it has no view layer to render — the same principle applied to the front end (thin templates, logic in TypeScript) recovers most of that testability.

---

## References / Further Reading

- DI & services — [Creating and using services](https://angular.dev/guide/di/creating-and-using-services), [DI overview](https://angular.dev/guide/di)
- Routing — [Common routing tasks](https://angular.dev/guide/routing/common-router-tasks) (route order & wildcard), [`RouterLink` API](https://angular.dev/api/router/RouterLink), [Route guards / `CanActivateFn`](https://angular.dev/api/router/CanActivateFn)
- Signals — [Signals guide](https://angular.dev/guide/signals), [`computed`](https://angular.dev/guide/signals#computed-signals), custom two-way [`model()`](https://angular.dev/guide/signals/model)
- Forms — [Template-driven forms](https://angular.dev/guide/forms/template-driven-forms), [Reactive forms](https://angular.dev/guide/forms/reactive-forms), [Form validation](https://angular.dev/guide/forms/form-validation)
- HTTP — [HttpClient / `provideHttpClient`](https://angular.dev/guide/http)
- RxJS — [rxjs.dev operators](https://rxjs.dev/guide/operators) (`map`, `tap`, `catchError`, `finalize`, `throwError`)
- SignalR (the .NET one, for contrast) — [Introduction to ASP.NET Core SignalR](https://learn.microsoft.com/aspnet/core/signalr/introduction)
- Books: *ng-book* (Angular); *Pro ASP.NET Core* (Adam Freeman) for the .NET/SignalR side.

## Related Notes

- [Day 13 — Angular Template Fundamentals](Day%2013%20Summary.md)
- [[Angular Dependency Injection]]
- [[Angular Routing & Route Guards]]
- [[Route Params vs Query Params]]
- [[Angular Signals & computed]]
- [[Template-Driven vs Reactive Forms]]
- [[Angular Form Validation & Control States]]
- [[Angular HttpClient]]
- [[RxJS Operators & the pipe() Method]]
- [[Observables vs Signals]]
- [[ASP.NET Core SignalR]]
- [[Unit Testing Angular Services & Forms]]

## Suggested Prerequisite Notes

- Dependency Injection (concept) & the Singleton pattern
- Observer pattern & reactive streams
- Single-Page Applications (SPA) & client-side routing
- HTTP verbs & REST basics
- Promises vs Observables

---

## Code Examples
- [Services and Dependency Injection](../../Code%20Examples/Day%2014%20-%20Angular%20Services,%20Routing,%20Signals%20and%20Forms/01%20-%20Services%20and%20Dependency%20Injection.md)
- [Routing](../../Code%20Examples/Day%2014%20-%20Angular%20Services,%20Routing,%20Signals%20and%20Forms/02%20-%20Routing.md)
- [Signals](../../Code%20Examples/Day%2014%20-%20Angular%20Services,%20Routing,%20Signals%20and%20Forms/03%20-%20Signals.md)
- [Forms](../../Code%20Examples/Day%2014%20-%20Angular%20Services,%20Routing,%20Signals%20and%20Forms/04%20-%20Forms.md)
- [HttpClient and RxJS](../../Code%20Examples/Day%2014%20-%20Angular%20Services,%20Routing,%20Signals%20and%20Forms/05%20-%20HttpClient%20and%20RxJS.md)
- [Product Catalogue Exercise](../../Code%20Examples/Day%2014%20-%20Angular%20Services,%20Routing,%20Signals%20and%20Forms/06%20-%20Product%20Catalogue%20Exercise.md)
