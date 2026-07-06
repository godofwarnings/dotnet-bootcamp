# Database Design Best Practices

*(The following practices align with the [Hospital Booking System](../Architecture/System%20Design%20-%20Hospital.md) designed in Day 5).*

## Status Fields
- Use **Enums** for status values (like `Booked`, `Cancelled`, `Completed`) instead of free text to prevent typos, ensure standardization, and make querying easier.
- **Context:** In the Hospital system, the `status` field of the `Appointment` table is an Enum, ensuring patients can't accidentally set their status to "Canceled" (misspelled).

## Deletions
- Do not delete any entity permanently (Hard Delete) as it breaks the traceability of system actions. 
- **Context:** If a Doctor leaves the hospital, deleting them from the `Doctor` table would cascade and delete all their historical `Appointment` records (or orphan them). 
- Instead, use a flag like `IsActive` or `IsDeleted` (Soft Delete). The Search Service can easily filter out doctors where `active_status = false` without losing their historical billing data.

## Constraints
- Prevent issues like double-booking by pushing rules to the database. Use **unique constraints** (e.g., `UNIQUE(doctor_id, slot_start)`) as the final safety net to guarantee data integrity, rather than relying solely on Application-level checks.
- **Context:** While the Slot Service checks if a slot is available before making the Booking request, race conditions can happen. The Database unique constraint ensures that even if two requests hit the DB at the exact same millisecond, only one transaction commits.
