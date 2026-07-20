# 06 - Phase 5 - System architecture & visual diagramming
 
### The architecture document
 
A high-level architecture document captures:
 
1. **System boundaries** — what the Leave Management System interacts with (identity provider, email, DB).
2. **Major components** — Angular SPA · Web API gateway · core domain logic · infrastructure/EF Core · SQL Server.
3. **Cross-cutting concerns** — logging, performance, security, caching, and **internationalization**.
 
> [!NOTE] What "i18n" means
> **i18n** = **internationalization**: the letter **i**, then **18** letters, then **n**. (Similarly **l10n** = *localization*, **a11y** = *accessibility*.) It is the practice of building the app so it can be adapted to different languages/locales without code changes.
 
### Diagramming with AI
 
Diagrams that used to be hand-drawn in Visio or draw.io can now be **generated as text** the AI emits and tools render — **Mermaid**, PlantUML, or structured JSON — making them versionable and quick to regenerate as the design changes.
