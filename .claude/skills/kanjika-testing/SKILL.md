---
name: kanjika-testing
description: Project-specific testing patterns for KanjiKa. Use when writing or reviewing tests — both frontend (Vitest + @testing-library/react) and backend (.NET xUnit + Moq). Enforces the exact conventions used in this repo.
license: project-specific
---

# KanjiKa Testing Guide

Reference this skill whenever writing new tests or reviewing existing ones.

---

## CI Requirements

| Target | Threshold |
|--------|-----------|
| Frontend coverage | **94%+** (Vitest v8) |
| Backend test count | **82+ tests** must pass |

CI runs on `windows-latest`. Tests run without a real DB — integration tests use `CustomWebApplicationFactory` with an in-memory/SQLite setup.

---

## Frontend Tests (Vitest + @testing-library/react)

### File layout
- Test files live in `client/test/` **mirroring** `client/src/`
  - `client/src/pages/FooPage.tsx` → `client/test/pages/FooPage.test.tsx`
  - `client/src/services/fooService.ts` → `client/test/services/fooService.test.ts`
  - `client/src/components/bar/Baz.tsx` → `client/test/components/bar/Baz.test.tsx`
- File naming: `*.test.tsx` for React components/pages, `*.test.ts` for plain TS

### Coverage exclusions (do NOT write tests for these)
```
src/main.tsx
src/App.tsx
```

### Imports
```typescript
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent, waitFor } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
```

### Wrapping components

**Pages with routing only:**
```tsx
render(
  <MemoryRouter>
    <MyPage />
  </MemoryRouter>
)
```

**Pages/components that call `useQuery` / `useMutation`:**
```tsx
const queryClient = new QueryClient({
  defaultOptions: { queries: { retry: false } },
})
render(
  <QueryClientProvider client={queryClient}>
    <MemoryRouter>
      <MyPage />
    </MemoryRouter>
  </QueryClientProvider>
)
```

### Mocking services (vi.mock — MUST be at top of file, before any imports of the mock)
```typescript
vi.mock('@/services/kanaService', () => ({
  default: {
    getCharacters: vi.fn(),
    getCharacterDetail: vi.fn(),
  }
}))
import kanaService from '@/services/kanaService'

// In test:
const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
svc.getCharacters.mockResolvedValue([...])
```

### Mocking fetch directly (for service unit tests)
```typescript
function mockFetchOk(data: unknown) {
  const json = vi.fn().mockResolvedValue(data)
  const okResponse = { ok: true, json } as unknown as Response
  ;(globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(okResponse) as unknown as typeof fetch
}

function mockFetchNotOk() {
  const notOkResponse = { ok: false, json: vi.fn() } as unknown as Response
  ;(globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(notOkResponse) as unknown as typeof fetch
}
```

### Standard test lifecycle
```typescript
beforeEach(() => { vi.restoreAllMocks() })
afterEach(() => { vi.clearAllMocks() })
```

### Async assertions
```typescript
// Wait for element to appear
expect(await screen.findByText('...')).toBeInTheDocument()

// Wait for side-effect
await vi.waitFor(() => {
  expect(svc.someMethod).toHaveBeenCalledWith(...)
})
```

### Path aliases (configured in vitest.config.ts)
```
@/         → client/src/
@components → client/src/components/
@pages     → client/src/pages/
@services  → client/src/services/
```

---

## Backend Tests (xUnit + Moq)

### File layout
- Unit tests: `server/test/KanjiKa.UnitTests/` mirroring `server/src/`
  - `KanjiKa.Api/Services/FooService.cs` → `KanjiKa.UnitTests/Api/Services/FooServiceTest.cs`
  - `KanjiKa.Core/Entities/Bar/Baz.cs` → `KanjiKa.UnitTests/Core/Entities/Bar/BazTest.cs`
- Integration tests: `server/test/KanjiKa.IntegrationTests/`

### Standard unit test structure
```csharp
using KanjiKa.Api.Services;
using KanjiKa.Core.DTOs.SomeNamespace;
using KanjiKa.Core.Entities.SomeNamespace;
using KanjiKa.Core.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class FooServiceTest
{
    [Fact]
    public async Task MethodName_Condition_ExpectedResult()
    {
        // Arrange
        var repo = new Mock<IFooRepository>();
        var dep  = new Mock<ISomeDependency>();
        repo.Setup(r => r.GetAsync(1)).ReturnsAsync(new FooEntity { Id = 1 });

        var service = new FooService(repo.Object, dep.Object);

        // Act
        var result = await service.DoSomethingAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("expected", result.Value);
        repo.Verify();
    }
}
```

### Assertion style — use xUnit `Assert.*` (NOT FluentAssertions)
```csharp
Assert.True(result.IsSuccess)
Assert.False(result.IsSuccess)
Assert.Equal(expected, actual)
Assert.NotNull(result.ErrorMessage)
Assert.Null(result.Value)
repo.Verify()  // verifies all Verifiable() setups
```

