---
title: "Intent: Hello World Greeting"
feature: "hello-world"
created: "2026-03-30"
updated: "2026-03-30"
depends-on:
  - "specs/hello-world/proposal.md"
supported-by:
  - "specs/hello-world/tasks.md"
---

# Intent: Hello World Greeting

## Objective

Deliver an immediate, reliable JSON greeting endpoint (`GET /api/v1/hello`). Internal client developers and automated health monitors use this endpoint to verify network ingress and response payload parsing without configuring database connections or authentication tokens.

## Boundaries & Scope Constraints

- **Untouchable:**
  - Localization resource files in `src/CustomerGreetings.Infrastructure/Resources/` (new) must remain isolated from this feature slice.
  - Core contextual greeting logic in `src/CustomerGreetings.Core/Services/GreetingService.cs` (new) must not be modified or coupled.
- **Scope Limits:**
  - MUST NOT implement any customer-facing UI screens or views.
  - MUST NOT introduce database tables, migrations, or persistent caching stores.
  - MUST NOT introduce language negotiation or localization dictionaries in Phase 1.

## Technical Constraints

- **Dependencies:**
  - Runtime: .NET 9 LTS with C# 13.
  - Web framework: ASP.NET Core Minimal APIs.
  - Validation: FluentValidation 11.x.
  - Time abstraction: Microsoft .NET `TimeProvider`.
- **Performance:**
  - Endpoint latency must remain under 25 milliseconds under standard load.
  - Endpoint executes purely in-memory with zero disk or database I/O.
- **Compatibility:**
  - JSON payload formatting must strictly follow camelCase conventions per DD-0001.
  - Error responses must strictly conform to RFC 7807 `ProblemDetails`.

## Data Definitions & Schemas

### Database Schema Changes

- **New Tables / Collections:** None. The feature slice operates statelessly.
- **Modified Tables / Collections:** None.
- **Data Constraints:** None.

### Code Data Structures

- **Domain Models & Entities:**
  - None required. Feature slice operates purely on stateless transfer objects.
- **DTOs (Data Transfer Objects):**
  - `HelloWorldRequest` in `src/CustomerGreetings.Core/Models/HelloWorldRequest.cs` (new):
    ```csharp
    public record HelloWorldRequest(string? Name);
    ```
  - `HelloWorldResponse` in `src/CustomerGreetings.Core/Models/HelloWorldResponse.cs` (new):
    ```csharp
    public record HelloWorldResponse(string Greeting, DateTimeOffset GeneratedAt);
    ```
- **Internal Data Flows & State Objects:**
  - `IHelloWorldService` in `src/CustomerGreetings.Core/Services/IHelloWorldService.cs` (new):
    ```csharp
    public interface IHelloWorldService
    {
        HelloWorldResponse GetGreeting(string? name = null);
    }
    ```
  - `HelloWorldService` in `src/CustomerGreetings.Core/Services/HelloWorldService.cs` (new):
    - Injects `TimeProvider` to obtain current UTC time.
    - Evaluates input recipient string with `string.IsNullOrWhiteSpace(name)`.
    - Returns salutation formatted as `"Hello, World!"` or `$"Hello, {name.Trim()}!"`.
  - `HelloWorldRequestValidator` in `src/CustomerGreetings.Core/Validators/HelloWorldRequestValidator.cs` (new):
    - Inherits `AbstractValidator<HelloWorldRequest>`.
    - Enforces maximum name length of 50 characters.
- **Type/Schema Verification:**
  - Validate response contracts against OpenAPI specifications generated in `src/CustomerGreetings.Api/Program.cs` (new).

## API & Integration Contracts

### Endpoints (REST / RPC / GraphQL)

- **Endpoint Definitions:**
  | Method | Path / Operation | Description | Changes data | Auth Required |
  | :--- | :--- | :--- | :--- | :--- |
  | `GET` | `/api/v1/hello` | Retrieve default or personalized greeting | No | No (ASSUME-0006) |
