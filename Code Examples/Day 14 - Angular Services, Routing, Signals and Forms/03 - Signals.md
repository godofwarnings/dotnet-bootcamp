# 03 - Signals (and why they're not SignalR)

> [!WARNING]
> **Angular `signal()` ≠ .NET SignalR.** A **signal** is an Angular front-end **reactive state** primitive. **SignalR** is an ASP.NET Core library for **real-time server↔client** messaging over WebSockets. Unrelated; different layers.
 
A signal is a reactive container — read by **calling it**, write with `.set()` / `.update()`, and any template reading it re-renders automatically.
 
`Source: potahto/src/app/auth.ts`
 
```ts
@Injectable({ providedIn: 'root' })
export class Auth {
  isLoggedIn = signal(false);          // reactive state, read as isLoggedIn()
  login(): void  { this.isLoggedIn.set(true); }
  logout(): void { this.isLoggedIn.set(false); }
}
```
 
`.update()` derives the next value from the current one (used later for the users list): `this.users.update(list => [...list, newUser])`.
 
### Relationship to two-way binding & `input()`/`output()` (from Day 13)
 
- `[(x)]` is **sugar** for `[x]="v"` + `(xChange)="v = $event"`. So `[(ngModel)]="name"` ≡ `[ngModel]="name" (ngModelChange)="name = $event"`.
- Parent→child `input()` and child→parent `output()` are two one-way bindings; pair them by the `x`/`xChange` convention and they become two-way. `model()` bundles input + output + `[(...)]` into one declaration.
 
> [!NOTE]
> **Can `input()`/`output()` talk between siblings, or live in a service?** No — they exist only as **template bindings** on components/directives. Siblings have no parent-child template link; a service has no template. For those cases use a **shared service**.
