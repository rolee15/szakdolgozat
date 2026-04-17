# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**KanjiKa** is a Japanese Hiragana/Katakana learning platform. It is a full-stack app with:
- **Frontend**: React 18 + TypeScript + Vite (`/client`)
- **Backend**: .NET 8 Web API + Entity Framework Core (`/server`)
- **Database**: PostgreSQL 17.2 (containerized)

## Commands

### Client (from `client/`)
```bash
npm install           # Install dependencies (1-2 min, never cancel)
npm run dev           # Dev server at http://localhost:5173/
npm run build         # Production build (2-3 min, never cancel)
npm run lint          # ESLint check
npm run test          # Vitest in watch mode
npm run test:coverage -- --run  # Single coverage run (CI-style)
```

### Server (from `server/`)
```bash
dotnet restore                         # Restore NuGet packages (1-2 min)
dotnet build --configuration Release   # Build solution (1-2 min)
dotnet test --collect:"XPlat Code Coverage"  # Run all xUnit tests
```

### Running locally

1. **Start the dev database:**
   ```bash
   POSTGRES_DB=kanjika POSTGRES_USER=postgres POSTGRES_PASSWORD=postgres \
   docker compose -f docker-compose.dev.yaml --profile dev up db -d
   # Wait 15-20s, then verify: docker exec kanjika-dev-db pg_isready -U postgres
   ```

2. **Start the API server** (from `server/src/KanjiKa.Api/`):
   ```bash
   ConnectionStrings__DefaultConnection="Host=localhost;Port=5433;Database=kanjika;Username=postgres;Password=postgres" \
   dotnet run
   ```
   > **Known issue**: Startup may fail due to `EnsureDeletedAsync()` timing out during data seeding. Workaround: run in Production mode to skip seeding.

3. **Start the client**: `npm run dev` (from `client/`)

## Architecture

### Backend — Clean Architecture (4 layers)
```
server/src/
├── KanjiKa.Api/         # Controllers, Program.cs, DI composition root
├── KanjiKa.Application/ # DTOs, interfaces, service implementations
├── KanjiKa.Domain/      # Domain entities, core interfaces (no dependencies)
└── KanjiKa.Data/        # EF Core DbContext (KanjiKaDbContext), repositories
server/test/
├── KanjiKa.UnitTests/
└── KanjiKa.IntegrationTests/
```

Controllers (`HiraganaController`, `KatakanaController`, `LessonsController`, `UsersController`, `GrammarController`, `KanjiController`, `ReadingController`, `PathController`, `AdminController`) call services defined in `KanjiKa.Application` (interfaces) and implemented there or in `KanjiKa.Data`. DB columns use snake_case via `EFCore.NamingConventions`.

**DB schema**: `Users`, `Characters` (kana with hiragana/katakana type), `Examples`, `Proficiencies` (User + Character composite key), `LessonCompletions` (User + Character composite key).

### Frontend — Service Layer Pattern
```
client/src/
├── App.tsx          # Router (React Router DOM v6) with all routes
├── pages/           # Route-level components
├── components/      # Shared UI (layout/, common/, lessons/)
├── services/        # API calls (kanaService, lessonService, userService) + routes.ts
└── types/           # TypeScript interfaces
client/test/         # Vitest tests mirroring src/ structure
```

API base URL is set via `VITE_API_URL` env var (dev: `https://localhost:7161/api`). The frontend uses TanStack React Query for data fetching and React Hook Form for forms.

### Authentication
JWT-based auth is fully implemented end-to-end. Backend uses `ITokenService` (JWT) and `IHashService` (bcrypt). Frontend validates tokens via auth context in `App.tsx`, uses `ProtectedRoute` for guarded routes, and reads the authenticated user's ID from context — no hardcoded IDs remain.

## Key Config Files
- `client/.env.development` / `.env.production` — Vite env vars
- `server/src/KanjiKa.Api/appsettings.Development.json` — DB connection string (port 5433)
- `server/src/KanjiKa.Api/appsettings.Production.json` — DB connection string (port 5432)
- `docker-compose.dev.yaml` — Dev database only (port 5433)
- `docker-compose.yaml` — Full production stack (port 5432)
- `.github/workflows/build.yml` — CI runs on Windows, expects all 217+ backend unit tests to pass and 94%+ frontend coverage

## Ports
| Service | Dev | Prod |
|---------|-----|------|
| Client | 5173 | 3000 (Docker) |
| API | 7161 (HTTPS) | 5000 |
| Database | 5433 | 5432 |

## Agent Routing

The project has six specialized agents in `.claude/agents/`. **Always delegate to the best-suited agent** rather than doing the work inline — either when the user explicitly asks for a sub-agent, or when you can detect that the task maps cleanly to one.

### Routing table

| Task type | Agent | Trigger signals |
| --- | --- | --- |
| Reviewing code, PRs, or diffs for quality/security/style | `code-reviewer` | "review", "check my code", "look at this PR", "is this correct" |
| Diagnosing a bug, error message, or wrong behavior | `debugger` | error message, stack trace, "why does X happen", "something is broken", unexpected output |
| Designing a new feature, planning a refactor, evaluating options | `architect` | "how should I implement", "design", "plan", "what's the best approach", "architect", multi-layer changes |
| Writing or expanding tests | `test-writer` | "write tests", "add coverage", "test this", "increase coverage", CI failure on coverage |
| Building React/TypeScript UI, components, pages, hooks | `frontend-dev` | files under `client/src/`, "add a page", "create a component", "fix the UI", React Query / Hook Form tasks |
| Building .NET endpoints, services, EF Core, DB migrations | `backend-dev` | files under `server/src/`, "add an endpoint", "create a migration", "fix the API", EF Core tasks |

