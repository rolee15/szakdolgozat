---
name: refactor-api
description: >
  Audits and redesigns the KanjiKa REST API surface. Reviews all controller endpoints against
  REST best practices, then delegates targeted fixes to backend-dev. Use when asked to "refactor
  the API", "review endpoint design", "fix HTTP status codes", "apply REST best practices",
  or "improve the API contract".
---

# API Design Refactor — REST Best Practices

You are auditing the KanjiKa REST API surface. **Do not write code yourself.** Produce a concrete violation report and refactor plan, then delegate all changes to the `backend-dev` agent.

---

## Phase 0 — Codebase scan

Read all controller files and their associated DTOs and interfaces:

1. All files in `server/src/KanjiKa.Api/Controllers/`
2. All files in `server/src/KanjiKa.Core/DTOs/`
3. All files in `server/src/KanjiKa.Core/Interfaces/`

For each endpoint, record:
- HTTP method and route template
- Input types (route params, query string, body DTO)
- Response types and status codes declared via `[ProducesResponseType]`
- Any entity type returned directly

---

## Phase 1 — Violation catalogue

For each violation, record:
```
Endpoint:  METHOD /api/path
Rule:      <which rule from Phase 2 below>
Issue:     <what is wrong>
Fix:       <what to change>
Files:     <controller, DTO, interface>
```

---

## Phase 2 — REST best-practice rules

Apply every rule below to each endpoint:

