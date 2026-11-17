# Customer Greetings Service — Test Strategy

## Overview

This document specifies the testing strategy for the Customer Greetings Service (`customer-greetings-api`). The testing approach guarantees functional accuracy, linguistic correctness, deterministic time boundary evaluation, low latency performance, and strict security compliance across all deployment environments.

## Test Environments & Access

- **Environments:**
  - **Local Development:** Developers execute unit, integration, and contract tests locally using .NET 9 CLI and Docker.
  - **CI (GitHub Actions):** Automated test pipelines run on every pull request and push to the `main` branch.
  - **Staging / QA:** Deployed in containerized staging cluster for end-to-end integration verification, contract testing, and performance validation.
  - **Production:** Monitored via synthetic health probes (`/healthz/ready`) and real-time OpenTelemetry metrics.
- **Access Needs:** Internal container registry tokens, GitHub Actions secrets, and read-only staging ingress tokens.
- **Data Management:** All greetings tests use isolated, stateless test payloads. No shared persistent database state exists.
- **CI/CD Integration:** PR builds gate merging on 100% test pass rate, zero critical SAST security findings, and minimum 85% branch coverage.

## Testing Approach

| Level | Scope / Objective | Tools / Frameworks | Success Criteria | Responsibilities |
|---|---|---|---|---|
| **Unit** | Core business logic, greeting formatting, time boundary classifiers, and localization dictionary mappings in isolation. | xUnit, FluentAssertions, `FakeTimeProvider` | 100% pass rate; >= 85% branch coverage; zero flaky tests | Developers |
| **Component (Web)** | Developer documentation UI components and interactive API test runner in isolation. | Playwright, Vitest / Jest | Component renders without errors across viewports | Frontend / QA |
| **Contract** | Verify request and response JSON schemas conform to consumer expectations. | Pact.Net, OpenAPI Schema Validator | Zero contract breaks against published schemas | Developers, QA |
| **Integration (Mocked)** | Test Minimal API endpoints with mocked infrastructure dependencies. | `Microsoft.AspNetCore.Mvc.Testing`, Moq / NSubstitute | Endpoints return expected HTTP status codes and RFC 7807 payloads | Developers, QA |
| **Integration (API / Events)** | Endpoints running in test containers verifying real localization resource file reading. | `WebApplicationFactory`, REST client | Validated endpoints return 200 OK with correct headers | Developers, QA |
| **Acceptance (Functional)** | Verification of complete greeting scenarios against business requirements across languages. | xUnit, SpecFlow / Reqnroll | All acceptance criteria met across all supported locales | QA, Developers |
| **End-to-End (E2E)** | Full caller request through API Gateway to Greetings Service and back. | Playwright / Postman CLI / Newman | Critical user paths succeed in Staging environment | QA, DevOps |
| **Performance (API)** | Load, spike, and latency tests on greeting endpoints. | k6 | p95 response time < 25ms at 2,000 requests per second | QA, DevOps |
| **Performance (Web)** | Interactive documentation load speed and bundle size analysis. | Lighthouse | Lighthouse Performance score >= 90; LCP < 2.0s | Developers, QA |
| **Security** | Static code analysis, dependency vulnerability scanning, and container image scans. | SonarQube, Trivy, GitHub CodeQL | Zero Critical or High severity vulnerabilities | Security, DevOps |
| **Accessibility** | Documentation portal and interactive UI compliance with accessibility standards. | axe-core, Pa11y | WCAG 2.1 Level AA compliance; zero critical axe violations | QA, UX, Developers |

## Risk & Mitigation

| Risk | Mitigation |
|---|---|
| E2E test flakiness (async state) | Use state-based assertions and deterministic HTTP completion signals instead of thread sleeps. |
| Timezone and temporal race conditions | Enforce [ASSUME-0003](assumptions/ASSUME-0003-deterministic-time-testing.md) using `FakeTimeProvider` for all temporal boundary tests. |
| Test data contamination | Maintain stateless execution; generate unique identifiers per test execution. |
| Localization resource drift | Execute automated schema validation tests comparing keys across all language JSON files during CI. |

## Reviewed by

| Name | Role | Approved | Date |
|---|---|---|---|
| Guy Smith-Ferrier (`guysmith-ferrier-at-hippo`) | Lead Architect | [x] | 2026-03-30 |
| QA Engineering Lead | Test Lead | [x] | 2026-03-30 |
| Product Operations Lead | Product Manager | [x] | 2026-03-30 |
