# PUT vs POST

The primary difference between `PUT` and `POST` is **idempotency**.

- **PUT** is idempotent. This means if you execute the same PUT request multiple times, the outcome on the server will be exactly the same as executing it once (e.g., updating a resource to have specific values).
- **POST** is not idempotent. Executing the same POST request multiple times will usually create multiple duplicate resources (e.g., creating a new user).

**Best Practice:**
When updating a resource via `PUT`, it's recommended to include a `?` (nullable) for the ID or perform proper validation so that bad data does not crash your application.
