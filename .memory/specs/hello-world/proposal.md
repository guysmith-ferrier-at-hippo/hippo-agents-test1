---
title: "Implementation Proposal: Hello World Greeting"
component: "customer-greetings-api"
domain: "api-design"
tags:
  - "hello-world"
  - "minimal-apis"
  - "rest"
  - "greeting"
created: "2026-03-30"
updated: "2026-03-30"
depends-on:
  - "specs/hello-world/feature.md"
  - "decisions/ADR-0001-use-dotnet-minimal-apis.md"
  - "decisions/ADR-0002-layered-vertical-slice-architecture.md"
  - "decisions/ADR-0003-endpoint-filter-validation-pattern.md"
  - "decisions/DD-0001-api-schema-and-tone-standards.md"
supported-by:
  - "specs/hello-world/intent.md"
  - "specs/hello-world/tasks.md"
  - "specs/hello-world/test-cases.md"
  - "specs/hello-world/security.md"
---

# Implementation Proposal: Hello World Greeting

## Intent

Client development teams need a simple, reliable endpoint to verify network connectivity and JSON response parsing. This feature delivers a lightweight greeting endpoint that returns an immediate JSON response without querying external databases.

## Scope

### In-Scope

- Expose `GET /api/v1/hello` using ASP.NET Core Minimal APIs.
- Deliver default greeting payload `{"greeting": "Hello, World!", "generatedAt": "..."}` when caller omits recipient parameter.
- Deliver personalized greeting payload `{"greeting": "Hello, {name}!", "generatedAt": "..."}` when caller provides a valid recipient name.
- Fall back to default greeting if recipient parameter contains only empty characters or blank spaces.
- Enforce maximum recipient name length of 50 characters using FluentValidation.
- Return standard RFC 7807 `ProblemDetails` error response with HTTP 400 status on invalid inputs.
- Use .NET `TimeProvider` abstraction to generate deterministic ISO-8601 UTC timestamps.
- Register endpoint metadata and contracts in OpenAPI documentation.

### Out-of-Scope

- Multi-language localization and language negotiation (deferred to the contextual greetings feature).
- Caller authentication validation for smoke testing (permits anonymous access per ASSUME-0006).
- Database persistence, cache storage, or external infrastructure queries.
- Customer-facing frontend user interface screens (backend API and OpenAPI documentation only).

## Knowledge Context

### Assumptions

- [ASSUME-0001: Stateless Customer Context Ingestion](../../assumptions/ASSUME-0001-stateless-greeting-architecture.md)
- [ASSUME-0002: Upstream Gateway Handles Authentication and Ingress Security](../../assumptions/ASSUME-0002-upstream-gateway-auth.md)
- [ASSUME-0003: Deterministic Time Simulation Using FakeTimeProvider](../../assumptions/ASSUME-0003-deterministic-time-testing.md)
- [ASSUME-0004: Hello-World Endpoint Accelerates Client Onboarding and Verification](../../assumptions/ASSUME-0004-hello-world-developer-onboarding.md)
- [ASSUME-0005: Hello-World Greeting Uses Standard Minimal Payload](../../assumptions/ASSUME-0005-hello-world-stateless-payload.md)
- [ASSUME-0006: Hello-World Endpoint Permits Anonymous Access](../../assumptions/ASSUME-0006-hello-world-anonymous-access.md)
- [ASSUME-0007: Hello-World Endpoint Operates Without Internationalization](../../assumptions/ASSUME-0007-hello-world-english-only-phase1.md)

### Architectural Decisions (ADRs)

- [ADR-0001: Use .NET 9 ASP.NET Core Minimal APIs](../../decisions/ADR-0001-use-dotnet-minimal-apis.md)
- [ADR-0002: Adopt Layered Architecture with Vertical Feature Slices](../../decisions/ADR-0002-layered-vertical-slice-architecture.md)
- [ADR-0003: Endpoint Filter Validation Pattern for Minimal APIs](../../decisions/ADR-0003-endpoint-filter-validation-pattern.md)
- [DD-0001: API Schema Standards and Content Tone Strategy](../../decisions/DD-0001-api-schema-and-tone-standards.md)

## Approach

The implementation follows the layered architecture with vertical slices established in ADR-0002.

1. **Domain & Core (`CustomerGreetings.Core`):**
   - Define `HelloWorldResponse` (new) DTO in `src/CustomerGreetings.Core/Models/HelloWorldResponse.cs` (new).
   - Define `HelloWorldRequest` (new) parameter model in `src/CustomerGreetings.Core/Models/HelloWorldRequest.cs` (new).
   - Implement `HelloWorldRequestValidator` (new) using FluentValidation in `src/CustomerGreetings.Core/Validators/HelloWorldRequestValidator.cs` (new).
   - Define `IHelloWorldService` (new) interface in `src/CustomerGreetings.Core/Services/IHelloWorldService.cs` (new).
   - Implement `HelloWorldService` (new) in `src/CustomerGreetings.Core/Services/HelloWorldService.cs` (new) with injected `TimeProvider`.

2. **Presentation & HTTP (`CustomerGreetings.Api`):**
   - Create generic `ValidationFilter<T>` (new) in `src/CustomerGreetings.Api/Filters/ValidationFilter.cs` (new) adhering to ADR-0003.
   - Map route `GET /api/v1/hello` in `src/CustomerGreetings.Api/Endpoints/HelloWorldEndpoints.cs` (new).
   - Wire dependency injection and OpenAPI annotations in `src/CustomerGreetings.Api/Program.cs` (new).

3. **PR Deployment Strategy:**
   - **PR 1 (Domain Core & Unit Tests):** Implement models, validator, service, and domain unit tests in `tests/CustomerGreetings.Core.UnitTests/Services/HelloWorldServiceTests.cs` (new) and `tests/CustomerGreetings.Core.UnitTests/Validators/HelloWorldRequestValidatorTests.cs` (new).
   - **PR 2 (API Slice & Integration Tests):** Implement validation filter, endpoint mapping, OpenAPI metadata, and HTTP integration tests in `tests/CustomerGreetings.Api.IntegrationTests/Endpoints/HelloWorldEndpointTests.cs` (new).
