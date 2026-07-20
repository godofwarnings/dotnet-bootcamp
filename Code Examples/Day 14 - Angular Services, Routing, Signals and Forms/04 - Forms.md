# 04 - Forms

Angular has two form styles. Both were built in this session.
 
### 4.1 Template-Driven Forms
 
The form model lives **in the template**, driven by `ngModel` + `ngForm` (from `FormsModule`). Good for simple forms.
 
`Source: potahto/src/app/app.html` (template-driven form, restored from commented history; trimmed)
 
```html
<form #registrationForm="ngForm" (ngSubmit)="submitForm()">
  <input type="text" name="name" [(ngModel)]="name" required #nameControl="ngModel">
  @if (nameControl.invalid && nameControl.touched) {
    <p>Name is required.</p>
  }
 
  <input type="email" name="email" [(ngModel)]="email" required email #emailControl="ngModel">
  @if (emailControl.errors?.['email'] && emailControl.touched) {
    <p>Enter a valid email address.</p>
  }
 
  <input type="tel" name="phone" [(ngModel)]="phone" required pattern="[0-9]{10}" #phoneControl="ngModel">
  @if (phoneControl.errors?.['pattern'] && phoneControl.touched) {
    <p>Enter a valid 10-digit phone number.</p>
  }
 
  <!-- radio group: same name + same [(ngModel)] ⇒ ONE control; `value` is what's stored -->
  <input type="radio" name="trainingMode" [(ngModel)]="trainingMode" value="Online" required> Online
  <input type="radio" name="trainingMode" [(ngModel)]="trainingMode" value="Classroom" required> Classroom
 
  <!-- checkbox binds to a boolean; required ⇒ must be checked -->
  <input type="checkbox" name="terms" [(ngModel)]="acceptedTerms" required #termsControl="ngModel">
 
  <button type="submit" [disabled]="registrationForm.invalid">Submit</button>
</form>
```
 
Key mechanics:
 
- **`#registrationForm="ngForm"`** exposes the whole form; **`#nameControl="ngModel"`** exposes one control — both are template reference variables bound to Angular's form directives.
- **`name="…"` is required** on each `ngModel` input so the form can register the control by name.
- **Validators are attributes**: `required`, `email`, `pattern="…"`. Errors surface via `control.errors?.['pattern']`.
- **Radio group**: same `name` + same `[(ngModel)]` makes several radios act as **one** control; the selected `value` is stored.
- **Checkbox** binds to a **boolean**.
 
> [!NOTE] `touched` vs `touch` — the control-state family
> There's no "touch" state; the states come in pairs (with methods like `markAsTouched()`):
> - **untouched / touched** — has the control been **blurred** (focused then left)? Used to delay errors until the user has interacted.
> - **pristine / dirty** — has the **value** been changed?
> - **valid / invalid** — do the validators pass?
> - **pending** — an async validator is still running.
>
> That's why errors are gated on `control.invalid && control.touched` — don't shout "required!" before the user has even reached the field.
 
### 4.2 Reactive Forms
 
The form model lives **in the component class** (TypeScript), built from `FormGroup`/`FormControl`/`Validators` and wired with `[formGroup]` + `formControlName` (from `ReactiveFormsModule`).
 
`Source: potahto/src/app/app.ts` (reactive form, restored from commented history)
 
```ts
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
 
@Component({ selector: 'app-root', imports: [ReactiveFormsModule], /* … */ })
export class App {
  registrationForm = new FormGroup({
    name:   new FormControl('', [Validators.required, Validators.minLength(3)]),
    email:  new FormControl('', [Validators.required, Validators.email]),
    course: new FormControl('', [Validators.required]),
  });
 
  submitForm(): void {
    if (this.registrationForm.valid) console.log(this.registrationForm.value);
  }
}
```
 
```html
<form [formGroup]="registrationForm" (ngSubmit)="submitForm()">
  <input type="text" formControlName="name">
  @if (registrationForm.controls.name.invalid && registrationForm.controls.name.touched) {
    <p>Name is required and must be at least 3 characters.</p>
  }
  <button type="submit" [disabled]="registrationForm.invalid">Submit</button>
</form>
```
 
> [!NOTE] Template-driven vs reactive — "backend control?"
> "Backend" here means the **component's TypeScript**, not the server. That's the core contrast:
> - **Template-driven** — the model is defined **in the template** (`ngModel`); Angular builds the `FormGroup` implicitly. Less code; harder to compose/validate dynamically.
> - **Reactive** — the model is defined **in code** (`new FormGroup({…})`), giving explicit, programmatic control: compose validators, react to `valueChanges`, add/remove controls at runtime.
>
> **Why reactive is easier to unit-test**: the form model is a plain object in the class, so you can assert validity and values **without rendering the DOM**. Template-driven forms bury that logic in the template, so testing needs a rendered component.
 
> [!TIP] Try it
> Log `registrationForm.valueChanges.subscribe(v => console.log(v))` in the reactive form and watch it emit on every keystroke — the reactive model is observable. The template-driven form has no equivalent single stream.
