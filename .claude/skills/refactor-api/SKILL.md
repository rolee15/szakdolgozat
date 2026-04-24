---
name: refactor-api
description: >
  Audits and redesigns the KanjiKa REST API surface. Reviews all controller endpoints against
  REST best practices, then delegates targeted fixes to backend-dev. Use when asked to "refactor
  the API", "review endpoint design", "fix HTTP status codes", or "apply REST best practices".
---

# API Design Refactor — REST Best Practices

**Plan — don't implement.** Produce a violation report and refactor plan; delegate all edits to the `backend-dev` agent.

## Phase 0 — Scan

Read `server/src/KanjiKa.Api/Controllers/`, `server/src/KanjiKa.Application/DTOs/`, and `server/src/KanjiKa.Application/Interfaces/`. For each endpoint, record: HTTP method + route, input types, response types, `[ProducesResponseType]` accuracy, any entity returned directly.

## Phase 1 — Violation report

For each issue, record:

```
Endpoint:  METHOD /api/path
Rule:      <which rule from Phase 2>
Issue:     <what is wrong>
Fix:       <what to change>
Files:     <controller, DTO, interface>
```

## Phase 2 — Rules

### Resource naming

- Nouns, not verbs (`GET /characters`, not `GET /getCharacters`).
- Plural collections (`/users`, `/proficiencies`); sub-resources for ownership (`/users/{id}/proficiencies`). Avoid nesting past `collection/item/collection`.
- Lowercase, hyphen-separated segments. No file extensions. Don't mirror DB table names — expose a business model.

### HTTP verbs

| Method | Purpose | Idempotent | Safe |
|---|---|---|---|
| `GET` | read | yes | yes |
| `POST` | create (server assigns ID) | no | no |
| `PUT` | full replace | yes | no |
| `PATCH` | partial update | no | no |
| `DELETE` | remove | yes | no |

`GET` must have no body and must not mutate state.

### Status codes

| Code | When |
|---|---|
| `200` | successful GET/PUT/PATCH with body |
| `201` | created — must include `Location` header |
| `204` | successful DELETE or response-less PUT/PATCH |
| `400` | malformed syntax |
| `401` / `403` | missing vs. insufficient credentials |
| `404` | not found |
| `409` | duplicate or state conflict |
| `422` | well-formed but business validation failed |

Never return `200` for a failed operation. Never use `400` for business-rule failures — use `409`/`422`.

### Error envelope

Every non-success response uses a typed DTO — never raw strings:

```json
{ "error": { "code": "CHARACTER_NOT_FOUND", "message": "No character with id 42." } }
```

- `code` — SCREAMING_SNAKE_CASE constant clients can switch on.
- `message` — human-readable, safe to display.
- Optional `details[]` for field-level validation errors.

Define as a `record` in `KanjiKa.Application/DTOs/`.

### DTOs, not entities

Controllers return DTOs. Separate request DTOs (input) from response DTOs (output). Use `record` for both.

### Collections

Paginate list endpoints (`?page=&pageSize=`); return total count in `X-Total-Count`. Return a plain array in the body unless metadata is needed. Default `pageSize=20`, cap at `100`.

### OpenAPI

Every action declares `[ProducesResponseType]` for every status code it can return. `[Consumes/Produces("application/json")]` at the controller level. Confirm `/swagger` lists each endpoint accurately.

### Versioning

Routes live under `/api/v1/...`. Bump to `v2` only for breaking changes (removed field, renamed route, changed auth). Additive, non-breaking changes stay on `v1`.

## Phase 3 — Plan

Numbered list, low-risk first. For each item:

```
### A-<N>: <Short title>
Endpoint(s): <METHOD /api/path>
Rule(s):     <from Phase 2>
Issue:       <what is wrong>
Fix:         <what to change>
Files:       <every file to touch>
Risk:        Low / Medium / High
Breaking?:   Yes / No
```

Flag breaking items; they require a version bump unless the user confirms there are no external clients.

## Phase 4 — Delegate & verify

1. Present the plan; wait for approval.
2. Batch non-breaking fixes into one `backend-dev` call; handle breaking changes separately.
3. After each batch: `dotnet build --configuration Release` + `dotnet test` (integration tests cover the API contract). On failure, spawn `debugger`.
4. Open `/swagger` and confirm every endpoint is documented.
5. Update `MANUAL_TEST.md` for changed routes; add `docs/references.md` entries for any externally-sourced pattern.
