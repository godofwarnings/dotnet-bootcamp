# 06 - Product Catalogue Exercise (`angular-1/`)
 
The capstone: load products from an API, filter locally, and render stock-aware cards.
 
> [!NOTE] Mock API trick (`json-server`)
> DummyJSON (`https://dummyjson.com/products`) was blocked, so a local `json-server` mock mirrors its shape. DummyJSON returns an **envelope** `{ products: [...], total, skip, limit }`, not a bare array. `json-server` serves a top-level **object** as a "singular resource" as-is, so `db.json` is nested:
> ```json
> { "products": { "products": [ /* 30 items */ ], "total": 30, "skip": 0, "limit": 30 } }
> ```
> → `GET http://localhost:3000/products` returns the exact envelope, so **only the URL changed** — the model/service didn't. One item (`Green Chili Pepper`, id 26) has `stock: 0` to demo the out-of-stock state.
> Run: `npm run api` (mock on :3000) + `npm start` (app on :4200).
 
### The model — an envelope interface
 
`Source: angular-1/src/app/models/product.ts`
 
```ts
export interface Product {
  id: number; price: number; rating: number; stock: number;
  title: string; description: string; category: string; thumbnail: string;
}
export interface ProductResponse {   // the API envelope
  products: Product[]; total: number; skip: number; limit: number;
}
```
 
### The service — unwrap the envelope with `map`
 
`Source: angular-1/src/app/services/product.service.ts`
 
```ts
getProducts(): Observable<Product[]> {
  return this.http.get<ProductResponse>(this.apiUrl).pipe(
    tap(res => console.log('Products received:', res)),
    map(res => res.products),                       // unwrap envelope → emit just Product[]
    catchError(err => {
      console.error('Api Error:', err);
      return throwError(() => new Error('Failed to load products. Try again.'));
    }),
  );
}
```
 
> [!NOTE] Design decision (from the session)
> The service returns `Observable<Product[]>` (unwrapped) rather than the raw `ProductResponse`. This keeps the envelope detail **inside the service**; the component gets a clean array. **Trade-off:** if pagination is added later, `total` is needed — expose it by returning the full response or adding a method.
 
### The component — auto-load + local filtering
 
`Source: angular-1/src/app/products/products.ts`
 
```ts
export class Products implements OnInit {
  private productService = inject(ProductService);
 
  products: Product[] = [];
  loading = false;
  errorMessage = '';
  searchText = '';
  selectedCategory = '';
 
  ngOnInit(): void { this.loadProducts(); }        // auto-load once on creation
 
  loadProducts(): void {
    this.loading = true;
    this.errorMessage = '';
    this.productService.getProducts().subscribe({
      next: products => { this.products = products; this.loading = false; },
      error: (err: Error) => { this.errorMessage = err.message; this.loading = false; },
    });
  }
 
  get categories(): string[] {                     // unique categories for the dropdown
    return [...new Set(this.products.map(p => p.category))];
  }
 
  get filteredProducts(): Product[] {              // LOCAL filter — no HTTP per keystroke
    const text = this.searchText.toLowerCase().trim();
    return this.products.filter(p => {
      const matchesCategory = !this.selectedCategory || p.category === this.selectedCategory;
      const matchesText = !text
        || p.title.toLowerCase().includes(text)
        || p.description.toLowerCase().includes(text)
        || p.category.toLowerCase().includes(text);
      return matchesCategory && matchesText;
    });
  }
}
```
 
- **`ngOnInit`** (lifecycle hook) runs once after construction — the right place to kick off the initial load (not the constructor, which should stay side-effect-free).
- **`[...new Set(...)]`** dedupes categories.
- **Getters filter locally** so typing doesn't fire a request per keystroke; they recompute on change detection.
 
`Source: angular-1/src/app/products/products.html` (trimmed)
 
```html
<input [(ngModel)]="searchText" placeholder="Search…">
<select [(ngModel)]="selectedCategory">
  <option value="">All categories</option>
  @for (cat of categories; track cat) { <option [value]="cat">{{ cat }}</option> }
</select>
 
@if (loading) {
  <p>Loading products...</p>
} @else if (errorMessage) {
  <p class="error">{{ errorMessage }}</p>
} @else {
  <div class="grid">
    @for (product of filteredProducts; track product.id) {     <!-- track by id, NOT $index -->
      <div class="card"
           [class.in-stock]="product.stock > 20"
           [class.low-stock]="product.stock > 0 && product.stock <= 20"
           [class.out-stock]="product.stock === 0">
        <h3>{{ product.title }}</h3>
        <p>{{ product.price | currency:'USD' }}</p>
        @if (product.stock > 20)      { <span class="badge in">In Stock</span> }
        @else if (product.stock > 0)  { <span class="badge low">Low Stock</span> }
        @else                         { <span class="badge out">Out of Stock</span> }
        <button [disabled]="product.stock === 0">Add to Cart</button>
      </div>
    } @empty {
      <p>No products match your search.</p>
    }
  </div>
}
```
 
This one template combines most of the course so far: two-way binding, `@for`/`@empty`/`track`, `@if`/`@else if`/`@else`, class binding, property binding (`[disabled]`, `[value]`, `[src]`), and the currency pipe.
 
> [!WARNING] `track product.id`, not `track $index`
> Tracking by `$index` re-associates DOM nodes by position, so a re-ordered/filtered list re-renders the wrong rows (and loses input state). Track by a **stable id**.
 
> [!TIP] Try it
> Compare `get filteredProducts()` with a **signals + `computed()`** version: hold `products`, `searchText`, `selectedCategory` as signals and derive `filteredProducts = computed(() => …)`. `computed` memoizes and only recalculates when a dependency changes — a getter recomputes on every change-detection pass.