### Rules

1. **Match before acting** — before writing any code or making edits, check whether the task fits one of the agents above. If it does, delegate.
2. **Multi-layer tasks** — if a feature spans both frontend and backend, start with `architect` to produce a plan, then delegate implementation to `frontend-dev` and `backend-dev` separately.
3. **Ambiguous requests** — if a request could map to multiple agents (e.g., "fix the login"), pick the layer where the bug most likely lives; if unclear, use `debugger` first.
4. **Explicit override** — if the user says "do it yourself" or "don't use an agent", proceed without delegating.
5. **Never split what belongs together** — don't use multiple agents for a single-file, single-concern change; only split when the task genuinely crosses concerns.

### Token efficiency rules

1. **Direct tools first** — for a single-file lookup (find a class, read a config), use `Glob`/`Grep`/`Read` directly. Spawn an agent only when the task needs multi-step reasoning or writes code.
2. **Model tiers** — agents already have the right model set; when calling `general-purpose` via the Agent tool, override with `model: haiku` for pure exploration/lookup tasks and `model: sonnet` (default) for code generation.
3. **Parallelize independent agents** — if frontend and backend work are both needed, spawn both agents in the same message, not sequentially.
4. **No re-explanation** — when delegating to an agent, include only the facts it needs (file paths, error text, what to change). Don't paste large code blocks the agent can read itself with tools.
5. **`/commit` skill** — use `/commit` for routine staging + push instead of a full agent call or manual multi-step git commands.

### Agent model reference

| Agent | Model | Why |
| --- | --- | --- |
| `architect` | `opus` | Complex multi-file planning, trade-off analysis |
| `frontend-dev` | `sonnet` | Code generation needs full reasoning |
| `backend-dev` | `sonnet` | Code generation needs full reasoning |
| `test-writer` | `sonnet` | Code generation needs full reasoning |
| `debugger` | `sonnet` | Root-cause analysis over unfamiliar code |
| `code-reviewer` | `haiku` | Pattern matching + textual feedback |

---

## Manual Test Checklist

`MANUAL_TEST.md` contains the regression test checklist for all user-facing features. **After every code change** (new feature, bug fix, refactor, UI change, API change), update `MANUAL_TEST.md`:

- **New feature**: Add test cases for the new functionality under the appropriate section, or create a new section.
- **Bug fix**: Add a test case that verifies the fix (the scenario that was previously broken).
- **Removed feature**: Remove the corresponding test cases.
- **Changed behavior**: Update affected test cases to reflect the new expected behavior.

Keep the checklist minimal — one checkbox per observable behavior, no redundant cases.

---

## Thesis Attribution Rules

These rules apply in **every session**. Whenever you write or adapt code from an external source (internet, documentation, tutorials, Stack Overflow, GitHub, etc.), you must:

### 1. Add an inline comment above the borrowed block

```
// [N] <Short description> — <URL> (accessed YYYY-MM-DD)
```

- Use `//` for TypeScript/TSX/C# files; use `/* [N] ... */` for CSS.
- `[N]` must match the reference number assigned in `docs/references.md`.
- Place the comment directly above the first line of the borrowed/adapted code.

### 2. Update `docs/references.md`

Add or update an IEEE-formatted entry:

```
[N] Author(s) or Organization, "Title or Page Name," *Site/Platform*, <URL>. Accessed: YYYY-MM-DD. License: <license if applicable>.
```

- Use the organization or site name when the author is unknown.
- Assign the next sequential number; never reuse or skip numbers.
- Add a `Used in:` note listing the file path(s) where the reference appears.

### 3. Scope

**Requires citation:**

- Code copied or adapted from docs, tutorials, blog posts, Stack Overflow, GitHub issues/gists/repos
- Algorithm or logic implementations based on a specific source
- Configuration snippets taken verbatim from official documentation

**Does NOT require citation:**

- Standard library calls and framework boilerplate
- Industry-standard patterns with no single authoritative source
- npm/NuGet packages (tracked via `package.json` / `.csproj` — but must be mentioned in the thesis bibliography)

**When in doubt, add a citation.**

---

## ELTE Bachelor Thesis Requirements

1. **Original authorship**: The majority of the codebase must be the student's own original work. Borrowed code must remain a minority and must always be cited per the rules above.
2. **License compliance**: Respect the license of any third-party code or data. Do not include GPL-licensed code without verifying compatibility. Record licenses in `docs/references.md`.
3. **Library attribution**: Significant third-party libraries (React Query, React Hook Form, Entity Framework Core, etc.) must appear in the thesis bibliography even though they do not require inline comments.
4. **Thesis language**: The thesis document is written in Hungarian. Code and code comments may be in English.
5. **AI assistance disclosure**: If code is substantially generated by an AI tool (including Claude Code), this must be disclosed in the thesis per ELTE's academic integrity policy. Claude Code assistance must be acknowledged in the thesis preface/acknowledgements.
6. **Data sources**: External datasets (e.g., KANJIDIC2, JMdict, KanjiVG) must be cited in both `docs/references.md` and the thesis bibliography, including their license.
