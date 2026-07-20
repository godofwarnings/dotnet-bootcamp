# 01 - Services and Dependency Injection

A **service** is a plain class holding reusable **logic or data** — no template. Components handle the view; services handle data/logic. Any component can reuse one instance instead of duplicating.

```bash
ng generate service message   # alias: ng g s message
```

`Source: potahto/src/app/calculator/calculator-service.ts`

```ts
import { Injectable } from '@angular/core';
 
// providedIn: 'root' → a single app-wide instance (singleton), tree-shakable.
// Logic lives here, out of the component (separation of concerns).
@Injectable({ providedIn: 'root' })
export class CalculatorService {
  add(a: number, b: number): number { return a + b; }
  subtract(a: number, b: number): number { return a - b; }
  multiply(a: number, b: number): number { return a * b; }
  divide(a: number, b: number): number {
    return b === 0 ? NaN : a / b;   // guard: NaN signals an invalid op to the UI
  }
}
```

### `@Injectable({ providedIn: 'root' })` vs the new `@Service()` (v22)

Angular 22 adds **`@Service()`** as an ergonomic shorthand for `@Injectable({ providedIn: 'root' })`:

| | `@Service()` | `@Injectable()` |
|---|---|---|
| Root singleton | Implicit | Requires `{ providedIn: 'root' }` |
| `inject()` support | Yes | Yes |
| Constructor DI | **No** | Yes |
| Advanced providers (`useClass`/`useValue`/`useExisting`/`useFactory`) | **No** — one `factory` option instead | Yes |

> [!NOTE]
> **Why keep `@Injectable`, then?** Use `@Service()` for the common shared-singleton case. Reach back for `@Injectable` when you need constructor injection, a non-root scope (e.g. `providedIn: 'platform'`), or the advanced provider keys. All services in these projects use classic `@Injectable`.

### How a component gets the service — `inject()`

`Source: potahto/src/app/calculator/calculator.ts`

```ts
export class Calculator {
  private calculator = inject(CalculatorService);   // never `new CalculatorService()`
 
  a = 0; b = 0;
  result: number | null = null;
 
  add(): void { this.result = this.calculator.add(this.a, this.b); }
  // subtract/multiply/divide delegate to the service the same way
}
```

The component never calls `new`. It **asks** for the dependency via `inject()`, and Angular's injector constructs and supplies it. Payoff: **loose coupling** (depends on shape, not construction), **swappable** (provide a fake), **testable** (`TestBed.inject(...)`), **shared state** (root singleton).

> [!TIP] Try it
> Inject `CalculatorService` into a second component and confirm both share one instance. Then provide it at a component (not root) and watch each get its own — proof of how `providedIn` controls lifetime.
