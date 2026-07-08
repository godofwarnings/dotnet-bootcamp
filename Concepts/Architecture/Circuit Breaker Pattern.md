# Circuit Breaker Pattern

The Circuit Breaker pattern is used to detect failures and encapsulate the logic of preventing a failure from constantly recurring, during maintenance, temporary external system failure or unexpected system difficulties. 

## States

```mermaid
graph LR
    subgraph "Circuit Breaker States"
    Closed[1. Closed: Normal Operations]
    Open[2. Open: Service Failing]
    HalfOpen[3. Half-Open: Testing Recovery]
    end

    Controller((Controller))
    FeeService((FeeService))

    Controller -- "Requests Flowing" --- Switch{Switch}
    Switch -. "Circuit Broken" .-> FeeService
    
    style Switch fill:#f96,stroke:#333,stroke-width:2px
```

1. **Closed**: Normal state. All requests are routed to the downstream service (e.g., `FeeService`).
2. **Open**: Triggered after a failure threshold is met. Requests fail instantly to save resources and prevent overwhelming the struggling downstream service.
3. **Half-Open**: A trial state to check if the downstream service is back online. A limited number of test requests are allowed through. If they succeed, the circuit closes; if they fail, the circuit opens again.

## Use Case
This pattern is often combined with **Retry Patterns**. A retry loop handles transient errors (like temporary network timeouts). However, if a service is completely down, repeatedly retrying just wastes resources. A circuit breaker stops the retries entirely until the service is healthy again.
