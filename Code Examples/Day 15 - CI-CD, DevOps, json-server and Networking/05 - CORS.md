# 05 - CORS (Cross-Origin Resource Sharing)
 
**CORS** is a browser security mechanism controlling whether a page from one **origin** may call a different origin.
 
- An **origin** = `scheme + host + port`. `http://localhost:4200` and `http://localhost:3000` are **different origins** (different port).
- The browser's **same-origin policy** blocks cross-origin reads from JavaScript by default. CORS is the *opt-in* by which the server says "requests from this other origin are allowed."
 
**How it works:** the server returns the right response headers, e.g.:
 
```http
Access-Control-Allow-Origin: http://localhost:4200
Access-Control-Allow-Methods: GET, POST, PUT, DELETE
Access-Control-Allow-Headers: Content-Type, Authorization
```
 
For "non-simple" requests (custom headers, `PUT`/`DELETE`, JSON bodies) the browser first sends a **preflight `OPTIONS`** request to check permission before the real request.
 
> [!WARNING] CORS is enforced by the *browser*, not the server
> The server still receives and could process the request; the browser just refuses to hand the **response** back to your JavaScript if the headers don't allow the origin. This is why the same call works from Postman/curl (no CORS enforcement) but fails in the browser.
 
> [!TIP] Where you'll hit this
> Running Angular on `:4200` and `json-server`/ASP.NET Core on another port triggers CORS. Fixes:
> - In **ASP.NET Core**, configure a CORS policy with `builder.Services.AddCors(...)` + `app.UseCors(...)`.
> - In Angular dev, use a **proxy** (`proxy.conf.json` + `ng serve --proxy-config`) so calls appear same-origin.
> - `json-server` enables permissive CORS by default, which is why the mock "just works."
