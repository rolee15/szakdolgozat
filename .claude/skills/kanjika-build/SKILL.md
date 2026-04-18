---
name: kanjika-build
description: Build the KanjiKa frontend and/or backend for production or CI verification. Use when asked to "build the project", "check if it compiles", "verify the build", or "production build".
license: project-specific
---

# KanjiKa Build Guide

---

## Backend build

```bash
# From server/ (builds the entire solution including tests)
dotnet build --configuration Release
```

Expected output: `Build succeeded. 0 Error(s)`

Pre-existing nullable reference warnings (CS8618 on navigation properties) are normal — they are not errors.

**Run backend tests:**
```bash
dotnet test --collect:"XPlat Code Coverage"
```

Expected: `Passed! - Failed: 0, Passed: 82` (or higher as tests are added)

---

## Frontend build

```bash
# From client/
npm install          # first time or after package.json changes
npm run build        # production build (output: client/dist/)
```

Expected output: `✓ built in Xs`

**Run frontend tests with coverage:**
```bash
npm run test:coverage -- --run
```

Expected: all tests pass, statement coverage ≥ **94%**

**Lint check:**
```bash
npm run lint
```

---

## CI checks (what GitHub Actions verifies)

The CI workflow (`.github/workflows/build.yml`) runs on `windows-latest` and checks:

| Check | Command | Threshold |
|-------|---------|-----------|
| Backend build | `dotnet build --configuration Release` | 0 errors |
| Backend tests | `dotnet test` | 82+ passing |
| Frontend build | `npm run build` | exit 0 |
| Frontend coverage | `npm run test:coverage -- --run` | ≥ 94% statements |

Run all four locally before opening a PR to avoid CI surprises.

---

## Quick full-stack verify (copy-paste)

```bash
# Backend
cd server && dotnet build --configuration Release && dotnet test

# Frontend
cd client && npm run build && npm run test:coverage -- --run
```

---

## Common build failures

| Error | Cause | Fix |
|-------|-------|-----|
| `CS0246: type not found` | Missing using / new dependency | Add `using` or `dotnet add package` |
| `TS2307: cannot find module '@/...'` | Wrong alias or file doesn't exist | Check path alias in `vitest.config.ts` / `vite.config.ts` |
| Coverage below 94% | New code without tests | Write tests for uncovered lines in `client/test/` |
| `npm run build` fails with `TS...` | TypeScript error in src | Fix the type error; `npm run lint` often shows it first |
| `dotnet restore` needed | New NuGet packages added | Run `dotnet restore` from `server/` |
