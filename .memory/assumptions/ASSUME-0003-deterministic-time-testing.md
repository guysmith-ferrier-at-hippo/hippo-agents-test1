---
id: ASSUME-0003
title: Deterministic Time Simulation Using FakeTimeProvider
component: customer-greetings-core
domain: testing
tags:
- testing
- timeprovider
- determinism
status: validated
created: '2026-03-30'
updated: '2026-03-30'
---

### Assumption Description
All automated unit and integration tests simulate temporal variations (morning, afternoon, evening, night) by injecting synthetic clock implementations (`FakeTimeProvider` from `Microsoft.Extensions.Time.Testing`) rather than manipulating system clocks or waiting for real-time passage.

### Rationale
Simulating time intervals via `TimeProvider` eliminates non-deterministic test race conditions, removes execution delay, and allows instant testing of complex boundary conditions.

### Impact if Invalid
If tests rely on `DateTime.UtcNow` or system clock manipulation directly, test suites will fail intermittently when executed near boundary minutes or across international CI runners.

### Validation Method
Enforce static analysis or architectural unit tests that forbid direct calls to `DateTime.Now` or `DateTime.UtcNow` in `CustomerGreetings.Core`.