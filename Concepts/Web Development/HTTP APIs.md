# HTTP APIs

## API and HTTP Actions

Standard HTTP verbs correspond to standard application logic and CRUD (Create, Read, Update, Delete) operations in a Web API or MVC backend.

```mermaid
graph TD
    subgraph Client ["Client Side (UI)"]
        direction TB
        G[<b>GET</b><br/><i>Read Data</i>] 
        P[<b>POST</b><br/><i>Create Data</i>] 
        U[<b>PUT</b><br/><i>Update Data</i>] 
        D[<b>DELETE</b><br/><i>Remove Data</i>]
    end

    subgraph Actions ["Application Logic"]
        direction TB
        G_App["• View Landing Screen"]
        P_App["• Create Post<br/>• Add Comment<br/>• Like Content<br/>• Share Post"]
        U_App["• Edit Post<br/>• Edit Comment"]
        D_App["• Remove Post<br/>• Remove Comment"]
    end

    subgraph Backend ["Infrastructure"]
        API[API Gateway]
        DB[(Database)]
    end

    %% Connections
    G --> G_App
    P --> P_App
    U --> U_App
    D --> D_App

    G_App & P_App & U_App & D_App --> API
    API --> DB
```

## JSON (JavaScript Object Notation)
We use JSON in HTTP responses because:
- It is highly readable for humans.
- It is cross-platform and language-agnostic.
- It is extremely easy to parse programmatically.
- It naturally represents Key-Value pairs.

## Status Codes
- Why use status codes? **Standardization**.
- HTTP status codes (200, 404, 500) provide a universal language that allows client applications (browsers, mobile apps) to instantly understand if a request succeeded, failed due to bad input, or crashed the server, without having to manually parse the response body text.
