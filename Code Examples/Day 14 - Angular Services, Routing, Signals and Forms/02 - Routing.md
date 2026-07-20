# 02 - Routing

Routing maps a **URL** to a **component**. Definitions live in `app.routes.ts`.

`Source: potahto/src/app/app.routes.ts`

```ts
export const routes: Routes = [
  { path: 'home',        component: Home },
  { path: 'about',       component: About },
  { path: 'contactus',   component: ContactUs },
  { path: 'products/:id', component: ProductDetails },        // :id = route parameter
  { path: '',   redirectTo: 'home', pathMatch: 'full' },      // default → redirect
  { path: 'admin', component: Admin, canActivate: [authGuard] }, // protected route
  { path: '**',  component: PageNotFound },                   // wildcard / 404 — must be LAST
];
```

In the template: `<a routerLink="/home">` navigates without a page reload (SPA), and `<router-outlet>` is where the matched component renders.

`Source: potahto/src/app/app.html` (routing nav, restored from commented history)

```html
<nav>
  <a routerLink="/home">HOME</a>
  <a routerLink="/about">ABOUT</a>
  <a routerLink="/contactus">CONTACT US</a>
  <a routerLink="/admin">ADMIN</a>
 
  <!-- dynamic segment + query params -->
  <a [routerLink]="['/products', 10]" [queryParams]="{ category: 'electronics', sort: 'price' }">
    Product 10
  </a>
</nav>
 
<router-outlet></router-outlet>
```

> [!WARNING] Misconception in the original code comment
> A comment in `app.routes.ts` claimed the `**` wildcard "can be placed anywhere now." **Not true.** Angular still matches routes **in order, first match wins**, so `**` must be **last** — otherwise it shadows every route after it. (Your array *does* place it last, so routing works; only the comment's reasoning is wrong.) Verified against the Angular routing guide.

### Route (path) parameters vs query parameters

Both come from the injected `ActivatedRoute`; the difference is **which map** and **where in the URL**:

| | Route (path) parameter | Query parameter |
|---|---|---|
| URL | `/products/5` | `/products/5?category=electronics&sort=price` |
| Route def | `path: 'products/:id'` | none |
| Read via | `route.paramMap.get('id')` | `route.queryParamMap.get('sort')` |
| For | *which* resource (required) | optional extras: filter, sort, page |

`Source: potahto/src/app/product-details/product-details.ts`

```ts
export class ProductDetails {
  private route = inject(ActivatedRoute);
 
  productId = signal(0);
  category = signal('');
  sortBy = signal('');
 
  constructor() {
    // paramMap & queryParamMap are Observables — they re-emit when the URL changes
    this.route.paramMap.subscribe(p => this.productId.set(Number(p.get('id'))));
    this.route.queryParamMap.subscribe(q => {
      this.category.set(q.get('category') ?? '');   // ?? '' : null-coalesce missing param
      this.sortBy.set(q.get('sort') ?? '');
    });
  }
}
```

### Why `[routerLink]` (property binding) instead of a plain string?

Rule: **no brackets → literal text; brackets → evaluate as a TypeScript expression.**

- `routerLink="/home"` — literal string, nothing to compute.
- `[routerLink]="['/products', 10]"` — property binding; the right side is evaluated as a real array (the "link parameters array") that Angular joins into `/products/10`.

**Why the array form needs brackets:** without them Angular reads the literal characters `"['/products', 10]"`, not an array. Any non-string value (array, number, variable) requires property binding. Same reason `[queryParams]="{…}"` uses brackets — it's an object, not text.

> [!NOTE]
> For a fixed URL, `routerLink="/products/10"` and `[routerLink]="['/products', 10]"` are equivalent. The array form is required only when a segment is **dynamic** (`product.id` from data). Gotcha: with brackets, a literal string needs inner quotes — `[routerLink]="'/home'"`.

### Route Guards

A guard decides whether navigation to a route is allowed. Current Angular uses **functional guards** (`CanActivateFn`).

`Source: potahto/src/app/auth-guard.ts`

```ts
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { Auth } from './auth';
 
export const authGuard: CanActivateFn = () => {
  const authService = inject(Auth);
  const router = inject(Router);
 
  if (authService.isLoggedIn()) return true;      // allow
  return router.createUrlTree(['/home']);          // else redirect to /home
};
```

Attach it with `canActivate: [authGuard]` (see the `admin` route above). A guard returns `true` (allow), `false` (block), or a `UrlTree` (redirect). Returning a `UrlTree` is preferred over blocking silently, so the user lands somewhere sensible.

> [!TIP] Try it
> Navigate to `/admin` while logged out — the guard redirects to `/home`. Click Login (flips the `Auth` signal), then retry — it now allows through.
