---
name: refactor-backend
description: >
  Refactors the KanjiKa .NET 8 backend. Scans server/src/ for code smells and anti-patterns,
  then delegates targeted fixes to the backend-dev agent. Use when asked to "refactor the backend",
  "clean up C# code", "apply .NET best practices", "improve EF Core queries", or "fix architecture".
---

# Backend Refactor — .NET 8 + EF Core + PostgreSQL

You are auditing and planning the refactor of the KanjiKa backend (`server/`). **Do not write code yourself.** Produce a concrete, risk-ordered plan and then delegate each item to the `backend-dev` agent.

---

## Phase 0 — Codebase scan

Read every file in this order:

1. `server/src/KanjiKa.Api/Program.cs`
2. All files in `server/src/KanjiKa.Api/Controllers/`
3. All files in `server/src/KanjiKa.Core/Interfaces/`
4. All files in `server/src/KanjiKa.Core/DTOs/`
5. All files in `server/src/KanjiKa.Core/Entities/`
6. `server/src/KanjiKa.Data/KanjiKaDbContext.cs`
7. All files in `server/src/KanjiKa.Data/Repositories/`
8. All files in `server/src/KanjiKa.Api/Services/`

For each file, note every instance of the smells catalogued below.

---

## Phase 1 — Smell catalogue

### Clean Architecture smells

| Smell | What to look for |
|---|---|
| EF Core leaking into Core | `DbSet`, `IQueryable`, or any EF namespace imported in `KanjiKa.Core` |
| Controller touching infrastructure | Controller directly calling a repository or `DbContext` |
| Logic in controllers | Business rules in a controller action — belongs in a service |
| Anemic domain model | Entities that are pure data bags with all behaviour in services |
| Missing interface | A concrete class injected directly instead of its interface |

### Async / threading smells

| Smell | What to look for |
|---|---|
| Blocking async | `.Result`, `.Wait()`, `.GetAwaiter().GetResult()` anywhere in application code |
| Sync-over-async | A non-async method calling an async one and blocking |
| Missing `CancellationToken` | Public async methods that do not accept and forward `CancellationToken` |
| Fire-and-forget | `_ = SomeAsync()` without a comment explaining why it is intentional |

### Error handling smells

| Smell | What to look for |
|---|---|
| Exceptions for control flow | `throw new Exception("not found")` instead of returning a failure result |
| Swallowed exceptions | Empty `catch {}` or `catch (Exception) { return null; }` |
| Untyped error responses | Controllers returning `BadRequest("some string")` instead of a structured DTO |
| Missing null guards | Service methods that can receive null inputs without checking |

### EF Core smells

| Smell | What to look for |
|---|---|
| N+1 queries | Looping over a collection and calling the DB inside the loop |
| Missing `AsNoTracking` | Read-only queries without `.AsNoTracking()` |
| Missing `.Include()` | Navigation properties accessed after the initial query (lazy-loading trap) |
| Multiple `SaveChangesAsync` per request | Called once per entity in a loop — must be called once per unit of work |
| Missing indexes | FK columns or frequently-filtered columns without a `HasIndex` in `OnModelCreating` |
| Entity exposed directly | Controller returns an EF entity instead of a DTO |

### C# code quality smells

| Smell | What to look for |
|---|---|
| Magic strings | Hard-coded role names, claim types, config keys not extracted to constants |
| Nullable suppression | `!` operator used to suppress nullable warnings without an explanatory comment |
| Long methods | Methods > ~30 lines mixing multiple concerns |
| Inline DI registration | Services registered inline in `Program.cs` rather than via extension methods |
| Missing `readonly` | Private fields that are never reassigned but not declared `readonly` |

---

## Phase 2 — Refactor plan

Produce a numbered list ordered by risk (lowest first). For each item:

```
### B-<N>: <Short title>

Layer(s):  Api / Core / Data
Files:     <list every file to touch>
Smell:     <which smell from Phase 1>
Pattern:   <Result<T> | guard clause | extension method | AsNoTracking | Include | record DTO | ...>
Change:    <what to do, precisely>
Rationale: <why this is better — cite the rule below if applicable>
Risk:      Low / Medium / High
```

---

## Phase 3 — Best-practice checklist

Apply every rule below when reviewing and when briefing `backend-dev`:

