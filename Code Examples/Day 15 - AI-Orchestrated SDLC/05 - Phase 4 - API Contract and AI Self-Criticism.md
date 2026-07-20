# 05 - Phase 4 - API contract definition & AI self-criticism
 
Define the **API contract first** so frontend and backend teams can work in parallel against a shared shape.
 
```json
// Leave Request DTO — initial draft
{
  "employeeId": "emp-1042",
  "employeeName": "John Doe",
  "fromDate": "2026-07-20",
  "toDate": "2026-07-21",
  "leaveType": "Sick"
}
```
 
### Running the AI as a critic
 
Prompt the AI to **critique its own contract** for security gaps, missing constraints, and architectural flaws. In the demo the design scored **8.5/10** for basic design but only **6.5/10** for production-grade resilience. Key findings and fixes:
 
- **Missing schema constraints** — no `required` fields or string-length limits declared. → Add explicit `[Required]`, `[StringLength]` / OpenAPI constraints.
- **Nullable date types** — a non-nullable `DateTime` **defaults to `0001-01-01`** when omitted, silently passing "not null" checks. → Use **`DateTime?`** so an omitted date is genuinely `null` and fails validation.
- **Loose enums** — `LeaveType` (Sick/Casual/WFH) and status (Pending/Approved/Rejected) accepted as free-form strings. → Model them as strict **C# `enum`s** so arbitrary values are rejected at binding time.
 
```csharp
// Hardened DTO after the critique
public class LeaveRequestDto
{
    [Required]
    public string EmployeeId { get; set; } = default!;
 
    [Required, MinLength(1)]
    public string EmployeeName { get; set; } = default!;   // combine with IsNullOrWhiteSpace check
 
    [Required]
    public DateTime? FromDate { get; set; }   // nullable -> omission is caught, not defaulted to 0001-01-01
 
    [Required]
    public DateTime? ToDate { get; set; }
 
    [Required, EnumDataType(typeof(LeaveType))]
    public LeaveType LeaveType { get; set; }
}
 
public enum LeaveType { Sick, Casual, WorkFromHome }
```
 
> [!WARNING] The `DateTime` default trap
> `DateTime` is a **value type**, so an omitted JSON field binds to `DateTime.MinValue` (`0001-01-01T00:00:00`) — *not* `null`. A naive "is it set?" check passes and the bad record slips through. Making the property **`DateTime?`** (nullable) is what lets `[Required]` / null checks actually fire.
 
> [!TIP] Enum vs. string for closed sets
> A `LeaveType` string accepts `"Banana"`; a `LeaveType` **enum** does not — invalid values fail model binding before your code runs. Prefer enums (or a validated allow-list) for any field with a fixed, known set of values.
