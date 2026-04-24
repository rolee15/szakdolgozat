---
name: kanjika-build
description: Build the KanjiKa frontend and/or backend for production or CI verification. Use when asked to "build the project", "check if it compiles", "verify the build", or "production build".
license: project-specific
---

# KanjiKa Build Guide

See `CLAUDE.md` for the current CI thresholds (backend test count, frontend coverage).

## Backend — from `server/`

```bash
dotnet build --configuration Release    # expect 0 errors (CS8618 nullable warnings on navs are expected)
dotnet test --collect:"XPlat Code Coverage"
```

## Frontend — from `client/`

```bash
npm install                              # first time or after package.json changes
npm run build                            # output → client/dist/
npm run test:coverage -- --run
npm run lint
```

## Full-stack verify (run before a PR)

```bash
cd server && dotnet build --configuration Release && dotnet test
cd ../client && npm run build && npm run test:coverage -- --run
```

## Common failures

| Error | Cause | Fix |
|-------|-------|-----|
| `CS0246: type not found` | Missing `using` or new dependency | Add `using` or `dotnet add package` |
| `TS2307: cannot find module '@/...'` | Wrong alias or missing file | Check path alias in `vite.config.ts` / `vitest.config.ts` |
| Coverage below threshold | New code without tests | Write tests for uncovered branches (see `vitest-coverage` / `dotnet-coverage`) |
| `dotnet restore` required | New NuGet packages | `dotnet restore` from `server/` |