- **Rule:** A `GET` never changes data. Use `POST`, `PUT`, `PATCH` or `DELETE` for anything that changes data or records an action.
- **Headers & Query Parameters:**
  - Query Parameter: `name` (optional, string, maximum length 50 characters).
  - Request Header: `Accept: application/json`.
  - Response Header: `Content-Type: application/json` or `Content-Type: application/problem+json`.

### Payload Contracts

- **Request Payloads:**
  - None. Endpoint accepts parameters exclusively via URL query string.
- **Response Payloads:**
  - HTTP 200 OK (Default Greeting):
    ```json
    {
      "greeting": "Hello, World!",
      "generatedAt": "2026-03-30T12:00:00Z"
    }
    ```
  - HTTP 200 OK (Personalized Greeting with `?name=Alice`):
    ```json
    {
      "greeting": "Hello, Alice!",
      "generatedAt": "2026-03-30T12:00:00Z"
    }
    ```
- **Error Responses:**
  - HTTP 400 Bad Request (Validation failure when `name` length > 50):
    ```json
    {
      "type": "https://tools.ietf.org/html/rfc7807",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "detail": "The recipient name must not exceed 50 characters.",
      "instance": "/api/v1/hello",
      "errors": {
        "name": [
          "The recipient name must not exceed 50 characters."
        ]
      }
    }
    ```

### Webhooks & Messaging

- **Publish/Subscribe Event Schemas:** None. Synchronous HTTP query only.
- **Third-Party Integrations:** None.

## Validation Rules

### Test Sets (Input/Output Rulesets)

- **Test Set 1:** Given request with no parameters, system returns HTTP 200 with `greeting="Hello, World!"`.
- **Test Set 2:** Given `name="Alice"`, system returns HTTP 200 with `greeting="Hello, Alice!"`.
- **Test Set 3:** Given empty parameter `name=""`, system returns HTTP 200 with `greeting="Hello, World!"`.
- **Test Set 4:** Given whitespace parameter `name="   "`, system returns HTTP 200 with `greeting="Hello, World!"`.
- **Test Set 5:** Given parameter `name` of exactly 50 characters, system returns HTTP 200 with `greeting="Hello, {name}!"`.
- **Test Set 6:** Given parameter `name` of 51 characters, system returns HTTP 400 with RFC 7807 problem details.
- **Test Set 7:** Given parameter `name="José"`, system returns HTTP 200 with safely encoded Unicode characters.

### Properties (Universally Quantifiable Predicates)

- **Property 1:** For all successful responses, `generatedAt` parses as a valid ISO-8601 UTC timestamp.
- **Property 2:** For all JSON payloads, field keys strictly adhere to camelCase naming conventions.
- **Property 3:** For all recipient names with length <= 50, the endpoint returns HTTP status code 200.
- **Property 4:** For all recipient names with length > 50, the endpoint returns HTTP status code 400.

### Contracts (Preconditions, Invariants, Postconditions)

- **Contract 1:** Precondition: Caller issues HTTP GET request to `/api/v1/hello`.
- **Contract 2:** Invariant: System executes statelessly without modifying persistent storage or reading databases.
- **Contract 3:** Postcondition: System returns valid JSON matching `HelloWorldResponse` or RFC 7807 `ProblemDetails`.

## Measurable Success Criteria

- **SC-001:** 95% of client developers complete connection verification in under 5 minutes.
- **SC-002:** The service returns greeting responses in under 25 milliseconds under standard load.
- **SC-003:** 100% of greeting responses conform to standard camelCase JSON schema.
- **SC-004:** Zero integration support tickets arise from baseline greeting schema ambiguities.
- **SC-005:** Automated test coverage exceeds 85% branch coverage across domain and API layers.

## Implementation Directives

- **Directive 1:** Use `FakeTimeProvider` in unit and integration tests to ensure deterministic timestamp evaluations.
- **Directive 2:** Do not inject localization providers or dictionary files into the hello-world service.
- **Directive 3:** Register `ValidationFilter<HelloWorldRequest>` on `/api/v1/hello` per ADR-0003.
- **Directive 4:** Execute `dotnet test` after completing each task group to verify all test suites pass.
