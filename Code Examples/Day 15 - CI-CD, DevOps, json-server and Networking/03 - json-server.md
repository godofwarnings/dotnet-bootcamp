# 03 - json-server — a zero-code mock REST API
 
When the front end is built before the real backend exists, `json-server` spins up a **full fake REST API from a single JSON file** — no code required — so Angular/React developers can hit realistic endpoints during development.
 
### Install
 
```bash
npm install json-server --save-dev
```
 
- `--save-dev` records it under `devDependencies` in `package.json`, because it's a **development-time tool**, not something shipped to production.
 
### Usage
 
Create a `db.json` describing your data:
 
```json
{
  "products": [
    { "id": 1, "name": "Keyboard", "price": 40 },
    { "id": 2, "name": "Mouse", "price": 20 }
  ]
}
```
 
Start the server:
 
```bash
npx json-server --watch db.json --port 3000
```
 
You now get a REST API for free:
 
| Method | URL | Effect |
| --- | --- | --- |
| `GET` | `/products` | List all products |
| `GET` | `/products/1` | Get product with `id` 1 |
| `POST` | `/products` | Create a product |
| `PUT` / `PATCH` | `/products/1` | Update product 1 |
| `DELETE` | `/products/1` | Delete product 1 |
 
Writes are **persisted back to `db.json`**, so the mock behaves like a real stateful API across requests.
 
> [!TIP] Why this and not hard-coded data in the component?
> Hard-coded arrays don't exercise the **HTTP layer** — your `HttpClient` service, error handling, loading states, and CORS behavior all go untested. `json-server` lets you develop against a *real* network boundary, so swapping in the production API URL later is a one-line change.
 
> [!WARNING] It is a mock, not a backend
> `json-server` has no auth, no validation, no business rules, and stores everything in a flat file. Never use it in production — it exists only to unblock front-end work. (Contrast with the hardened, validated .NET API in **Part 1**.)
