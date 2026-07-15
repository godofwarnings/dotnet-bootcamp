# 02 - Angular Template Concepts

> [!NOTE]
> Code in this section is integrated from the Angular practice project **`potahto`** (`Codebase/15-07-26/potahto/src`). Most concepts were saved as commented-out exercises in `app/app.ts` and `app/app.html`; they have been restored, cleaned, and grouped by concept below. The final, active state of the project is the parent↔child communication exercise. Prose explanations expand on the code; they were not part of the original note.

### Tooling & Language

- **npm** — the package manager for the Node.js/JavaScript ecosystem. Before package managers, dependencies were downloaded manually and committed to the project, with versions and transitive dependencies tracked by hand.
- **NuGet** — the .NET package manager. It plays the same role as npm but for the .NET ecosystem; they are separate tools for separate ecosystems (not one "after" the other).
- **TypeScript** — adds static typing on top of JavaScript: compile-time type checking, better tooling/IntelliSense, easier refactoring, and clearer contracts in large codebases. It compiles down to plain JavaScript. Angular is built with TypeScript.

### CLI Commands

```bash
ng serve -o                    # run the dev server; -o opens the browser automatically
ng generate component message  # scaffold a component (alias: ng g c message)
```

> [!TIP]
> `ng generate component <name>` creates the four-file component folder (`.ts`, `.html`, `.css`, `.spec.ts`) and wires up the class and selector. The `message`, `product`, and `registration` components in this project were all created this way.

### Project Structure (standalone bootstrap)

Modern Angular uses **standalone components** — there is no root `NgModule`. The app is bootstrapped directly from the root component, and each component declares its own `imports`.

`Source: src/main.ts`

```ts
import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
```

`Source: src/app/app.config.ts`

```ts
import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes)   // routes are currently empty (src/app/app.routes.ts)
  ]
};
```

**Purpose of each file.** `main.ts` is the entry point that bootstraps the root `App` component with `appConfig`. `app.config.ts` registers application-wide providers (here: the router and global error listeners). `app.routes.ts` holds route definitions (empty in this project). Each component folder contains its class (`.ts`), template (`.html`), styles (`.css`), and unit-test spec (`.spec.ts`).

### Interpolation

The simplest binding: render a component property into the template with `{{ expression }}`. Demonstrated by the generated `Message` component.

`Source: src/app/message/message.ts`

```ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-message',   // custom element tag used by other templates
  imports: [],
  templateUrl: './message.html',
  styleUrl: './message.css'
})
export class Message {
  text = 'This content comes from the Message component.';
}
```

`Source: src/app/message/message.html`

```html
<h2>Message Component</h2>
<p>{{ text }}</p>   <!-- interpolation renders the component's `text` property -->
```

- **Purpose:** display component state in the view.
- **Key syntax:** `{{ }}` evaluates a template expression and inserts its string result. It is one-way (component → view).

### Property Binding & Event Binding

Bind a DOM property with `[property]="expr"` and handle events with `(event)="handler()"`.

`Source: src/app/app.ts` (property-binding exercise, restored from commented history)

```ts
export class App {
  heading = 'Property Binding';
  buttonText = 'Submit';
  isButtonDisabled = true;

  enableButton(): void { this.isButtonDisabled = false; }
  disableButton(): void { this.isButtonDisabled = true; }
}
```

`Source: src/app/app.html`

```html
<h1>{{ heading }}</h1>

<button [disabled]="isButtonDisabled">{{ buttonText }}</button>

<button (click)="enableButton()">Enable Submit Button</button>
<button (click)="disableButton()">Disable Submit Button</button>
```

- **Property binding** `[disabled]="isButtonDisabled"` sets the element's *property* (not a static attribute) from a component value; it re-renders when the value changes.
- **Event binding** `(click)="enableButton()"` calls a method on the event. Mutating state in the handler triggers Angular to update bindings.
- **Common mistake:** confusing `[disabled]` (property, evaluates an expression) with a plain `disabled` attribute (always truthy if present).

### Two-Way Data Binding (`[(ngModel)]`)

"Banana in a box" — `[(...)]` combines property binding `[...]` and event binding `(...)` so the input and the component property stay in sync.

`Source: src/app/app.ts` (two-way-binding exercise, restored from commented history)

```ts
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';   // required for [(ngModel)]

@Component({
  selector: 'app-root',
  imports: [FormsModule],   // ngModel lives in FormsModule — import it here
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  name = 'khan';
  location = 'Hyd';
}
```

`Source: src/app/app.html`

```html
<input type="text" [(ngModel)]="name" placeholder="Enter your name" />
<input type="text" [(ngModel)]="location" placeholder="Enter your location" />

<p>You entered: {{ name }}</p>
<p>You entered: {{ location }}</p>
```

- **Where does `ngModel` live?** In `FormsModule` (`@angular/forms`). A standalone component must add `FormsModule` to its `imports` array or `[(ngModel)]` will not compile.
- **Mnemonic:** `[( )]` = a box `[ ]` containing a banana `( )`.

### Conditional Rendering (`@if` / `@else`)

Angular's built-in control-flow block (replaces the older `*ngIf`). No module import required.

`Source: src/app/app.ts` (conditional-rendering exercise, restored from commented history)

