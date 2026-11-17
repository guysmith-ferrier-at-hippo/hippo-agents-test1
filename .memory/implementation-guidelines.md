# Implementation Guidelines

## 1. Minimal API Endpoint Filters

Use generic endpoint filters to validate incoming requests.
Do not write repetitive validation code inside route handlers.
Register validators using FluentValidation in the dependency container.
Resolve validators with `GetRequiredService<IValidator<T>>()` to fail fast when configuration is missing.
Check property names for null or empty values before name conversion.
Return RFC 7807 problem details payloads on validation failures.

## 2. Time Abstraction

Use Microsoft .NET `TimeProvider` for all temporal operations.
Do not call `DateTime.UtcNow` or `DateTimeOffset.UtcNow` directly.
Inject `TimeProvider` into application services.
Use `FakeTimeProvider` in tests to guarantee deterministic time checks.

## 3. JSON Contract Standards

Use camelCase property names for all response models.
Convert dictionary keys to camelCase in validation error responses.
Mask internal stack traces in all production error outputs.
Use typed results for all Minimal API endpoints.

## 4. Build Configuration

Configure shared build settings in `Directory.Build.props`.
Do not duplicate shared properties across test project files.
