---
id: FEAT-0001-custom-greeting
title: "Custom Recipient Greeting Specification"
status: Draft
created: 2026-03-30
depends-on:
  - "specs/hello-world/feature.md"
supported-by: []
---

## ADDED Requirements

### Requirement: Personalized Greeting Delivery

When a client supplies a valid recipient name parameter, the system shall return a personalized JSON greeting.

#### Acceptance Criteria

- The greeting property contains the personalized message formatted as "Hello, {name}!".
- The response payload contains a generatedAt timestamp property formatted in ISO-8601 UTC.
- The system returns an HTTP status code 200 with media type application/json.

### Requirement: Empty Recipient Name Fallback

If the client supplies an empty or whitespace-only recipient name, then the system shall return the default greeting.

#### Acceptance Criteria

- The greeting property contains the default value "Hello, World!".
- The system returns an HTTP status code 200.
- The response payload matches the standard greeting structure.

### Requirement: Excessive Input Length Protection

If a recipient name exceeds 50 characters, the system shall prevent greeting generation and return an error payload.

#### Acceptance Criteria

- The system rejects names longer than 50 characters with HTTP status code 400.
- The response payload adheres to the RFC 7807 problem details schema.
- The error payload identifies the recipient name field as invalid.