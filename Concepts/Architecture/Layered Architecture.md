# Layered Architecture

Layered architecture separates the concerns of an application into logical tiers or layers (e.g., Presentation, Service/Business Logic, Data Access). This prevents monolithic, tightly coupled code that is difficult to maintain or test.

## Monolithic Approach (The Problem)
When all logic (UI, validation, business rules, data access) is stuffed into a single method (like `Main`), the code becomes a **monolithic script**. 
- It is hard to read.
- It is impossible to unit test individual parts.
- See [Bad_Program.cs](../../Code%20Examples/Day%204%20-%20Layered%20Architecture/Bad_Program.cs) for an example of this anti-pattern.

## Layered Approach (The Solution)
By breaking the code down, we achieve a separation of concerns:
1. **Models / Entities**: Define the structure of the data (e.g., [Course.cs](../../Code%20Examples/Day%204%20-%20Layered%20Architecture/Course.cs), [Student.cs](../../Code%20Examples/Day%204%20-%20Layered%20Architecture/Student.cs), [Admission.cs](../../Code%20Examples/Day%204%20-%20Layered%20Architecture/Admission.cs)).
2. **Data Transfer Objects (DTOs)**: Define the shape of data coming in or going out (e.g., [AdmissionRequest.cs](../../Code%20Examples/Day%204%20-%20Layered%20Architecture/AdmissionRequest.cs)).
3. **Repositories**: Handle data storage and retrieval (e.g., [CourseRepository.cs](../../Code%20Examples/Day%204%20-%20Layered%20Architecture/CourseRepository.cs), [AdmissionRepository.cs](../../Code%20Examples/Day%204%20-%20Layered%20Architecture/AdmissionRepository.cs)).
4. **Services**: Handle the core business logic and coordinate between validators, repositories, and other services (e.g., [AdmissionService.cs](../../Code%20Examples/Day%204%20-%20Layered%20Architecture/AdmissionService.cs)).

By following a "Model-first approach", you define your domain entities and then build the repositories and services around them.
