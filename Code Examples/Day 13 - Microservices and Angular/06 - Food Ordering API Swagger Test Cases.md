# 06 - Food Ordering API Swagger Test Cases

Manual test cases for exercising the API through Swagger UI ("Try it out").

#### How to run

1. Start the API:
   ```bash
   dotnet run --project FoodOrderingApi
   ```
2. Open Swagger UI: **http://localhost:5005/swagger**
3. Expand **POST /api/orders** → click **Try it out**.
4. For each test below:
   - Paste the JSON into the **request body**.
   - Set the **`Idempotency-Key`** header field (a required parameter on this endpoint — use a **new unique value** for every distinct order, e.g. `TC01-key`).
   - Click **Execute** and compare against the **Expected result**.

> [!IMPORTANT]
> The endpoint is idempotent: reusing an `Idempotency-Key` that was already processed returns the **original cached response** and does **not** create a new order (see TC-15).

#### Seeded test data (reference)

**Menu items** (`MenuItemId` → name, price, availability)

| Id | Item | Price | Available |
|----|------|-------|-----------|
| 1 | Chicken Biryani | 250 | ✅ |
| 2 | Paneer Butter Masala | 220 | ✅ |
| 3 | Mutton Biryani | 350 | ❌ (unavailable) |
| 4 | Butter Naan | 40 | ✅ |
| any other | — | — | not found |

**Customers** — every `CustomerId` exists **except `404`** (treated as "not found").

**Restaurants** (by `RestaurantId`)

| Id | Behaviour |
|----|-----------|
| `999` | Not found |
| `500` | Exists but **closed** |
| any other positive id | Open / serving |

**Payment** — `PaymentMethod` of `"Declined"` (case-insensitive) is rejected by the gateway; any other value (e.g. `"CreditCard"`, `"UPI"`) is approved.

#### Happy path

