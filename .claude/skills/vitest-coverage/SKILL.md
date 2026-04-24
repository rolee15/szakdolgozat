---
name: vitest-coverage
description: Run Vitest test coverage for the KanjiKa frontend and analyze results. Use when asked to "check coverage", "run frontend tests", "show coverage report", or when the CI coverage threshold needs to be verified.
---

# Vitest Coverage

See `CLAUDE.md` for the current CI coverage threshold. Framework: Vitest + @testing-library/react. Excluded files (entry, type-only) are configured in `vite.config.ts` / `vitest.config.ts`.

## Run

From `client/`:

```bash
npm run test:coverage -- --run
```

Produces a console summary per file plus an HTML report in `client/coverage/`.

For a single file / folder:

```bash
npm run test:coverage -- --run -- --coverage.include="src/path/to/file.ts"
```

Watch mode (dev, no coverage):

```bash
npm run test
```

## Reading the output

Per-file columns: `% Stmts | % Branch | % Funcs | % Lines | Uncovered Line #s`. Priority:

1. **% Branch** — most often the gap; a line can be 100% covered while a branch in it isn't.
2. **Uncovered Line #s** — open the source and identify the construct (`if`, ternary, `??`, `?.`, `&&` in JSX, early return).
3. **% Stmts / % Lines** — usually follow branch.

`% Lines: 100 | % Branch: 80` means some decision path never runs — a real gap.

## Closing gaps

For each file below 100% branch:

1. Look up the uncovered line numbers in the source.
2. Identify which arm is missing (usually the `false` / `null` / error arm).
3. Add one focused test per missing arm in `client/test/` (mirrors `client/src/`).
4. Re-run coverage and repeat until changed files reach 100% branch coverage.
