# Idempotency

Idempotency is the property of certain operations in mathematics and computer science whereby they can be applied multiple times without changing the result beyond the initial application.

In Web APIs, an **idempotent** endpoint guarantees that making the same request once or multiple times has the same effect on the server's state.

## HTTP Methods

- **GET, PUT, DELETE** are inherently idempotent by REST standards. Calling `PUT /students/1` ten times results in the same final state as calling it once.
- **POST** is **not** idempotent. Calling `POST /students` ten times creates ten different students.

## Implementing Idempotency for POST

Sometimes you need a `POST` operation (like processing a payment or generating a certificate) to be idempotent so that network retries don't result in duplicate charges or duplicate certificates.

### 1. Client-Side Prevention (UI)
The simplest defense is disabling the submit button on the UI immediately after it is clicked. This prevents impatient users from double-clicking and sending two identical requests.

### 2. Backend Prevention: The `Idempotency-Key` Header
For true API resilience, the client should generate a unique identifier (e.g., a GUID) for the transaction and pass it in a custom HTTP header (like `Idempotency-Key`).

**The Flow:**
1. Client generates UUID: `1234-abcd`
2. Client sends `POST /api/certificates` with `Idempotency-Key: 1234-abcd`
3. Server receives request. It checks its database for the key `1234-abcd`.
   - **If found**: The server returns the previously generated response without reprocessing.
   - **If not found**: The server processes the request, generates the certificate, saves the result to the database linked to `1234-abcd`, and returns the response.

This ensures that even if the client's network drops and they retry the exact same POST request, the server safely returns the cached result instead of generating a second certificate.
