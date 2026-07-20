# 02 - Phase 1 - Project initialization & AI-driven scaffolding
 
The app is an end-to-end integration of a **.NET Core Web API** backend with an **Angular** frontend.
 
**Approach:** prompt the AI to produce a **step-by-step build plan** (in the demo, ~37 steps), starting with the Web API before the Angular app. The generated blueprint covers:
 
- Creating the .NET Core Web API project from the template.
- Configuring **routing**, **dependency injection**, and the **middleware** pipeline.
- Enabling **Swagger / OpenAPI** UI for interactive API discovery.
- Writing initial controllers and models, and exercising them with **Postman**.
 
> [!TIP] Make the AI plan *before* it codes
> Asking for the ordered step list first (rather than "build me the app") gives you a reviewable blueprint. You can correct the sequence or scope *before* any code exists, which is far cheaper than reworking generated code.