```ts
export class App {
  isLoggedIn = false;

  login(): void { this.isLoggedIn = true; }
  logout(): void { this.isLoggedIn = false; }
}
```

`Source: src/app/app.html`

```html
@if (isLoggedIn) {
  <p>Welcome! You are logged in.</p>
  <button (click)="logout()">Logout</button>
} @else {
  <p>Please log in.</p>
  <button (click)="login()">Login</button>
}
```

- **Key point:** `@if`/`@else` is template syntax built into the compiler — unlike `*ngIf`, it needs no `CommonModule` import.

### Looping (`@for`) and `track`

`Source: src/app/app.ts` (looping exercise, restored from commented history)

```ts
export class App {
  technologies = ['Angular', 'ASP.NET Core', 'SQL Server', 'Docker'];

  numbers = Array.from({ length: 9 }, (_, index) => {
    const i = index + 1;
    return `${i} is ${i % 2 === 1 ? 'odd' : 'even'}`;
  });
}
```

`Source: src/app/app.html`

```html
<ul>
  @for (technology of technologies; track technology) {
    <li>{{ technology }}</li>
  }
</ul>

<ul>
  @for (number of numbers; track number) {
    <li>{{ number }}</li>
  }
</ul>
```

- **What is `track`?** It gives each item a stable identity so Angular can re-render **only what changed** instead of rebuilding the whole list — the performance key to efficient list updates. `track` is **required** in `@for`.
- **Best practice:** track by a unique, stable id (e.g. `track item.id`); tracking by the value itself only works when values are unique.

### Attribute & Class Binding

`Source: src/app/app.ts` (attribute/class-binding exercise, restored from commented history)

```ts
export class App {
  message = 'Welcome to Angular';
  isHighlighted = true;
  tooltip = 'This text comes from TypeScript';

  toggleHighlight(): void { this.isHighlighted = !this.isHighlighted; }
}
```

`Source: src/app/app.html`

```html
<p [attr.title]="tooltip">Hover over this paragraph</p>

<p [class.highlight]="isHighlighted">{{ message }}</p>

<button (click)="toggleHighlight()">Toggle Highlight</button>
```

- **Attribute binding** `[attr.title]` sets an HTML *attribute* (use this for attributes that have no matching DOM property, e.g. `aria-*`, `colspan`).
- **Class binding** `[class.highlight]="isHighlighted"` toggles a **CSS class** based on a boolean.

> [!NOTE]
> Here "class" means a **CSS class**, not an OOP class.

### Style Binding

`Source: src/app/app.ts` (style-binding exercise, restored from commented history)

```ts
export class App {
  textColor = 'blue';   // drives [style.color]
  fontSize = 24;        // drives [style.font-size.px]

  makeRed(): void { this.textColor = 'red'; }
  increaseSize(): void { this.fontSize += 4; }
}
```

`Source: src/app/app.html`

```html
<p [style.color]="textColor" [style.font-size.px]="fontSize">
  This text changes dynamically.
</p>

<button (click)="makeRed()">Make Red</button>
<button (click)="increaseSize()">Increase Font Size</button>
```

- **Unit suffix:** `[style.font-size.px]="fontSize"` binds a numeric value and appends the `px` unit automatically.

### Pipes

Pipes transform a value for display without changing the underlying data. The built-in pipes come from `@angular/common`.

`Source: src/app/app.ts` (pipes exercise, restored from commented history)

```ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';   // provides built-in pipes

@Component({
  selector: 'app-root',
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  name = 'sla sh';
  message = 'learning angular is interesting';
  amount = 45678.5;
  today = new Date();     // consumed by the date pipe
  percentage = 0.85;      // fraction; percent pipe renders it as 85%
}
```

`Source: src/app/app.html`

```html
<p>Uppercase: {{ name | uppercase }}</p>
<p>Lowercase: {{ name | lowercase }}</p>
<p>Title case: {{ name | titlecase }}</p>

<p>Amount: {{ amount | currency }}</p>
<p>Indian currency: {{ amount | currency:'INR' }}</p>   <!-- argument after the colon -->

<p>Date: {{ today | date }}</p>
<p>Formatted date: {{ today | date:'dd-MM-yyyy' }}</p>

<p>Percentage: {{ percentage | percent }}</p>
```

- **Syntax:** `value | pipeName:arg1:arg2`. Chain pipes with multiple `|`.
- **Note:** newer standalone code often imports only the specific pipes it needs (e.g. `CurrencyPipe`, `TitleCasePipe`) rather than all of `CommonModule` — see the `Product` and `Registration` components below.

### Template Reference Variables

A `#ref` on an element exposes that element inside the template, so its value can be read without two-way binding.

`Source: src/app/app.ts` (template-reference exercise, restored from commented history)

```ts
export class App {
  enteredValue = '';

  // receives the input value passed from the template ref (#nameInput.value)
  displayValue(value: string): void {
    this.enteredValue = value;
  }
}
```

`Source: src/app/app.html`

```html
<input type="text" #nameInput placeholder="Enter your name" />

<button (click)="displayValue(nameInput.value)">Display Value</button>

<p>You entered: {{ enteredValue }}</p>
```

- **Key syntax:** `#nameInput` declares a reference to the `<input>`; `nameInput.value` reads its current value directly in the template.
