# 05 - HttpClient & RxJS
 
### 5.1 Enabling HttpClient
 
`HttpClient` must be provided before it can be injected.
 
`Source: angular-1/src/app/app.config.ts`
 
```ts
export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),   // makes HttpClient injectable app-wide
  ],
};
```
 
> [!WARNING]
> Injecting `HttpClient` without `provideHttpClient()` in the app config is the classic "No provider for HttpClient" error.
 
### 5.2 The RxJS pipeline — `.pipe()` and its operators
 
`HttpClient` methods return an **Observable** (a stream), not a Promise. **`.pipe()` is the "pipeline method"**: it composes operators that transform the stream between source and subscriber. Each operator takes the stream in and returns a new stream out.
 
`Source: potahto/src/app/user.ts`
 
```ts
getUsers(): Observable<User[]> {
  return this.http.get<User[]>(this.apiUrl).pipe(
    tap(users => console.log('Users received:', users)),        // side effect, doesn't alter data
    map(users => users.map(u => ({ ...u, name: u.name.toUpperCase() }))), // transform each item
    catchError(error => this.handleError(error)),                // intercept failures
  );
}
 
private handleError(error: HttpErrorResponse): Observable<never> {
  let message = 'An unexpected error occurred.';
  if (error.status === 0)        message = 'Unable to connect to the server.';
  else if (error.status === 404) message = 'Users were not found.';
  else if (error.status >= 500)  message = 'Server error. Please try again later.';
  return throwError(() => new Error(message));   // re-emit as an error the component can show
}
```
 
The operators used across both services:
 
| Operator | Role |
|---|---|
| `tap` | Run a **side effect** (logging) without changing the emitted value. |
| `map` | **Transform** each emitted value (uppercase names; unwrap an envelope). |
| `catchError` | **Intercept** an error and return a replacement stream (here, a friendly error via `throwError`). |
| `throwError` | Create an Observable that **errors** with the given value. |
| `finalize` | Run cleanup **whether the stream succeeds or fails** (used for the loading flag). |
 
`Source: potahto/src/app/users/users.ts`
 
```ts
loadUsers(): void {
  this.loading.set(true);
  this.message.set('');
 
  this.userService.getUsers()
    .pipe(finalize(() => this.loading.set(false)))   // turn off spinner on success OR error
    .subscribe({
      next: users => this.users.set(users),
      error: err  => this.message.set(err.message),
    });
}
```
 
> [!NOTE] Why `finalize` instead of setting `loading = false` in both `next` and `error`?
> `finalize` runs on **any** termination (complete, error, or unsubscribe), so the flag can't get stranded if a new branch is added later. Setting it in `next` only would leave the spinner spinning on error.
 
### 5.3 Subscribing & holding state in signals
 
`Source: potahto/src/app/app.ts`
 
```ts
users = signal<User[]>([]);
loading = signal(false);
errorMessage = signal('');
 
loadUsers(): void {
  this.loading.set(true);
  this.userService.getUsers().subscribe({
    next: users => { this.users.set(users); this.loading.set(false); },
    error: ()    => { this.errorMessage.set('Unable to load users.'); this.loading.set(false); },
  });
}
```
 
The `{ next, error }` observer object is how you react to each emission and to failures. Results land in **signals**, so the template re-renders automatically.
 
> [!NOTE] Signals vs Observables — which when?
> **Signals** = synchronous UI **state** read in templates (`x()`). **Observables** = asynchronous **streams/events** (HTTP, route changes). Convert with `toSignal()` / `toObservable()`. Here the pattern is: Observable (HTTP) → subscribe → push into a signal (state).
