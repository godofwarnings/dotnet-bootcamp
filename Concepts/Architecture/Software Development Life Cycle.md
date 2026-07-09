---
tags: [architecture, sdlc, process]
---
# Software Development Life Cycle (SDLC)

## Wireframing
- Wireframes should be used in place of actual images while developing the UI. They give a general idea of the layout and structure without getting bogged down in high-fidelity design details early on.

## Promotion Path & Workflow

Below is a standard development lifecycle and promotion path showing how code moves from requirements all the way to production.

```mermaid
graph TD
    A[Product Owner: Requirements] --> B[Arch & Tech Lead: Tech Doc]
    B --> C[Coding Phase: SSEs & SEs]
    
    subgraph Development
    C --> C1[SE1: API/JSON & Postman Testing]
    C --> C2[SE2: UI & Wireframes]
    C1 & C2 --> D[Raise Pull Request]
    end

    D --> E{Deploy to Testing Env}
    
    subgraph Quality Assurance
    E --> F[QA: Test & Raise Bugs]
    F --> G[Fix Bugs]
    G --> H[Regression Testing]
    end

    H --> I[Promote to Higher Envs]

    subgraph Promotion Path
    I --> J[INT Env]
    J --> K[UAT Env]
    K --> L[PPE: Pre-Prod]
    L --> M[Prod: GO LIVE]
    end

    style M fill:#f96,stroke:#333,stroke-width:2px
```

### Key Environments:
- **INT**: Integration Environment
- **UAT**: User Acceptance Testing Environment
- **PPE**: Pre-Production Environment (staging)
- **Prod**: Production (Live)