### Resource naming
*(Source: [Azure API Design Best Practices](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design), [REST API Best Practices — DEV](https://dev.to/_d7eb1c1703182e3ce1782/rest-api-best-practices-endpoint-naming-versioning-and-error-handling-4321))*

- **Nouns, not verbs**: the URI identifies a resource; the HTTP method expresses the action
  ```
  ✅  GET  /api/characters
  ❌  GET  /api/getCharacters
  ✅  POST /api/users/{id}/lesson-completions
  ❌  POST /api/completelesson
  ```
- **Plural nouns for collections**: `/characters`, `/users`, `/proficiencies`
- **Singular noun for a specific item**: `/characters/{id}`, `/users/{id}`
- **Sub-resources for owned entities**: model ownership with hierarchy, but stop at `collection/item/collection`:
  ```
  ✅  /users/{userId}/proficiencies
  ✅  /users/{userId}/lesson-completions
  ❌  /users/{userId}/lesson-completions/{lessonId}/characters/{charId}  ← too deep
  ```
- **Lowercase, hyphen-separated** path segments: `/lesson-completions` not `/LessonCompletions` or `/lesson_completions`
- **No file extensions**: `/characters` not `/characters.json`
- **Never mirror the DB schema**: routes expose a business model, not table names

### HTTP verbs and idempotency
*(Source: [Azure API Design Best Practices](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design))*

| Method | Use for | Idempotent? | Safe? |
|---|---|---|---|
| `GET` | Retrieve a resource or collection | Yes | Yes |
| `POST` | Create a new resource (server assigns ID) | No | No |
| `PUT` | Full replacement of a known resource | Yes | No |
| `PATCH` | Partial update of a known resource | No | No |
| `DELETE` | Remove a resource | Yes | No |

- `GET` requests must **never have a body** and must **never mutate state**
- `PUT` must replace the **entire** resource — use `PATCH` for partial updates
- `POST` to a collection (`/users/{id}/proficiencies`) creates a child resource; `POST` to a specific item is an error unless it's a non-resource action (e.g. `/answers:check`)

### HTTP status codes
*(Source: [Azure API Design Best Practices](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design), [REST API Best Practices — Postman](https://blog.postman.com/rest-api-best-practices/))*

**Success codes:**

| Code | When to use |
|---|---|
| `200 OK` | Successful GET, PUT, PATCH |
| `201 Created` | Successful POST that created a resource — **must** include `Location` header pointing to the new resource |
| `204 No Content` | Successful DELETE, or PUT/PATCH with no response body |

**Client error codes:**

| Code | When to use |
|---|---|
| `400 Bad Request` | Malformed request syntax (unparseable JSON, missing required field at the protocol level) |
| `401 Unauthorized` | Request lacks valid credentials — authentication required |
| `403 Forbidden` | Credentials are valid but the user lacks permission |
| `404 Not Found` | Resource does not exist |
| `409 Conflict` | Resource already exists (duplicate creation), or state conflict (e.g. completing a lesson already completed) |
| `422 Unprocessable Entity` | Well-formed request but business validation failed (e.g. invalid answer stage value) |

**Server error codes:**

| Code | When to use |
|---|---|
| `500 Internal Server Error` | Unhandled exception — should never be returned deliberately |

Rules:
- **Never return `200 OK` for a failed operation** — use the appropriate 4xx or 5xx
- **`201 Created` must include a `Location` header**: `Response.Headers.Location = $"/api/characters/{id}"`
- **`204 No Content` must have no body**
- Do not return `400` for business rule violations — use `409` or `422`

### Error response format
*(Source: [REST API Best Practices — DEV](https://dev.to/_d7eb1c1703182e3ce1782/rest-api-best-practices-endpoint-naming-versioning-and-error-handling-4321), [REST API Best Practices — oneuptime](https://oneuptime.com/blog/post/2026-02-20-api-design-rest-best-practices/view))*

Every error response must use a **consistent structured envelope** — never return raw strings:
```json
{
  "error": {
    "code": "CHARACTER_NOT_FOUND",
    "message": "No character with id 42 exists."
  }
}
```

- `code` — machine-readable SCREAMING_SNAKE_CASE constant; clients can `switch` on it
- `message` — human-readable explanation, safe to display
- Optional `details` array for validation errors with multiple field-level messages:
  ```json
  {
    "error": {
      "code": "VALIDATION_FAILED",
      "message": "The request contains invalid fields.",
      "details": [
        { "field": "stage", "message": "Stage must be between 0 and 8." }
      ]
    }
  }
  ```
- Define this shape as a DTO in `KanjiKa.Core/DTOs/`: `record ErrorResponse(ErrorDetail Error)` and `record ErrorDetail(string Code, string Message, IReadOnlyList<FieldError>? Details = null)`
- All controllers return this DTO for every non-success response — **never** `BadRequest("some string")`

### Response envelopes — collections
*(Source: [Azure API Design Best Practices](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design), [REST API Design Best Practices — upforcetech](https://upforcetech.com/best-practices-for-rest-api-design/))*

- List endpoints must support pagination: `GET /characters?page=1&pageSize=20`
- Return total count in the **`X-Total-Count` response header** (keeps the body as a plain array)
- Return a plain JSON array in the body — not `{ "data": [...] }` — unless you need to attach metadata alongside the array
- If filtering and sorting are needed, use query parameters: `?type=hiragana&sort=value&order=asc`
- Never return the entire table without a page size cap — default to `pageSize=20`, max to `pageSize=100`

### OpenAPI / Swagger documentation
*(Source: [Azure API Design Best Practices](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design))*

- Every controller action must declare **`[ProducesResponseType]`** for every status code it can return:
  ```csharp
  [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
  ```
- Use `[Consumes("application/json")]` and `[Produces("application/json")]` at the controller level
- Add `[SwaggerOperation(Summary = "...", Description = "...")]` for non-obvious endpoints
- Verify `/swagger` loads and every endpoint is documented after the refactor

### API versioning
*(Source: [REST API Best Practices — DEV](https://dev.to/_d7eb1c1703182e3ce1782/rest-api-best-practices-endpoint-naming-versioning-and-error-handling-4321), [API Design Best Practices 2025](https://myappapi.com/blog/api-design-best-practices-2025))*

- All routes must be under **`/api/v1/...`** to enable future non-breaking evolution
  ```
  ✅  /api/v1/characters
  ❌  /api/characters
  ```
- Use URL path versioning (not headers or query strings) — it is explicit and easy to test in a browser
- Introduce a **new version only for breaking changes**: removed fields, changed types, changed auth, renamed routes
- Non-breaking additions (new optional fields, new endpoints) do not require a new version
- When deprecating a version, add a `Deprecation` response header with the sunset date

### DTOs — never expose entities
*(Source: [Clean Architecture in .NET 8](https://medium.com/@madu.sharadika/clean-architecture-in-net-8-web-api-483979161c80))*

- Controllers must **never return an EF entity** — always map to a DTO before responding
- Request DTOs validate input at the controller boundary; entity constructors enforce business invariants
- Separate **request DTOs** (input) from **response DTOs** (output) — do not reuse the same type for both
- Use **`record`** for all DTOs:
  ```csharp
  public record CreateUserRequest(string Username, string Password);
  public record UserDto(int Id, string Username, DateTimeOffset CreatedAt);
  ```

---

## Phase 3 — Refactor plan

Produce a numbered list ordered by blast radius (lowest first). For each item:

```
### A-<N>: <Short title>

Endpoint(s): <METHOD /api/path>
Rule(s):     <which rules from Phase 2>
Issue:       <what is currently wrong>
Fix:         <what to change, exactly>
Files:       <list every file to touch>
Risk:        Low / Medium / High
Breaking?:   Yes / No  (breaking = clients must update)
```

Flag any change that is **breaking** — these require a version bump to `/api/v2/...`.

---

## Phase 4 — Delegation

1. Present the full violation report and plan to the user. Wait for approval.
2. Group non-breaking fixes into a single `backend-dev` call.
3. Handle breaking changes separately — confirm with the user whether to bump the version or whether there are no external clients to protect.
4. After each agent call, verify:
   ```bash
   cd server && dotnet build --configuration Release
   cd server && dotnet test test/KanjiKa.IntegrationTests/ --logger "console;verbosity=normal"
   ```
   Integration tests cover the API contract — if any fail, spawn `debugger` before continuing.
5. Open `/swagger` manually and confirm every endpoint is documented correctly.
6. Update `MANUAL_TEST.md` for any changed route or behavior; update `docs/references.md` if any pattern was adapted from external sources.
