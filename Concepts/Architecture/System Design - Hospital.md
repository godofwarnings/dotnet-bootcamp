# High Level System Design

## 1. Intro Example: Food Delivery App

When designing a system, we first identify the actors (users, restaurants, delivery partners) and map out their interactions.

```mermaid
flowchart TD
    actor["User"]
    actor2["Restaurant Owner"]
    actor3["Delivery Partner"]
    
    UC1["Browse Restaurants"]
    UC2["Placing Order"]
    UC3["Accept/Reject Order"]
    UC4["Manage Menu"]
    UC5["Accept Order (Delivery)"]
    UC6["Route to Restaurant/Customer"]
    
    actor --> UC1
    actor --> UC2
    actor2 --> UC3
    actor2 --> UC4
    actor3 --> UC5
    actor3 --> UC6
```

### Food Delivery Booking Flow
```mermaid
sequenceDiagram
    participant User
    participant WebAPI as Web API (MVC)
    participant DB as Database
    participant Restaurant
    participant Driver

    User->>WebAPI: Browse & Place Order
    WebAPI->>DB: Save Order Details
    DB-->>WebAPI: Order Confirmed
    WebAPI->>Restaurant: Notify (Accept/Reject)
    Restaurant->>WebAPI: Order Accepted
    WebAPI->>Driver: Dispatch (Route Info)
    Driver->>WebAPI: Order Picked Up/Delivered
```

---

## 2. Deep Dive: Hospital Appointment System

The core problem in a hospital booking system is preventing double booking while providing extremely fast search results for patients.

### 2.1 Users and Use Cases

- **Patient:** Register, login, search doctors, view slots, book, cancel, and view history.
- **Doctor:** View daily schedule and patient appointments.
- **Admin:** Add doctors, edit availability, and manage appointment records.

```mermaid
flowchart TD
    P[Patient]
    D[Doctor]
    A[Admin]

    U1[Register / Login]
    U2[Search Doctors]
    U3[View Available Slots]
    U4[Book Appointment]
    U5[Cancel Appointment]
    U6[View Appointment History]
    U7[View Daily Schedule]
    U8[Add Doctor]
    U9[Update Availability]
    U10[Manage Appointment Records]

    P --> U1
    P --> U2
    P --> U3
    P --> U4
    P --> U5
    P --> U6

    D --> U7

    A --> U8
    A --> U9
    A --> U10
```

### 2.2 High-Level Architecture

We break the system into modular microservices (Search, Slot, Appointment, Notification).

```mermaid
flowchart LR
    Client[Web / Mobile App]
    GW[API Gateway]
    Auth[Auth Service]
    Search[Doctor Search Service]
    Slot[Availability / Slot Service]
    Appt[Appointment Service]
    Notify[Notification Service]
    DoctorUI[Doctor Schedule Service]
    AdminUI[Admin Service]
    Cache[(Redis Cache)]
    DB[(Primary Database)]
    MQ[(Message Queue)]

    Client --> GW
    GW --> Auth
    GW --> Search
    GW --> Slot
    GW --> Appt
    GW --> DoctorUI
    GW --> AdminUI

    Search --> Cache
    Search --> DB
    Slot --> Cache
    Slot --> DB
    Appt --> DB
    Appt --> MQ
    MQ --> Notify
    Notify --> DB
    DoctorUI --> DB
    AdminUI --> DB
```

### 2.3 Booking Flow

Patients search doctors, view live slots, book appointments, and receive async confirmations. The most important correctness rule is preventing double-booking.

```mermaid
sequenceDiagram
    participant Patient
    participant API as Backend API
    participant Search as Search Service
    participant Slot as Slot Service
    participant Appt as Appointment Service
    participant DB as Database
    participant MQ as Message Queue
    participant N as Notification Service

    Patient->>API: Search doctors by specialization
    API->>Search: Query request
    Search->>DB: Fetch matching doctors
    DB-->>Search: Doctor list
    Search-->>API: Results
    API-->>Patient: Show doctors

    Patient->>API: View available slots
    API->>Slot: Request slots
    Slot->>DB: Read doctor availability + booked slots
    DB-->>Slot: Slot data
    Slot-->>API: Available slots
    API-->>Patient: Show slots

    Patient->>API: Book appointment
    API->>Appt: Create booking request
    Appt->>DB: Atomic insert with no-double-booking rule
    DB-->>Appt: Success or failure
    Appt->>MQ: Publish AppointmentBooked event
    MQ-->>N: Consume event
    N-->>Patient: SMS / email confirmation
    Appt-->>API: Booking response
    API-->>Patient: Confirm booking
```

### 2.4 Suggested Data Model

```mermaid
classDiagram
    class User {
      id
      name
      email
      phone
      role
      password_hash
    }

    class Doctor {
      id
      user_id
      specialization
      active_status
      hospital_id
    }

    class DoctorAvailability {
      id
      doctor_id
      day_of_week
      start_time
      end_time
      slot_duration
    }

    class Appointment {
      id
      patient_id
      doctor_id
      slot_start
      slot_end
      status
      created_at
    }

    class Notification {
      id
      appointment_id
      channel
      status
      sent_at
    }

    User <|-- Doctor
    Doctor --> DoctorAvailability
    Doctor --> Appointment
    User --> Appointment
    Appointment --> Notification
```

### 2.5 Key Design Decisions
1. **Search caching:** Use Redis for fast search and availability reads.
2. **Dynamic Slot Generation:** Generate slots from schedule data instead of storing every possible slot forever.
3. **Atomic DB Booking:** Use atomic DB transactions or unique constraints to avoid double-booking ([See Race Conditions](../Database/Race%20Conditions%20and%20Locking.md)).
4. **Async Notifications:** Do booking and notification separately. Send confirmation messages asynchronously using a Message Queue so the booking stays fast and doesn't fail if the email service goes down.