### Moq patterns used in this repo
```csharp
// Return value
mock.Setup(m => m.GetAsync(id)).ReturnsAsync(entity);

// Return null (nullable)
mock.Setup(m => m.GetAsync("missing")).ReturnsAsync((MyType?)null);

// Verifiable Task
mock.Setup(m => m.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

// Any argument
mock.Setup(m => m.AddAsync(It.IsAny<MyEntity>())).Returns(Task.CompletedTask).Verifiable();

// Return tuple
mock.Setup(t => t.GenerateToken(userId)).Returns(("token", "refreshToken"));
```

### Test method naming convention
```
MethodName_Condition_ExpectedResult
// Examples:
Login_UserNotFound_ReturnsFailure
Register_NewUser_HashesAddsSavesAndReturnsTokens
CheckAnswer_CorrectAnswer_AdvancesSrsStage
```

### Integration test pattern
```csharp
// Inherit from IClassFixture<CustomWebApplicationFactory>
// Use SharedTestCollection
// See server/test/KanjiKa.IntegrationTests/Kana/GetKanaTest.cs for reference
```

---

## Branch Coverage Patterns

Branch coverage requires testing **every decision point**, not just reaching a line. A line can be executed while one of its branches stays uncovered.

### Frontend branch patterns

| Construct | Branches to test |
|-----------|-----------------|
| `if (x) { A } else { B }` | Test when `x` is truthy (A runs) AND when `x` is falsy (B runs) |
| `condition ? a : b` | Test both the `true` and `false` outcome |
| `x ?? fallback` | Test when `x` is `null`/`undefined` (fallback used) AND when `x` has a value |
| `x?.foo` | Test when `x` is `undefined` (returns `undefined`) AND when `x` is defined |
| `{condition && <Component />}` | Test when `condition` is falsy (nothing renders) AND truthy (renders) |
| `isLoading` state | Test the loading branch (query pending) separately from the loaded branch |
| Error state | Test when the query/mutation rejects — component must show an error message |
| Empty array `[]` | Test when the returned list is empty (empty state render) |

**Example — testing a conditional render:**
```tsx
it('does not render the panel when data is empty', async () => {
  svc.getData.mockResolvedValue([])
  render(<MyPage />)
  await waitFor(() => {
    expect(screen.queryByTestId('panel')).not.toBeInTheDocument()
  })
})

it('renders the panel when data is present', async () => {
  svc.getData.mockResolvedValue([{ id: 1, name: 'foo' }])
  render(<MyPage />)
  expect(await screen.findByTestId('panel')).toBeInTheDocument()
})
```

**Example — testing service non-ok branch:**
```typescript
it('throws when response is not ok', async () => {
  mockFetchNotOk()
  await expect(myService.getData()).rejects.toThrow()
})
```

### Backend branch patterns

| Construct | Branches to test |
|-----------|-----------------|
| `if (entity == null) return Failure` | Test when repo returns `null` → service returns failure |
| `if (!condition) throw` | Test the throw path AND the happy path |
| `switch (x) { case A: ... case B: ... default: ... }` | One test per case including `default` |
| `result ?? defaultValue` | Test when result is `null` (uses default) AND when it has a value |
| Early `return` inside a method | Test the condition that triggers the early return |
| `await Assert.ThrowsAsync<T>` | Required for every `throw` path |

**Example — testing the null-return branch:**
```csharp
[Fact]
public async Task GetProficiency_UserNotFound_ReturnsFailure()
{
    // Arrange
    var repo = new Mock<IKanjiRepository>();
    repo.Setup(r => r.GetProficiencyAsync(99, 1)).ReturnsAsync((KanjiProficiency?)null);
    var service = new KanjiService(repo.Object);

    // Act
    var result = await service.GetProficiencyAsync(99, 1);

    // Assert
    Assert.False(result.IsSuccess);
}
```

**Example — testing the throw path:**
```csharp
[Fact]
public async Task SubmitAnswer_InvalidStage_ThrowsArgumentOutOfRange()
{
    var service = new KanjiService(new Mock<IKanjiRepository>().Object);
    await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
        () => service.SubmitAnswerAsync(userId: 1, kanjiId: 1, stage: -1));
}
```

---

## New feature checklist

When implementing a new feature, always write:

**Frontend:**
- [ ] Service unit test (mocking fetch): covers success + non-ok responses
- [ ] Page/component test: covers render, loading state, error state, empty state
- [ ] Each conditional render branch tested separately
- [ ] Each user interaction tested with both success and failure responses
- [ ] Update existing tests if you changed component APIs (e.g. new props)
- [ ] Re-run `npm run test:coverage -- --run` and verify **% Branch** is 100% for changed files

**Backend:**
- [ ] Unit test for every new Service method: success path + all failure paths
- [ ] One test per `if/else` branch, one test per `null` return, one test per `throw`
- [ ] Unit test for new entity/DTO constructors if they have validation logic
- [ ] Integration test for new API endpoint (at minimum: happy path)
- [ ] Re-run `dotnet test` and verify all tests pass and count has increased

---

## Running tests locally

```bash
# Frontend (from client/)
npm run test:coverage -- --run    # single run with coverage report
npm run test                      # watch mode

# Backend (from repo root)
dotnet test .\server\KanjiKa.sln --collect:"XPlat Code Coverage"
```
