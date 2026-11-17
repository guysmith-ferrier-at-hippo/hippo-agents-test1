---
id: ADR-0001
title: Use .NET 9 ASP.NET Core Minimal APIs
component: customer-greetings-api
domain: architecture
tags:
- dotnet
- csharp
- minimal-apis
- rest
status: accepted
created: '2026-03-30'
updated: '2026-03-30'
---

# ADR-0001: Use .NET 9 ASP.NET Core Minimal APIs

## Context
The Customer Greetings Service requires high throughput, minimal overhead, low memory consumption, and straightforward endpoint definitions. We must choose an application framework and programming language that provides high performance and long-term support.

## Options Considered
1. **.NET 9 ASP.NET Core Minimal APIs** — Lightweight HTTP pipeline, native AOT support options, low memory footprint, and strong performance.
2. **.NET 9 Controller-based Web API** — Traditional MVC controller structure with heavier reflection overhead.
3. **Node.js with Express/Fastify** — Rapid JavaScript setup, but does not match existing repository .NET configuration (`.gitignore`).

## Decision
We select **.NET 9 ASP.NET Core Minimal APIs** for the Customer Greetings Service.

## Consequences
- **Positive:** Reduces boilerplate code, accelerates execution speed, decreases memory utilization, and integrates smoothly with standard .NET dependency injection and OpenAPI generators.
- **Negative:** Requires team familiarity with Minimal API endpoint mapping patterns instead of traditional controller classes.