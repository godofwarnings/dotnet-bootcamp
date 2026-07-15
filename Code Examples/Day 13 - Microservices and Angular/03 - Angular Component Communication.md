# 03 - Angular Component Communication

### Component Communication (parent ↔ child)

The project's final, **active** state. The parent `App` embeds the `Product` child, passes data **down** with input bindings, and receives an event **up** via an output.

`Source: src/app/product/product.ts` (child)

```ts
import { Component, input, output } from '@angular/core';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe],
  templateUrl: './product.html',
  styleUrl: './product.css',
})
export class Product {
  productName = 'Wireless Mouse';
  price = 1499;
  isAvailable = true;
  isHighlighted = false;

  nameInput = input<string>();    // parent -> child (signal input)
  priceInput = input<number>();

  selected = output<string>();    // child -> parent event

  selectProduct(): void {
    this.selected.emit(this.nameInput() ?? this.productName);   // emit payload upward
  }

  toggleAvailability(): void { this.isAvailable = !this.isAvailable; }
  toggleHighlight(): void { this.isHighlighted = !this.isHighlighted; }
}
```

`Source: src/app/product/product.html` (trimmed to the communication parts)

```html
<div class="card" [class.highlight]="isHighlighted">
  <h1>{{ productName }}</h1>
  <p>Price - {{ price | currency:'INR' }}</p>

  <p>Availability -</p>
  @if (isAvailable) {
    <p class="in_stock">In Stock!</p>
  } @else {
    <p class="out_stock">Not in Stock... :\</p>
  }

  <button [disabled]="!isAvailable">Buy Now</button>

  <!-- parent -> child: values received via input() signals, read by calling them -->
  <h3>From parent: {{ nameInput() }}</h3>
  <h4>Price: {{ priceInput() | currency:'INR' }}</h4>

  <!-- child -> parent: clicking emits the (selected) output -->
  <button (click)="selectProduct()">Select this product</button>
</div>
```

`Source: src/app/app.ts` (parent)

```ts
import { Component } from '@angular/core';
import { Product } from './product/product';   // import child to use its selector

@Component({
  selector: 'app-root',
  imports: [Product],   // register the child component for this template
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  productName = 'Wireless Keyboard';   // passed down via [nameInput]
  productPrice = 3500;                 // passed down via [priceInput]
  selectedProduct = '';

  receiveProduct(productName: string): void {   // handles the child's (selected) output
    this.selectedProduct = productName;
  }
}
```

`Source: src/app/app.html` (parent)

```html
<app-product
  [nameInput]="productName"
  [priceInput]="productPrice"
  (selected)="receiveProduct($event)">
</app-product>

@if (selectedProduct) {
  <p>You selected: {{ selectedProduct }}</p>
}
```

- **Down (parent → child):** the parent binds `[nameInput]`/`[priceInput]`; the child declares them with `input<T>()` and reads them by **calling the signal** (`nameInput()`).
- **Up (child → parent):** the child declares `selected = output<string>()` and calls `selected.emit(...)`; the parent listens with `(selected)="receiveProduct($event)"`, where `$event` is the emitted payload.
- **`input()` / `output()`** are the current **signal-based** APIs, replacing the older `@Input()` / `@Output()` decorators.

> [!NOTE]
> **Angular vs. React (added context — not from the project).** Angular's `input()` maps to React **props**; Angular's `output()` event maps to a **callback prop** in React (e.g. `onSelect`). Angular wires child→parent through an `EventEmitter`/output the parent subscribes to in the template, whereas React passes a function down and the child invokes it directly.
