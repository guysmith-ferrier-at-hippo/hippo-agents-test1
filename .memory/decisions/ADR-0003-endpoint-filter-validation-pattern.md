---
id: ADR-0003
title: Endpoint Filter Validation Pattern for Minimal APIs
component: customer-greetings-api
domain: architecture
tags:
- minimal-apis
- fluentvalidation
- validation
- rfc7807
status: accepted
created: '2026-03-30'
updated: '2026-03-30'
depends_on:
- decisions/ADR-0001-use-dotnet-minimal-apis.md
- decisions/ADR-0002-layered-vertical-slice-architecture.md
supported_by:
- specs/hello-world/proposal.md
---

# ADR-0003: Endpoint Filter Validation Pattern for Minimal APIs

**Feature:** hello-world  
**Date:** 2026-03-30  
**Deciders:** Lead Architect, Hippo Core Services Team  
**Status:** accepted  

## Context

Minimal APIs in .NET 9 do not perform automatic model validation. Each endpoint must validate input parameters consistently. Handlers must not contain repetitive validation code. The service must format all validation errors as standard RFC 7807 problem details payloads.

## Options Considered

* **Option 1:** Minimal API Endpoint Filters with FluentValidation
* **Option 2:** Manual Validation in Endpoint Handlers
* **Option 3:** MediatR Pipeline Behaviors

## Evaluation

### Option 1: Minimal API Endpoint Filters with FluentValidation
* **Pros:**
    * Intercepts HTTP requests before endpoint handlers execute.
    * Formats RFC 7807 validation problem details automatically.
    * Keeps endpoint delegates concise and readable.
* **Cons:**
    * Requires registering filter components on endpoint definitions.

### Option 2: Manual Validation in Endpoint Handlers
* **Pros:**
    * Requires no additional pipeline abstractions.
* **Cons:**
    * Duplicates validation checks across every endpoint.
    * Increases risk of inconsistent error payload formats.

### Option 3: MediatR Pipeline Behaviors
* **Pros:**
    * Centralizes request pipeline execution.
* **Cons:**
    * Adds unnecessary architectural complexity and external dependencies.

## Decision

We adopt ASP.NET Core Minimal API endpoint filters combined with FluentValidation. The filter intercepts requests, executes registered validators, and returns standard RFC 7807 problem details payloads when validation fails.

## Consequences

### Positive
* Standardizes RFC 7807 problem details across all HTTP endpoints.
* Decouples validation logic from endpoint route handlers.
* Improves unit and integration testability.

### Negative
* Developers must register endpoint filters on route groups or endpoints.
