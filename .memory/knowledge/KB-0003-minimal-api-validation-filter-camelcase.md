---
id: KB-0003
title: Minimal API Validation Filter with CamelCase RFC 7807 Serialization
component: api-validation
domain: api-design
tags:
- minimal-apis
- fluentvalidation
- rfc7807
- camelcase
status: validated
created: '2026-03-30'
updated: '2026-03-30'
depends_on:
- decisions/ADR-0003-endpoint-filter-validation-pattern.md
- decisions/DD-0001-api-schema-and-tone-standards.md
supported_by: []
---

## What Happened

Minimal APIs in .NET do not perform automatic model validation. FluentValidation property names use PascalCase by default. Returning these names directly creates non-standard JSON error payloads.

## Key points

Implement a generic `IEndpointFilter` for request validation.
Convert all validation error property names to camelCase using `JsonNamingPolicy.CamelCase`.
Return standard RFC 7807 problem details payloads with HTTP status code 400.

## Why It Matters

Inconsistent naming breaks client JSON parsers.
Consistent camelCase property names ensure smooth integration across all client libraries.

## Applies When

Implement input validation filters for ASP.NET Core Minimal API endpoints.
