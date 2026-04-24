---
name: refactor-backend
description: >
  Refactors the KanjiKa .NET 8 backend. Scans server/src/ for code smells and anti-patterns,
  then delegates targeted fixes to the backend-dev agent. Use when asked to "refactor the backend",
  "clean up C# code", "apply .NET best practices", "improve EF Core queries", or "fix architecture".
---

# Backend Refactor — .NET 8 + EF Core

**Plan — don't implement.** Produce a risk-ordered plan; delegate each item to `backend-dev`.

## Phase 0 — Scan

Read `server/src/KanjiKa.Api/Program.cs`, all controllers, the interfaces, DTOs, and entities under `KanjiKa.Application/` and `KanjiKa.Domain/`, `KanjiKaDbContext.cs`, and every repository and service implementation. Note each smell below per file.

## Phase 1 — Smells

### Clean Architecture

- EF Core (`DbSet`, `IQueryable`, EF namespaces) leaking into `Domain` or `Application`.
- Controllers calling repositories or `DbContext` directly (should go through services).
- Business rules in controllers.
- Concrete classes injected where an interface exists.
- Anemic entities (pure data bags, all behaviour in services) where domain logic would fit.

### Async / threading

- `.Result`, `.Wait()`, `.GetAwaiter().GetResult()`.
- Public async methods missing `CancellationToken` (not accepted, or accepted but not forwarded).
- Fire-and-forget (`_ = SomeAsync()`) without a justification comment.

### Error handling

- Exceptions used for expected failures (not-found, duplicate, validation).
- Swallowed exceptions (`catch {}`, `catch { return null; }`).
- Controllers returning `BadRequest("some string")` instead of a typed error DTO.
- Missing null guards at the service boundary.

### EF Core

- N+1 (DB calls inside a loop; missing `.Include()` for navs you'll read).
- Read-only query without `.AsNoTracking()`.
- `SaveChangesAsync` called once per entity in a loop instead of once per unit of work.
- Missing indexes on FK / frequently-filtered columns.
- Entity returned from a controller instead of a DTO.

### C# quality

- Magic strings for claim types, roles, or config keys.
- `!` suppressions without an explanatory comment.
- Methods > ~30 lines mixing concerns.
- Inline `AddScoped` chains in `Program.cs` instead of grouped extension methods.
- Private fields never reassigned but not `readonly`.

## Phase 2 — Plan

```
### B-<N>: <Short title>
Layer(s):  Api / Application / Domain / Data
Files:     <every file to touch>
Smell:     <from Phase 1>
Pattern:   <Result<T> | guard clause | extension method | AsNoTracking | Include | record DTO | ...>
Change:    <what to do>
Rationale: <why it's better>
Risk:      Low / Medium / High
```

Order low-risk first.

## Phase 3 — Rules

### Clean Architecture

- Dependency direction is `Api → Application → Domain ← Data`. `Domain` and `Application` have zero EF refs; `Application` depends only on `Domain`.
- Controllers: validate → call service → map → return `IActionResult`. No business logic.
- Register services via `IServiceCollection` extension methods, not inline chains.

### Async / cancellation

- Always `await`. Every public async method accepts a `CancellationToken` and forwards it to all EF/HttpClient calls.
- `ConfigureAwait(false)` in library code (`Application`/`Data`).

### Result pattern

Services return `Result<T>` for expected failures instead of throwing:

```csharp
public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public static Result<T> Success(T v) => new(true, v, null);
    public static Result<T> Failure(string e) => new(false, default, e);
    private Result(bool s, T? v, string? e) { IsSuccess = s; Value = v; Error = e; }
}
```

Controllers map to HTTP responses. Reserve `throw` for truly exceptional conditions.

### Nullability

- `<Nullable>enable</Nullable>` in every csproj. Use `T?` on returns that can be null; guard at the service boundary. No `!` without a comment.

### EF Core

- `AsNoTracking()` on reads. Eager-load with `.Include()` / `.ThenInclude()`, or project with `.Select()`. One `SaveChangesAsync` per unit of work. Index every FK and filter column. `DateTime.UtcNow` for timestamps.

### Code quality

- `record` for DTOs. Guard clauses over nested ifs. Extract magic strings to constants/enums. `readonly` on private fields set only in the constructor. Keep methods small.

### DI

- Correct lifetime (`Scoped` for EF repos, `Singleton` for stateless, `Transient` for lightweight). No service-locator anti-pattern.

## Phase 4 — Delegate & verify

1. Present the plan; wait for approval.
2. Batch low-risk, same-layer items into one `backend-dev` call; isolate high-risk ones.
3. After each batch: `dotnet build --configuration Release` + `dotnet test test/KanjiKa.UnitTests/`. Test count must not drop — if it does, spawn `debugger`.
4. Update `MANUAL_TEST.md`; cite external sources in `docs/references.md`.
