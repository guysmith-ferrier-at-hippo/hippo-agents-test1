# 🧭 Product Vision Document

## 1. Product Overview

**Product Name:** Customer Greetings Service (`customer-greetings-api`)  
**Author / Owner:** Hippo Engineering (`guysmith-ferrier-at-hippo`)  

---

## 2. Vision Statement

Provide a reliable, low-latency, and localized customer greeting API that delivers consistent and respectful greetings across all digital channels.

---

## 3. Problem Statement

Multiple digital applications generate customer greeting messages independently.

- **Who experiences the problem:** Front-end web developers, mobile application engineers, notification service teams, and end customers.
- **Impact of the problem today:** Inconsistent greeting tone, incorrect time-of-day salutations, inadequate localization, and duplicated formatting logic across codebases.
- **Why solving this matters:** Centralized greeting logic ensures brand consistency, reduces duplicate development effort, and provides an inclusive customer experience in multiple languages.

---

## 4. Target Users / Audiences

| Persona | Description | Key Needs / Goals |
|---|---|---|
| Client Application Developer | Engineers building customer-facing web and mobile applications | Needs a simple REST API to retrieve formatted, localized greetings quickly |
| Notification Service | Automated backend services that send email and push messages | Needs reliable greeting generation with low latency and consistent templates |
| End Customer | Individuals using digital services across web and mobile portals | Receives accurate, respectful, and time-appropriate greetings in their chosen language |

---

## 5. Product Goals & Outcomes

| Goal | Metric / KPI | Desired Outcome |
|---|---|---|
| Deliver high performance | API response time (p95) | Less than 25 milliseconds response time under standard load |
| Ensure high service availability | Uptime percentage | 99.95% operational availability across target environments |
| Provide localization support | Supported locales | Provide accurate greetings in English, Spanish, French, and German at launch |
| Maintain high code quality | Automated test coverage | Minimum 85% branch and line coverage across domain and API layers |

---

## 6. Key Features / Capabilities

- **Personalized Salutations:** Format greetings using customer first name, full name, or respectful honorifics.
- **Time-Contextual Greetings:** Compute time-of-day greetings (morning, afternoon, evening) using customer time zone or local time.
- **Multi-Language Support:** Support localization via standard IETF BCP 47 language tags (e.g., `en-GB`, `es-ES`, `fr-FR`, `de-DE`).
- **Tone Selection:** Allow callers to select formal, friendly, or casual greeting tones.
- **OpenAPI Documentation:** Publish interactive OpenAPI 3.0 specifications and health endpoints (`/healthz`, `/readyz`).

---

## 7. Value Proposition

| For (user type) | Who want to (need) | Our product (solution) | Unlike (alternative) |
|---|---|---|---|
| Internal Product Teams | Deliver consistent customer greetings | The Customer Greetings Service API | Fragmented, hard-coded greeting logic duplicated in each client app |

---

## 8. Constraints / Assumptions

- **Constraints:**
  - The service must comply with .NET 9 LTS runtime requirements and containerized Linux hosting.
  - The service must return errors formatted strictly to RFC 7807 (`ProblemDetails`).
  - The service must not store sensitive personally identifiable information (PII) to maintain compliance with data privacy regulations.
- **Assumptions:**
  - [ASSUME-0001: Stateless Customer Context Ingestion](assumptions/ASSUME-0001-stateless-greeting-architecture.md) — The service operates statelessly without direct profile database lookups.
  - [ASSUME-0002: Upstream Gateway Handles Authentication and Ingress Security](assumptions/ASSUME-0002-upstream-gateway-auth.md) — Edge gateways provide authentication, SSL termination, and rate limiting.

---

## 9. Success Metrics

- **Primary metric(s):** Mean latency < 20 ms, p95 latency < 25 ms, HTTP 5xx error rate < 0.01%.
- **Secondary metric(s):** Developer adoption rate across Hippo internal client applications.
- **Qualitative indicators:** Positive developer feedback regarding API clarity and OpenAPI contract quality.

---

## 10. Stakeholders

| Role | Name / Team | Interest / Contribution |
|---|---|---|
| Product Owner / Lead | Guy Smith-Ferrier (`guysmith-ferrier-at-hippo`) | Architecture governance, release sign-off, vision approval |
| Engineering Team | Hippo Core Services Team | Implementation, test automation, pipeline maintenance |
| Client Application Teams | Web & Mobile Frontend Leads | API consumption, schema feedback, localization validation |
| Operations & Platform | Cloud Platform Operations | Container hosting, monitoring, and telemetry ingestion |

---

## 11. Risks & Open Questions

- **Risks:** Incorrect client time zone parsing could yield inappropriate time-of-day salutations.
- **Open Questions:** Will future phases require custom organization-level greeting templates configured via dynamic storage?

---

## 12. References

- [ASSUME-0001: Stateless Customer Context Ingestion](assumptions/ASSUME-0001-stateless-greeting-architecture.md)
- [ASSUME-0002: Upstream Gateway Handles Authentication and Ingress Security](assumptions/ASSUME-0002-upstream-gateway-auth.md)
- RFC 7807: Problem Details for HTTP APIs (https://datatracker.ietf.org/doc/html/rfc7807)
- IETF BCP 47: Tags for Identifying Languages (https://tools.ietf.org/rfc/bcp/bcp47.txt)

---

## 13. Value Rule Set

### Organizational Value Definition

Hippo evaluates features by four primary value drivers:
- **Developer Velocity:** Enable client teams to integrate greetings in minutes.
- **Operational Efficiency:** Eliminate duplicated greeting logic across multiple frontend codebases.
- **Platform Reliability:** Maintain low latency and continuous service availability.
- **Brand Consistency:** Provide standardized, respectful greetings across all digital channels.

### Value Rule Set

Features must meet these evaluation criteria before approval for development:

| Rule / Criterion | Description | Minimum Threshold for Go |
|---|---|---|
| Developer Adoption Velocity | Enables fast client onboarding and simple verification | Client verifies connection in under 5 minutes |
| Architectural Compliance | Follows stateless, zero-PII, and gateway-decoupled patterns | 100% adherence to architecture standards |
| Operational Simplicity | Adds no persistent state or external database requirements | Zero new operational databases |
| Strategic Alignment | Supports baseline greeting delivery or localization roadmap | Directly advances platform greeting goals |
