# Database Design Best Practices

## Status Fields
- Use Enums for status values (like `Booked`, `Cancelled`) instead of free text to prevent typos, ensure standardisation, and make querying easier.

## Deletions
- Do not delete any entity permanently (Hard Delete). 
- It breaks the traceability of the system actions. For example, doctors might be removed from the UI, but they can still be kept in the database to allow for historical consistency and auditing. 
- Instead, use a flag like `IsActive` or `IsDeleted` (Soft Delete).

## Constraints
- Prevent issues like double booking by pushing rules to the database. Use unique constraints (e.g., `(doctor_id, slot_start)`) as the final safety net to guarantee data integrity, rather than relying solely on Application-level checks.
