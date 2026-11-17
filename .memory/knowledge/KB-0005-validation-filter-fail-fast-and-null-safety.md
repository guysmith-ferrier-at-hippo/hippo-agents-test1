---
id: KB-0005
title: Endpoint Validation Filter Fail-Fast Resolution and Property Name Null Safety
component: api-validation
domain: api-design
tags:
- minimal-apis
- validation-filter
- fail-fast
- null-safety
- fluentvalidation
status: validated
created: '2026-03-30'
updated: '2026-03-30'
depends_on:
- decisions/ADR-0003-endpoint-filter-validation-pattern.md
- knowledge/KB-0003-minimal-api-validation-filter-camelcase.md
supported_by: []
---

## What Happened

During code review, the generic validation filter used `GetService` instead of `GetRequiredService`. If a validator was missing from dependency injection, the filter silently permitted unvalidated requests to reach domain handlers. In addition, `ConvertName` failed with an unhandled exception when validation errors contained null or empty property names.

## Key points

Always resolve required validators with `GetRequiredService<IValidator<T>>()`.
This causes an immediate configuration exception when a validator registration is missing.
Guard property names for null or empty values before name conversion.
Use empty string fallback for model-level validation errors.
Add comprehensive filter tests to verify fail-fast behavior and null handling.

## Why It Matters

Silent validation bypass permits invalid customer data to reach business logic.
Unhandled null property exceptions trigger HTTP 500 errors instead of client-actionable HTTP 400 responses.
Failing fast at development time prevents silent security and data integrity defects in production.

## Applies When

Implement generic endpoint validation filters in ASP.NET Core Minimal APIs with FluentValidation.
