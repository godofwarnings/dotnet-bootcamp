# Day 15 Summary (AI-Orchestrated SDLC + CI/CD, Azure DevOps & Web Ops)
 
**Date:** 17 July 2026

**Topics:** AI-Assisted Development, Testing, Security, Docker, CI/CD Pipeline, Azure DevOps, json-server, Load Balancing, CORS
 
## Overview
 
The unifying theme is **how a feature travels from an idea to a running, hardened, deployed application** — and how an AI assistant compresses each stage.
 
- **Part 1** treats the AI as an *orchestrated collaborator*: the engineer designs high-level specs, the AI scaffolds and drafts, the engineer critically reviews, and together they build validation, tests, a security/resilience checklist, and a container image.
- **Part 2** covers the delivery plumbing that carries that app onward: the branch→PR→pipeline flow (CI/CD), where Azure DevOps fits, mocking the backend with `json-server` during front-end work, distributing traffic once deployed (Load Balancer vs. Traffic Manager), and the browser rule that governs cross-origin API calls (CORS).

---

## References / Further Reading

**AI-orchestrated development (Part 1)**
 
- xUnit — [xUnit.net documentation](https://xunit.net/)
- Moq — [Moq quickstart (GitHub)](https://github.com/devlooped/moq)
- Integration tests in ASP.NET Core — [Microsoft Learn](https://learn.microsoft.com/aspnet/core/test/integration-tests)
- Model validation & data annotations — [Microsoft Learn](https://learn.microsoft.com/aspnet/core/mvc/models/validation)
- Selenium WebDriver — [selenium.dev docs](https://www.selenium.dev/documentation/)
- Angular Reactive Forms — [angular.dev — Reactive forms](https://angular.dev/guide/forms/reactive-forms)
- Containerize a .NET app — [Microsoft Learn — .NET & Docker](https://learn.microsoft.com/dotnet/core/docker/build-container)
- .NET container images & default port 8080 — [Microsoft Learn — container images](https://learn.microsoft.com/dotnet/core/docker/container-images)
- Podman — [podman.io](https://podman.io/)
- EF Core — [Microsoft Learn — EF Core](https://learn.microsoft.com/ef/core/)
 
**Delivery & ops (Part 2)**
 
- Azure Pipelines — [What is Azure Pipelines? (Microsoft Learn)](https://learn.microsoft.com/azure/devops/pipelines/get-started/what-is-azure-pipelines)
- CI/CD concepts — [Continuous delivery overview (Microsoft Learn)](https://learn.microsoft.com/devops/deliver/what-is-continuous-delivery)
- Azure Load Balancer — [What is Azure Load Balancer? (Microsoft Learn)](https://learn.microsoft.com/azure/load-balancer/load-balancer-overview)
- Azure Traffic Manager — [What is Traffic Manager? (Microsoft Learn)](https://learn.microsoft.com/azure/traffic-manager/traffic-manager-overview)
- json-server — [typicode/json-server (GitHub)](https://github.com/typicode/json-server)
- CORS — [Cross-Origin Resource Sharing (MDN)](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS)
- Enable CORS in ASP.NET Core — [Microsoft Learn](https://learn.microsoft.com/aspnet/core/security/cors)
- Angular dev-server proxy — [angular.dev — proxying to a backend](https://angular.dev/tools/cli/serve#proxying-to-a-backend-server)
 
## Related Notes
 
- [[AI-Assisted Development Workflow]]
- [[Entity Framework Core Basics]]
- [[ASP.NET Core Model Validation]]
- [[Unit vs Integration Testing]]
- [[Selenium UI Automation]]
- [[Docker Multi-Stage Builds]]
- [[Resilience — RTO, RPO & Disaster Recovery]]
- [[Git Branching & Pull Requests]]
- [[Angular Reactive Forms]]
- [[Angular HttpClient]]
- [[ASP.NET Core Middleware Pipeline]]
- [[REST API Design]]

---

## Code Examples & Concepts

### Part 1: AI-Orchestrated SDLC
- [01 - Orchestrator Mindset and System Context](../../Code%20Examples/Day%2015%20-%20AI-Orchestrated%20SDLC/01%20-%20Orchestrator%20Mindset%20and%20System%20Context.md)
- [02 - Phase 1 - Project Initialization](../../Code%20Examples/Day%2015%20-%20AI-Orchestrated%20SDLC/02%20-%20Phase%201%20-%20Project%20Initialization.md)
- [03 - Phase 2 - Validation and Edge Cases](../../Code%20Examples/Day%2015%20-%20AI-Orchestrated%20SDLC/03%20-%20Phase%202%20-%20Validation%20and%20Edge%20Cases.md)
- [04 - Phase 3 - Testing Strategy](../../Code%20Examples/Day%2015%20-%20AI-Orchestrated%20SDLC/04%20-%20Phase%203%20-%20Testing%20Strategy.md)
- [05 - Phase 4 - API Contract and AI Self-Criticism](../../Code%20Examples/Day%2015%20-%20AI-Orchestrated%20SDLC/05%20-%20Phase%204%20-%20API%20Contract%20and%20AI%20Self-Criticism.md)
- [06 - Phase 5 - System Architecture and Diagramming](../../Code%20Examples/Day%2015%20-%20AI-Orchestrated%20SDLC/06%20-%20Phase%205%20-%20System%20Architecture%20and%20Diagramming.md)
- [07 - Phase 6 - Security and Resilience](../../Code%20Examples/Day%2015%20-%20AI-Orchestrated%20SDLC/07%20-%20Phase%206%20-%20Security%20and%20Resilience.md)
- [08 - Phase 7 - Containerization with Podman and Docker](../../Code%20Examples/Day%2015%20-%20AI-Orchestrated%20SDLC/08%20-%20Phase%207%20-%20Containerization%20with%20Podman%20and%20Docker.md)

### Part 2: CI-CD, DevOps, json-server and Networking
- [01 - CI-CD Pipeline](../../Code%20Examples/Day%2015%20-%20CI-CD,%20DevOps,%20json-server%20and%20Networking/01%20-%20CI-CD%20Pipeline.md)
- [02 - Azure DevOps](../../Code%20Examples/Day%2015%20-%20CI-CD,%20DevOps,%20json-server%20and%20Networking/02%20-%20Azure%20DevOps.md)
- [03 - json-server](../../Code%20Examples/Day%2015%20-%20CI-CD,%20DevOps,%20json-server%20and%20Networking/03%20-%20json-server.md)
- [04 - Load Balancer vs Traffic Manager](../../Code%20Examples/Day%2015%20-%20CI-CD,%20DevOps,%20json-server%20and%20Networking/04%20-%20Load%20Balancer%20vs%20Traffic%20Manager.md)
- [05 - CORS](../../Code%20Examples/Day%2015%20-%20CI-CD,%20DevOps,%20json-server%20and%20Networking/05%20-%20CORS.md)
