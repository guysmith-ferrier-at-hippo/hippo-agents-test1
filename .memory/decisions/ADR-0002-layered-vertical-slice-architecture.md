---
id: ADR-0002
title: Adopt Layered Architecture with Vertical Feature Slices
component: customer-greetings-api
domain: architecture
tags:
- architecture
- clean-architecture
- layering
- dotnet
status: accepted
created: '2026-03-30'
updated: '2026-03-30'
---

# ADR-0002: Adopt Layered Architecture with Vertical Slices

## Context
The service must maintain high readability, testability, and clear separation of concerns. Business rules for greeting generation (time-of-day calculations, localization formatting, and tone management) must remain isolated from HTTP transport mechanisms and external integrations.

## Options Considered
1. **Layered Architecture with Feature Slices** — Clear separation between API presentation, core domain logic, and infrastructure/localization adapters, grouped by capability.
2. **Single-Project Monolithic Scripting** — All logic embedded in endpoint lambdas in one file; quick to set up but difficult to test and maintain as rules grow.
3. **Full Hexagonal / Clean Architecture with heavy MediatR CQRS** — Strong decoupling, but introduces excessive boilerplate for a lightweight stateless greeting service.

## Decision
We adopt a **Layered Architecture with Vertical Feature Slices** using three clean projects:
1. `CustomerGreetings.Api` (Presentation / Minimal API endpoints / OpenAPI)
2. `CustomerGreetings.Core` (Domain logic, greeting models, localization rules, validators)
3. `CustomerGreetings.Infrastructure` (Resource localization providers, system clock adapters, caching)

## Consequences
- **Positive:** Isolates greeting logic from HTTP concerns, enables pure domain unit testing without HTTP mocks, and prevents architectural drift.
- **Negative:** Requires multi-project solution structure and explicit dependency injection wiring.