##### TC-01 — Place a valid order (single item)
**Header:** `Idempotency-Key: TC01-key`
```json
{
  "customerId": 101,
  "restaurantId": 10,
  "items": [
    { "menuItemId": 1, "quantity": 2 }
  ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `201 Created`. Body has `status: "OrderPlaced"`, `totalAmount: 500` (250 × 2), `paymentStatus: "APPROVED"`, a non-empty `transactionId`, `deliveryPartnerName: "Rahul Delivery Partner"`, `estimatedDeliveryTimeInMinutes: 35`, `notificationStatus: "NotificationQueued"`, and an `orderId` starting at 9001+.

##### TC-02 — Place a valid order (multiple items)
**Header:** `Idempotency-Key: TC02-key`
```json
{
  "customerId": 102,
  "restaurantId": 10,
  "items": [
    { "menuItemId": 1, "quantity": 1 },
    { "menuItemId": 2, "quantity": 2 },
    { "menuItemId": 4, "quantity": 3 }
  ],
  "deliveryAddress": "45 Park Street, Kolkata",
  "paymentMethod": "UPI"
}
```
**Expected:** `201 Created`. `totalAmount: 810` (250×1 + 220×2 + 40×3). `status: "OrderPlaced"`.

#### Validation errors → `400 Bad Request`

##### TC-03 — Missing Idempotency-Key header
**Header:** *(leave `Idempotency-Key` empty)*
```json
{
  "customerId": 101,
  "restaurantId": 10,
  "items": [ { "menuItemId": 1, "quantity": 1 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `400 Bad Request`, message `"Idempotency-Key header is required."`

##### TC-04 — Invalid customer id (≤ 0)
**Header:** `Idempotency-Key: TC04-key`
```json
{
  "customerId": 0,
  "restaurantId": 10,
  "items": [ { "menuItemId": 1, "quantity": 1 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `400 Bad Request`, message `"Invalid customer id."`

##### TC-05 — Invalid restaurant id (≤ 0)
**Header:** `Idempotency-Key: TC05-key`
```json
{
  "customerId": 101,
  "restaurantId": 0,
  "items": [ { "menuItemId": 1, "quantity": 1 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `400 Bad Request`, message `"Invalid restaurant id."`

##### TC-06 — Empty items list
**Header:** `Idempotency-Key: TC06-key`
```json
{
  "customerId": 101,
  "restaurantId": 10,
  "items": [],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `400 Bad Request`, message `"At least one item is required."`

##### TC-07 — Blank delivery address
**Header:** `Idempotency-Key: TC07-key`
```json
{
  "customerId": 101,
  "restaurantId": 10,
  "items": [ { "menuItemId": 1, "quantity": 1 } ],
  "deliveryAddress": "   ",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `400 Bad Request`, message `"Delivery address is required."`

##### TC-08 — Item quantity ≤ 0
**Header:** `Idempotency-Key: TC08-key`
```json
{
  "customerId": 101,
  "restaurantId": 10,
  "items": [ { "menuItemId": 1, "quantity": 0 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `400 Bad Request`, message `"Item quantity must be greater than zero."`

#### Not found → `404 Not Found`

##### TC-09 — Customer not found
**Header:** `Idempotency-Key: TC09-key`
```json
{
  "customerId": 404,
  "restaurantId": 10,
  "items": [ { "menuItemId": 1, "quantity": 1 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `404 Not Found`, message `"Customer not found."`

##### TC-10 — Restaurant not found
**Header:** `Idempotency-Key: TC10-key`
```json
{
  "customerId": 101,
  "restaurantId": 999,
  "items": [ { "menuItemId": 1, "quantity": 1 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `404 Not Found`, message `"Restaurant not found."`

##### TC-11 — Menu item not found
**Header:** `Idempotency-Key: TC11-key`
```json
{
  "customerId": 101,
  "restaurantId": 10,
  "items": [ { "menuItemId": 9999, "quantity": 1 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `404 Not Found`, message `"One or more menu items were not found."`

#### Business conflicts → `409 Conflict`

##### TC-12 — Restaurant closed
**Header:** `Idempotency-Key: TC12-key`
```json
{
  "customerId": 101,
  "restaurantId": 500,
  "items": [ { "menuItemId": 1, "quantity": 1 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `409 Conflict`, message `"Restaurant is currently closed."`

##### TC-13 — Menu item unavailable
**Header:** `Idempotency-Key: TC13-key`
```json
{
  "customerId": 101,
  "restaurantId": 10,
  "items": [ { "menuItemId": 3, "quantity": 1 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "CreditCard"
}
```
**Expected:** `409 Conflict`, message `"One or more menu items are currently unavailable."` (Mutton Biryani is unavailable.)

##### TC-14 — Payment declined
**Header:** `Idempotency-Key: TC14-key`
```json
{
  "customerId": 101,
  "restaurantId": 10,
  "items": [ { "menuItemId": 1, "quantity": 1 } ],
  "deliveryAddress": "12 MG Road, Bengaluru",
  "paymentMethod": "Declined"
}
```
**Expected:** `409 Conflict`, message `"Payment failed."`

#### Idempotency

##### TC-15 — Same Idempotency-Key returns the original order
1. First, run **TC-01** with header `Idempotency-Key: TC01-key` → note the returned `orderId`.
2. Execute this request **with the same header** `Idempotency-Key: TC01-key` but a **different body** (e.g. change quantity to 5):
   ```json
   {
     "customerId": 101,
     "restaurantId": 10,
     "items": [ { "menuItemId": 1, "quantity": 5 } ],
     "deliveryAddress": "Different Address",
     "paymentMethod": "CreditCard"
   }
   ```
**Expected:** Response is **identical to the original TC-01 response** — same `orderId` and `totalAmount: 500` (the new body is ignored because the key was already processed). No new order is created.

> [!NOTE]
> `Created` responses returned via the idempotency cache are surfaced as `201 Created` again with the cached body.

#### Coverage summary

| # | Scenario | Expected status |
|---|----------|-----------------|
| TC-01 | Valid order, single item | 201 |
| TC-02 | Valid order, multiple items | 201 |
| TC-03 | Missing Idempotency-Key | 400 |
| TC-04 | Customer id ≤ 0 | 400 |
| TC-05 | Restaurant id ≤ 0 | 400 |
| TC-06 | Empty items | 400 |
| TC-07 | Blank delivery address | 400 |
| TC-08 | Item quantity ≤ 0 | 400 |
| TC-09 | Customer not found (404 id) | 404 |
| TC-10 | Restaurant not found (999) | 404 |
| TC-11 | Menu item not found | 404 |
| TC-12 | Restaurant closed (500) | 409 |
| TC-13 | Menu item unavailable (id 3) | 409 |
| TC-14 | Payment declined | 409 |
| TC-15 | Idempotent replay | 201 (cached) |