### Clean Architecture rules
*(Source: [Clean Architecture in .NET 8](https://medium.com/@madu.sharadika/clean-architecture-in-net-8-web-api-483979161c80), [ISE Developer Blog](https://devblogs.microsoft.com/ise/next-level-clean-architecture-boilerplate/))*

- Dependency direction is **strictly enforced**: `Api` → `Core` ← `Data`. `Core` never references `Api` or `Data`.
- `KanjiKa.Core` contains: entities, interfaces, DTOs. Zero EF Core references.
- `KanjiKa.Data` contains: `DbContext`, repository implementations, migrations.
- `KanjiKa.Api` contains: controllers, application services, DI wiring.
- Controllers must be **thin**: validate input, call a service method, map the result to a response DTO, return an `IActionResult`. No business logic.
- Register all services via **extension methods** on `IServiceCollection`, not inline in `Program.cs`:
  ```csharp
  // ✅ Good
  builder.Services.AddApplicationServices();
  builder.Services.AddDataServices(builder.Configuration);

  // ❌ Bad — 40 lines of AddScoped/AddTransient in Program.cs
  ```

### Async / cancellation rules
*(Source: [.NET Best Practices 2025](https://tabdelta.com/net-best-practices-every-developer-cto-follow-in-2025/))*

- **No `.Result` or `.Wait()`** — always `await`
- Every public async method in a controller or service must accept a `CancellationToken` and forward it to all EF Core and HttpClient calls:
  ```csharp
  public async Task<Result<UserDto>> GetUserAsync(int id, CancellationToken ct)
      => await _repo.GetByIdAsync(id, ct);
  ```
- Use `ConfigureAwait(false)` in library code (Core/Data layers) to avoid deadlocks in sync contexts

### Result pattern rules
*(Source: [.NET Best Practices 2025](https://tabdelta.com/net-best-practices-every-developer-cto-follow-in-2025/))*

- Services **never throw** for expected business failures (entity not found, duplicate, validation). They return `Result<T>`:
  ```csharp
  public sealed class Result<T>
  {
      public bool IsSuccess { get; }
      public T? Value { get; }
      public string? Error { get; }

      private Result(bool success, T? value, string? error) { ... }

      public static Result<T> Success(T value) => new(true, value, null);
      public static Result<T> Failure(string error) => new(false, default, error);
  }
  ```
- Controllers map results to HTTP responses:
  ```csharp
  var result = await _service.GetAsync(id, ct);
  return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
  ```
- Reserve `throw` for truly **exceptional** conditions (null reference from a coding bug, unrecoverable infrastructure failure)

### Nullability rules
*(Source: [.NET Best Practices 2025](https://tabdelta.com/net-best-practices-every-developer-cto-follow-in-2025/))*

- `<Nullable>enable</Nullable>` in every `.csproj`
- Mark nullable returns with `?`: `Task<User?>`, never `Task<User>` when null is a valid outcome
- Guard against null at the **service boundary**, not scattered throughout:
  ```csharp
  // ✅ Guard clause at the top
  public async Task<Result<CharacterDto>> GetAsync(int id, CancellationToken ct)
  {
      var entity = await _repo.GetByIdAsync(id, ct);
      if (entity is null) return Result<CharacterDto>.Failure("Character not found");
      return Result<CharacterDto>.Success(entity.ToDto());
  }
  ```
- Never use `!` to suppress a warning without a comment explaining why it is safe

### EF Core rules
*(Source: [Scalable .NET 8 Web API guide](https://dev.to/iamcymentho/building-a-scalable-net-8-web-api-clean-architecture-cqrs-jwt-postgresql-redis-a-5bpj), [.NET Best Practices 2025](https://tabdelta.com/net-best-practices-every-developer-cto-follow-in-2025/))*

- **`AsNoTracking()`** on every read-only query — tracking is only needed when you will call `SaveChangesAsync`:
  ```csharp
  return await _context.Characters
      .AsNoTracking()
      .Where(c => c.Type == type)
      .ToListAsync(ct);
  ```
- **Eager load** with `.Include()` / `.ThenInclude()` — never access a navigation property after the query returns (lazy loading disabled):
  ```csharp
  // ✅ Eager
  .Include(u => u.Proficiencies).ThenInclude(p => p.Character)

  // ❌ N+1 — second query per user in the loop
  foreach (var user in users) { _ = user.Proficiencies; }
  ```
- **Project with `.Select()`** when you only need a subset of columns — avoids fetching entire entities:
  ```csharp
  .Select(c => new CharacterDto { Id = c.Id, Value = c.Value })
  ```
- **One `SaveChangesAsync` per unit of work** — batch all changes, then save once
- **Index every FK and filter column** in `OnModelCreating`:
  ```csharp
  entity.HasIndex(p => p.UserId).HasDatabaseName("IX_Proficiencies_UserId");
  entity.HasIndex(p => new { p.UserId, p.CharacterId }).IsUnique();
  ```
- **`DateTime.UtcNow`** for all timestamps — never `DateTime.Now`; override `SaveChangesAsync` to centralise audit fields

### C# code quality rules
*(Source: [Clean Architecture in .NET 8](https://medium.com/@madu.sharadika/clean-architecture-in-net-8-web-api-483979161c80))*

- Use **`record`** for DTOs — immutable, structural equality, concise:
  ```csharp
  public record CharacterDto(int Id, string Value, string Type, string Romanisation);
  ```
- **Guard clauses over nested ifs** — return early, keep the happy path at the bottom of the method:
  ```csharp
  // ✅ Guard clause
  if (user is null) return Result<TokenDto>.Failure("User not found");
  if (!_hash.Verify(dto.Password, user.PasswordHash)) return Result<TokenDto>.Failure("Invalid password");
  // happy path continues...
  ```
- Extract all magic strings to `static class Constants` or enums:
  ```csharp
  public static class ClaimTypes { public const string UserId = "sub"; }
  ```
- All private fields that are set only in the constructor must be **`readonly`**
- Keep methods under ~30 lines; if a method is longer, extract private helper methods

### Dependency injection rules

- Register with the **correct lifetime**: `AddScoped` for request-scoped services (EF repos), `AddSingleton` for stateless services (token generation), `AddTransient` for lightweight stateless
- Never use the **service locator** anti-pattern — inject dependencies through the constructor, not via `IServiceProvider.GetService<T>()` at runtime

---

## Phase 4 — Delegation

1. Present the full plan to the user and wait for approval.
2. Group low-risk, same-layer changes into one `backend-dev` call. Isolate high-risk changes.
3. After each agent call, verify:
   ```bash
   cd server && dotnet build --configuration Release
   cd server && dotnet test test/KanjiKa.UnitTests/ --logger "console;verbosity=normal"
   ```
   Test count must not drop. If it does, spawn `debugger` before continuing.
4. After all changes: update `MANUAL_TEST.md`; add citations to `docs/references.md` if any pattern was adapted from an external source.
