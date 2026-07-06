# EF Core Data Annotations

Data Annotations in Entity Framework Core are attributes placed on model properties to configure the database schema, add validation rules, and influence how ASP.NET Core MVC generates UI elements (Scaffolding).

## Common Annotations

- **`[Key]`**: By default, EF Core assumes any property named `Id` (or `<ClassName>Id`) is the Primary Key. If your primary key is named differently (e.g., `Identifier`), you must place `[Key]` above it.
- **`[DataType(DataType.Date)]`**: Tells EF Core and the MVC framework that this property represents a Date (without time). 
  - Helps the UI render a date picker input.
  - Tells scaffolding templates to ignore the time portion.
- **`[Required]`**: Enforces that the property cannot be null. In the database, it generates a `NOT NULL` column constraint.
- **`[Display(Name = "Product Name")]`**: Changes the label displayed in the UI when scaffolding views.
- **`[Range(1, 1000)]`**: Adds validation ensuring numeric inputs fall within the specified bounds.
- **`[ScaffoldColumn(false)]`**: Tells the ASP.NET Core MVC scaffolding engine to skip generating UI input fields for this property (often used for navigation properties or internal tracking fields).
