---
title: "Tasks: Hello World Greeting"
component: "customer-greetings-api"
domain: "api-design"
tags:
  - "hello-world"
  - "minimal-apis"
  - "implementation-tasks"
created: "2026-03-30"
updated: "2026-03-30"
depends-on:
  - "specs/hello-world/intent.md"
supported-by:
  - "specs/hello-world/proposal.md"
---

## 1. Domain Models and Core Business Logic (PR 1)

- [ ] 1.1 Create `HelloWorldRequest` record in `src/CustomerGreetings.Core/Models/HelloWorldRequest.cs` (new).
- [ ] 1.2 Create `HelloWorldResponse` record in `src/CustomerGreetings.Core/Models/HelloWorldResponse.cs` (new).
- [ ] 1.3 Create `HelloWorldRequestValidator` using FluentValidation in `src/CustomerGreetings.Core/Validators/HelloWorldRequestValidator.cs` (new).
- [ ] 1.4 Define `IHelloWorldService` interface in `src/CustomerGreetings.Core/Services/IHelloWorldService.cs` (new).
- [ ] 1.5 Implement `HelloWorldService` using `TimeProvider` in `src/CustomerGreetings.Core/Services/HelloWorldService.cs` (new).

## 2. Core Unit Tests and Coverage Verification (PR 1)

- [ ] 2.1 Implement unit tests for `HelloWorldRequestValidator` in `tests/CustomerGreetings.Core.UnitTests/Validators/HelloWorldRequestValidatorTests.cs` (new) using `write-tests`.
- [ ] 2.2 Implement unit tests for `HelloWorldService` in `tests/CustomerGreetings.Core.UnitTests/Services/HelloWorldServiceTests.cs` (new) using `write-tests`.
- [ ] 2.3 Execute `dotnet test` to verify all domain unit tests pass with minimum 85% branch coverage.

## 3. API Presentation Layer and Validation Filter (PR 2)

- [ ] 3.1 Implement generic `ValidationFilter<T>` in `src/CustomerGreetings.Api/Filters/ValidationFilter.cs` (new) per ADR-0003.
- [ ] 3.2 Create `HelloWorldEndpoints` mapping `GET /api/v1/hello` in `src/CustomerGreetings.Api/Endpoints/HelloWorldEndpoints.cs` (new).
- [ ] 3.3 Register `IHelloWorldService`, validator, and endpoint mappings in `src/CustomerGreetings.Api/Program.cs` (new).
- [ ] 3.4 Configure OpenAPI metadata and response contracts in `src/CustomerGreetings.Api/Program.cs` (new).

## 4. API Integration Tests and End-to-End Verification (PR 2)

- [ ] 4.1 Implement integration tests for `GET /api/v1/hello` in `tests/CustomerGreetings.Api.IntegrationTests/Endpoints/HelloWorldEndpointTests.cs` (new) using `write-tests`.
- [ ] 4.2 Verify behavioral test scenarios TC-01 through TC-09 in integration test suite.
- [ ] 4.3 Verify OpenAPI contract conformance for scenario TC-11 in integration test suite.
- [ ] 4.4 Execute complete solution test run and verify zero compiler or linter warnings.
