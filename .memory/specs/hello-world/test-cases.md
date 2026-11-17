# Test Cases: Hello World Greeting

## 🟢 Happy Path Scenarios

### TC-01: Default Greeting Delivery
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send HTTP GET request to `/api/v1/hello` without query parameters.
- **Expected Result**: The service returns HTTP 200 OK with payload `{"greeting": "Hello, World!", "generatedAt": "..."}`.

### TC-02: Personalized Greeting Delivery
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send HTTP GET request to `/api/v1/hello?name=Sarah`.
- **Expected Result**: The service returns HTTP 200 OK with payload `{"greeting": "Hello, Sarah!", "generatedAt": "..."}`.

### TC-03: Personalized Greeting with Unicode Characters
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send HTTP GET request to `/api/v1/hello?name=José`.
- **Expected Result**: The service returns HTTP 200 OK with properly escaped Unicode characters `{"greeting": "Hello, José!", "generatedAt": "..."}`.

## 🔴 Negative Scenarios

### TC-04: Reject Recipient Name Longer Than 50 Characters
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Generate a recipient string containing 51 alphabetical characters.
  2. Send HTTP GET request to `/api/v1/hello?name={string}`.
- **Expected Result**: The service returns HTTP 400 Bad Request with RFC 7807 problem details identifying the name parameter.

### TC-05: Reject Query with Excessive String Length
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Generate a string containing 500 characters.
  2. Send HTTP GET request to `/api/v1/hello?name={string}`.
- **Expected Result**: The service rejects the request with HTTP 400 Bad Request without leaking internal stack traces.

## 🟡 Edge Cases

### TC-06: Empty Recipient Name Fallback
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send HTTP GET request to `/api/v1/hello?name=`.
- **Expected Result**: The service falls back to default greeting `{"greeting": "Hello, World!", "generatedAt": "..."}` with HTTP 200 OK.

### TC-07: Whitespace-Only Recipient Name Fallback
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send HTTP GET request to `/api/v1/hello?name=%20%20%20`.
- **Expected Result**: The service falls back to default greeting `{"greeting": "Hello, World!", "generatedAt": "..."}` with HTTP 200 OK.

### TC-08: Recipient Name Exactly 50 Characters
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Generate a recipient string containing exactly 50 characters.
  2. Send HTTP GET request to `/api/v1/hello?name={string}`.
- **Expected Result**: The service accepts the string and returns HTTP 200 OK with personalized greeting `{"greeting": "Hello, {string}!", "generatedAt": "..."}`.

### TC-09: Recipient Name with Surrounding Spaces
- **Pre-conditions**: The service is running and healthy.
- **Steps**:
  1. Send HTTP GET request to `/api/v1/hello?name=%20Alex%20`.
- **Expected Result**: The service trims the input and returns HTTP 200 OK with greeting `{"greeting": "Hello, Alex!", "generatedAt": "..."}`.

## 🔵 Non-Functional Scenarios

### TC-10: Sub-25ms Response Latency Verification
- **Pre-conditions**: The service is running under normal operating load.
- **Steps**:
  1. Send 100 sequential HTTP GET requests to `/api/v1/hello`.
  2. Record the elapsed execution time for each response.
- **Expected Result**: The 95th percentile response latency remains under 25 milliseconds.

### TC-11: OpenAPI Contract Conformance Check
- **Pre-conditions**: The service exposes OpenAPI endpoints.
- **Steps**:
  1. Retrieve the OpenAPI JSON specification from `/openapi/v1.json`.
  2. Inspect the operations registered for path `/api/v1/hello`.
- **Expected Result**: The document defines the GET operation, query parameter `name`, and HTTP 200/400 response contracts.
