# 04 - Angular Registration Preview

### Putting It Together — Registration Live Preview

The `Registration` component combines two-way binding, conditional rendering, pipes, and property binding in one form.

`Source: src/app/registration/registration.ts`

```ts
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';                  // enables [(ngModel)]
import { TitleCasePipe, UpperCasePipe } from '@angular/common';

@Component({
  selector: 'app-registration',
  imports: [FormsModule, TitleCasePipe, UpperCasePipe],
  templateUrl: './registration.html',
  styleUrl: './registration.css',
})
export class Registration {
  name = '';
  city = '';
  technology = '';

  clearFields(): void {
    this.name = '';
    this.city = '';
    this.technology = '';
  }
}
```

`Source: src/app/registration/registration.html`

```html
<div class="form">
  <h1>Registration</h1>

  <input type="text" [(ngModel)]="name" placeholder="Name" />
  <input type="text" [(ngModel)]="city" placeholder="City" />
  <input type="text" [(ngModel)]="technology" placeholder="Technology" />

  <!-- Clear is disabled only when every field is empty -->
  <button (click)="clearFields()" [disabled]="!name && !city && !technology">
    Clear
  </button>
</div>

<!-- preview appears only once a name has been entered -->
@if (name) {
  <div class="preview">
    <h2>Registration Preview</h2>
    <p>Name: {{ name | titlecase }}</p>
    <p>City: {{ city }}</p>
    <p>Technology: {{ technology | uppercase }}</p>
    <p>Name length: {{ name.length }}</p>
  </div>
}
```

- Demonstrates all four binding types working together: `[(ngModel)]` (two-way), `@if` (conditional), `| titlecase`/`| uppercase` (pipes), and `[disabled]` (property).
