---
id: ASSUME-0002
title: Upstream Gateway Handles Authentication and Ingress Security
component: customer-greetings-api
domain: infrastructure
tags:
- authentication
- api-gateway
- security
status: unresolved
created: '2026-03-30'
updated: '2026-03-30'
---

### Assumption Description
Edge API Gateways or upstream ingress controllers handle client authentication, TLS termination, and rate limiting. The internal Customer Greetings Service expects validated claims or service-to-service mutual TLS tokens.

### Rationale
Centralizing authentication at the API Gateway standardizes token validation and protects internal services from unauthenticated traffic.

### Impact if Invalid
If callers invoke the service directly without an API Gateway, the service must implement full token validation, authorization middleware, and rate-limiting modules directly.

### Validation Method
Review infrastructure ingress architecture with the platform infrastructure team before Beta deployment.