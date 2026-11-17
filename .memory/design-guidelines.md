# Customer Greetings Service — Design Guidelines

## Design Principles & Vibe

The Customer Greetings Service delivers backend greetings content and developer-facing API documentation. The design emphasizes:

- **Clarity and Precision:** API contracts and error responses use clear, predictable, and unambiguous terminology.
- **Warmth and Respect:** Generated greetings maintain an inclusive, respectful, and culturally appropriate voice across all supported languages.
- **Developer Utility:** Developer documentation provides clean, high-contrast, and fast-loading interactive exploration.

## Design System & Tooling

- **Interactive API Documentation:** OpenAPI 3.0 / Swagger UI and Scalar interface.
- **Design System for Docs:** Clean, dark-and-light theme responsive layout adhering to WCAG 2.1 AA standards.
- **Formatting Standards:** camelCase JSON payloads with RFC 7807 `ProblemDetails` compliance.

## UI Constraints

These constraints apply to the developer portal and interactive documentation interfaces:

### Color Palette

| Name | Hex/CSS Variable | Usage |
|:---|:---|:---|
| Primary Brand | `#0b5fff` | Interactive buttons, active links, primary endpoints |
| Neutral Dark (Text) | `#111827` | Headings and primary documentation text |
| Neutral Light (Surface) | `#ffffff` | Documentation main background |
| Secondary Surface | `#f3f4f6` | Code blocks, parameters tables, cards |
| Success State | `#10b981` | HTTP 200/201 response tags, operational status badges |
| Error State | `#ef4444` | HTTP 4xx/5xx badges, validation error callouts |
| Border / Divider | `#e5e7eb` | Content dividers, table borders |

### Typography

- **Primary Font:** Inter, system-ui, -apple-system, sans-serif
- **Monospace Font:** JetBrains Mono, Fira Code, monospace (used for JSON schemas, endpoints, and code examples)
- **Headings:** 
  - `h1`: 32px / line-height 40px / Semi-bold (600)
  - `h2`: 24px / line-height 32px / Semi-bold (600)
  - `h3`: 18px / line-height 26px / Medium (500)
- **Body Text:** 14px / line-height 22px / Regular (400)

### Spacing & Layout

- **Layout System:** Single-column content layout with sticky right-hand code sample pane (desktop) and responsive single-column layout (mobile).
- **Spacing Scale:** 4px base increments (4px, 8px, 12px, 16px, 24px, 32px).
- **Breakpoints:** Mobile < 768px, Tablet 768px-1024px, Desktop >= 1024px.

## Content Strategy

The service generates customer greetings in three selectable tones:

1. **Formal Tone:**
   - **Characteristics:** Respectful, polite, uses honorifics when available.
   - **English Example:** "Good morning, Dr. Vance. Welcome to your account."
   - **Spanish Example:** "Buenos días, Dr. Vance. Bienvenido a su cuenta."
2. **Friendly Tone (Default):**
   - **Characteristics:** Warm, approachable, uses customer preferred first name.
   - **English Example:** "Good afternoon, Sarah! Great to see you today."
   - **Spanish Example:** "¡Buenas tardes, Sarah! Qué gusto verte hoy."
3. **Casual Tone:**
   - **Characteristics:** Brief, conversational, energetic.
   - **English Example:** "Hi Alex, welcome back!"
   - **Spanish Example:** "¡Hola Alex, qué bueno que volviste!"

### Terminology Rules

- **Use:** "Greeting", "Salutation", "Preferred Name", "Honorific", "Locale", "Time Zone".
- **Avoid:** Jargon, ambiguous colloquialisms, culturally specific slang, or patronizing phrases.
- **Grammar & Formatting:**
  - Punctuation must strictly follow localized linguistic norms (for example, inverted exclamation mark `¡` and question mark `¿` in Spanish).
  - Sentence case for all documentation headings.

## Accessibility (a11y)

- **Standard:** WCAG 2.1 Level AA.
- **Contrast Ratios:** Minimum 4.5:1 for standard body text and code snippets against backgrounds; minimum 3:1 for large UI indicators.
- **Focus States:** High-visibility outline (2px solid `#0b5fff` with 2px offset) on all interactive elements in documentation.
- **Screen Reader Support:** Semantic HTML tags for documentation elements (`<header>`, `<main>`, `<nav>`, `<code>`, `<pre>`).
- **Cognitive Clarity:** Generated greetings avoid idiom-heavy text to ensure clear machine translation and text-to-speech comprehension.

## Component Behaviors

- **Copy Code Buttons:** Provide visual and accessible feedback (text change to "Copied!" for 2 seconds with `aria-live` announcement).
- **Endpoint Test Runner:** Visual indicators denote network in-flight state with animated spinner and disabled submission button.
- **Validation Messages:** Display immediately adjacent to the offending input parameter with explicit remediation instructions.
