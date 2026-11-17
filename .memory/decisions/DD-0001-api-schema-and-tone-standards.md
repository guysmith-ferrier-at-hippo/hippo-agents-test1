---
id: DD-0001
title: API Schema Standards and Content Tone Strategy
component: customer-greetings-api
domain: design-guidelines
tags:
- api-design
- content-strategy
- openapi
- rfc7807
status: accepted
created: '2026-03-30'
updated: '2026-03-30'
---

# DD-0001: API Schema Standards and Content Tone Strategy

## Context
The Customer Greetings Service is a backend API that generates user-facing content (customer greetings) and exposes developer-facing interactive documentation (OpenAPI / Swagger UI). We must define clear constraints for API payload naming conventions, error structures, content tone specifications, and developer documentation styling.

## Options Considered
1. **camelCase JSON with RFC 7807 ProblemDetails and Multi-Tone Content Rules** — Standardizes all API contracts to idiomatic REST guidelines, provides clear machine-readable error responses, and defines explicit tone definitions (formal, friendly, casual).
2. **snake_case JSON with custom error payloads** — Non-standard in modern .NET ecosystems, increases integration friction with client libraries.

## Evaluation
Standard camelCase naming aligns directly with modern JavaScript/TypeScript frontends and standard System.Text.Json defaults. RFC 7807 provides predictable error handling across all client integrations.

## Decision
Adopt camelCase JSON property naming, RFC 7807 `ProblemDetails` for all error responses, OpenAPI 3.0 / Swagger UI documentation, and three clear greeting tone profiles (Formal, Friendly, Casual).

## Consequences
- **Positive:** Guarantees interoperability across diverse client frameworks, simplifies error parsing, and ensures consistent greeting voice across applications.
- **Negative:** Requires strict validation to ensure custom error objects do not leak internal stack traces.