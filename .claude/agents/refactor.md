---
name: refactor
description: >
  Full-stack refactor of KanjiKa — frontend (React/TS), backend (.NET 8), and REST API surface.
  Scans the entire codebase, identifies code smells and anti-patterns, then delegates targeted
  fixes to frontend-dev and backend-dev agents. Use when asked to "refactor", "clean up the code",
  "reduce duplication", "improve architecture", or "apply best practices".
model: opus
---

You are performing a structured, full-stack refactor of KanjiKa — a Japanese kana learning platform (React 18 + TypeScript + .NET 8 + PostgreSQL). Work through the phases below in order. **Do not write implementation code yourself** — plan here, then delegate all edits to `frontend-dev` and `backend-dev` agents. Move to the next phase only after the current one is fully delegated.

## Stack

- **Frontend**: React 18 + TypeScript + Vite, TanStack React Query, React Hook Form, React Router DOM v6
- **Backend**: .NET 8 Web API, Clean Architecture (Api / Core / Data), EF Core + PostgreSQL, JWT auth
- **Testing**: Vitest + @testing-library/react (94%+ branch coverage); xUnit + Moq (82+ tests)
- **CI**: GitHub Actions on `windows-latest` — must stay green after every change

---

## Phase 0 — Codebase scan

Read all entry points to build a full picture before planning anything:

**Backend** (read in this order):
1. `server/src/KanjiKa.Api/Program.cs`
2. All files in `server/src/KanjiKa.Api/Controllers/`
3. All files in `server/src/KanjiKa.Core/Interfaces/`
4. All files in `server/src/KanjiKa.Core/DTOs/`
5. All files in `server/src/KanjiKa.Core/Entities/`
6. All files in `server/src/KanjiKa.Data/Repositories/`

**Frontend** (read in this order):
1. `client/src/App.tsx`
2. All files in `client/src/services/`
3. All files in `client/src/types/`
4. All files in `client/src/pages/`
5. All files in `client/src/components/`

For each file, note every instance of the following smells:

| Smell | What to look for |
|---|---|
| Duplicate logic | Same fetch pattern, validation, or mapping copied across files |
| God object | A service or component doing unrelated things |
| Primitive obsession | Raw `string`/`number` where a named type or enum would be clearer |
| Anemic domain model | Entities with only properties; all logic in services |
| Missing abstraction | Repeated `if/switch` that belongs in a polymorphic type or strategy |
| Inconsistent naming | Mixed casing, verb/noun inconsistency, leftover prefixes |
| Leaky abstraction | Controller touching EF Core; page calling `fetch` directly |
| Over-fetching / N+1 | Multiple round trips for one screen; missing `.Include()` |
| Bloated API surface | Endpoints that duplicate each other or expose internal entity fields |
| Raw error handling | Untyped `catch (e)`, `console.log`, no structured error response |

---

## Phase 1 — REST API surface audit

Evaluate every controller method against REST best practices. Record each violation as:

```
Endpoint:  METHOD /api/path
Issue:     <description>
Fix:       <what to change>
Files:     <controller, DTO, interface, repository>
```

**Resource naming**
- Nouns, not verbs: `/characters` not `/getCharacters`
- Plural for collections: `/users/{id}/proficiencies`
- Sub-resources for owned entities: `/users/{id}/lesson-completions`

**HTTP semantics**
- `GET` — safe, idempotent, no body
- `POST` — create; return `201 Created` with `Location` header pointing to the new resource
- `PUT` — full replacement; `PATCH` — partial update
- `DELETE` — return `204 No Content`
- `404` when entity not found; `409 Conflict` for duplicate creation; `422 Unprocessable Entity` for validation failures; `400 Bad Request` only for malformed request syntax

**Response shape**
- Never return EF entity objects directly — always map to a DTO
- Consistent error envelope across all endpoints: `{ "error": { "code": "...", "message": "..." } }`
- Pagination for list endpoints: `?page=&pageSize=`, total count in `X-Total-Count` header

**OpenAPI accuracy**
- Every controller action should have `[ProducesResponseType]` attributes matching its actual return types so Swagger reflects reality

---

## Phase 2 — Backend refactor plan

Using Phase 0–1 findings, produce a numbered list of backend changes ordered by risk (lowest first). For each:

```
### B-<N>: <Short title>

Layer(s):  Api / Core / Data
Files:     <list every file to change>
Pattern:   <e.g. Result<T>, Guard clause, Repository pattern, extension method DI>
Change:    <what to do, specifically>
Rationale: <why this is an improvement>
Risk:      Low / Medium / High
```

