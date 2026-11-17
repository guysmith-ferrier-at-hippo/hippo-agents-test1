---
id: KB-0001
title: Standard Time Boundaries for Contextual Greetings
component: customer-greetings-core
domain: domain-rules
tags:
- time-boundaries
- greetings
- domain-rules
status: validated
created: '2026-03-30'
updated: '2026-03-30'
---

### Overview
Time-of-day greeting classification requires defined standard operational intervals to maintain consistent output across international locales:
- **Morning:** `05:00:00` to `11:59:59`
- **Afternoon:** `12:00:00` to `16:59:59`
- **Evening:** `17:00:00` to `21:59:59`
- **General / Night:** `22:00:00` to `04:59:59` (defaults to neutral greeting or standard "Hello" to avoid intrusive nighttime messaging unless explicitly requested)

### Why It Matters
Ambiguity in temporal boundaries creates erratic test results and jarring customer experiences across time zone conversions. Establishing standard intervals guarantees predictable unit test assertions and consistent user perceptions.

### Key Rules & Applicable Scope
1. Use `TimeProvider` (.NET 8/9 standard) to abstract system time during greeting computation.
2. All time computations must evaluate against the customer's specified local time or offset.
3. When time is omitted, the service falls back to a neutral salutation (e.g., "Welcome", "Hello").