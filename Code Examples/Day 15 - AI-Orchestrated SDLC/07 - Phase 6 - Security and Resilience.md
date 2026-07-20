# 07 - Phase 6 - Security checklist & resilience validation
 
### Security checklist
 
- **Authentication & Authorization** — enforce access control on **every** endpoint (no anonymous gaps).
- **XSS protection** — sanitize/encode inputs on both Angular forms and the API payload layer.
- **Rate limiting** — cap request rates to blunt brute-force and DoS attempts.
- **Sign-off / audit** — an **immutable audit log** recording who approved/rejected each leave request.
 
### Resilience targets
 
| Metric | Target | Meaning |
| --- | --- | --- |
| **Availability** | **99.5%** uptime / month | Allowed downtime budget |
| **RTO** (Recovery Time Objective) | **≤ 2 hours** | Max time to *restore service* after failure |
| **RPO** (Recovery Point Objective) | **≤ 15 minutes** | Max acceptable *data loss* window |
| **Disaster Recovery** | Active/passive across regions (e.g. Primary US, Secondary Japan) | Survive a localized regional outage |
 
- **Idempotency keys** — attach a unique idempotency header to POST requests so a double-click ("Submit" twice) does **not** create duplicate records.
 
> [!TIP] RTO vs. RPO — don't confuse them
> **RTO = time** (how fast you're back up). **RPO = data** (how much you can afford to lose). A 15-minute RPO implies backups/replication at least every 15 minutes; a 2-hour RTO constrains how your recovery procedure and infrastructure are designed.
 
### Performance & load testing
 
- **Latency benchmark** — 95% of standard requests return within **500 ms** (a p95 target).
- **Load testing** — exercise the system at **3× projected concurrent users** to find breaking points.
- **Spike testing** — simulate sudden bursts (e.g. the 9:00 AM Monday login rush) to confirm graceful handling of traffic spikes.
 
> [!NOTE] Load vs. spike vs. stress
> **Load** = sustained expected/high volume. **Spike** = sudden sharp burst then drop. **Stress** = push past limits to see how it fails (and whether it recovers). They answer different questions; run all three for a critical system.