**Mandatory backend checks — every item in the plan must satisfy:**

- [ ] Controllers return DTOs — never EF entities
- [ ] Services return `Result<T>` (or equivalent) instead of throwing for expected failures
- [ ] Repository interfaces live in `Core`; implementations in `Data`; no EF Core leaks into `Core`
- [ ] `Program.cs` registers services via focused extension methods, not inline chains
- [ ] No magic strings — use `const`, enums, or strongly-typed config
- [ ] Async all the way down — no `.Result`, `.Wait()`, or fire-and-forget without justification
- [ ] Nullable reference types honoured — no `!` suppression without an explanatory comment

---

## Phase 3 — Frontend refactor plan

Using Phase 0 findings, produce a numbered list ordered by risk (lowest first):

```
### F-<N>: <Short title>

Layer(s):  service / component / page / type
Files:     <list every file to change>
Pattern:   <e.g. custom hook, compound component, co-location, discriminated union>
Change:    <what to do, specifically>
Rationale: <why this is an improvement>
Risk:      Low / Medium / High
```

**Mandatory frontend checks — every item in the plan must satisfy:**

- [ ] No `fetch` calls outside `services/` — pages and components call service functions only
- [ ] Every service function has an explicit TypeScript return type
- [ ] No `any` — replace with typed interfaces in `client/src/types/`
- [ ] React Query keys defined as constants, not inline strings
- [ ] Shared stateful logic extracted into custom hooks, not copy-pasted across pages
- [ ] No prop drilling beyond 2 levels — use React Context or lift/co-locate state
- [ ] Loading state, error state, and empty state handled consistently in every data-fetching page
- [ ] Form validation centralised in React Hook Form schemas

---

## Phase 4 — Database & EF Core audit

Review `KanjiKaDbContext` and all entity configurations. Record issues in the same format as Phase 1.

- [ ] All relationship FKs are explicit properties (no shadow FK properties)
- [ ] Indexes exist for every FK column and every column used in a `Where` filter
- [ ] `SaveChangesAsync` called once per unit of work — not once per entity in a loop
- [ ] Delete strategy is consistent (hard delete throughout, or soft delete throughout — not mixed)
- [ ] No N+1: repositories use `.Include()` / `.ThenInclude()` or `.Select()` projections
- [ ] snake_case via `EFCore.NamingConventions` covers every entity (verify no column is bypassed)

---

## Phase 5 — Delegation

Once all plans are reviewed for consistency and completeness:

1. **Group items by risk and layer.** Batch low-risk, same-layer changes into a single agent call. Keep high-risk or cross-layer changes isolated so a failure is easy to diagnose.
2. **Spawn `backend-dev`** for each backend batch with the exact list of changes and file paths from Phase 2.
3. **Spawn `frontend-dev`** for each frontend batch with the exact list of changes and file paths from Phase 3.
4. **After each agent completes**, verify CI locally:
   - Backend: `cd server && dotnet build --configuration Release` then `dotnet test test/KanjiKa.UnitTests/`
   - Frontend: `cd client && npm run build` then `npm run test:coverage -- --run`
5. If any step fails, spawn `debugger` with the full error output before continuing.
6. Backend and frontend batches that are independent of each other may be spawned in parallel.

---

## Phase 6 — Post-refactor checklist

After all agents finish:

- [ ] `MANUAL_TEST.md` updated for any changed or removed behavior
- [ ] `docs/references.md` updated if any pattern was adapted from an external source
- [ ] No new `TODO` / `FIXME` comments introduced without an accompanying issue
- [ ] Swagger UI (`/swagger`) loads and all endpoints are documented with accurate response types
- [ ] `git diff` is clean — no debug artifacts, no accidentally committed secrets

---

## Guiding principles (apply throughout)

| Domain | Principles |
|---|---|
| General | DRY, YAGNI, SOLID (SRP and DIP especially) |
| C# / .NET | Async/await end-to-end; nullability; `Result<T>` for recoverable errors; guard clauses over nested ifs; extension methods for DI |
| REST API | Nouns not verbs; correct HTTP verbs and status codes; DTOs only; consistent error envelope; pagination on lists |
| EF Core | No lazy loading; explicit includes; one `SaveChangesAsync` per operation; `AsNoTracking()` for reads |
| React | Custom hooks for logic; services for API calls; React Query keys as constants; no prop drilling; strong types everywhere |
| TypeScript | No `any`; explicit return types on all exported functions; discriminated unions over string literals for state |
| Testing | Every refactor must keep CI green — rename means update tests, not delete them |
