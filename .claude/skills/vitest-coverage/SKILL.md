---
name: vitest-coverage
description: Run Vitest test coverage for the KanjiKa frontend and analyze results. Use when asked to "check coverage", "run frontend tests", "show coverage report", or when CI coverage threshold (94%+) needs to be verified.
---

# Vitest Coverage

This skill runs and analyzes frontend test coverage for KanjiKa using Vitest.

## When to Use

- User asks to run frontend tests or check coverage
- CI is failing due to insufficient coverage (threshold: 94%+)
- User wants to know which files are under-covered
- User asks "what's the test coverage?" or "do tests pass?"

## How to Run Coverage

Always run from the `client/` directory:

```bash
cd client && npm run test:coverage -- --run
```

This produces:
- Console summary per file
- `client/coverage/` directory with detailed HTML report

## Coverage Threshold

The CI requires **94%+ coverage** across statements, branches, functions, and lines.

Files excluded from coverage (see `vite.config.ts` or `vitest.config.ts`):
- `src/main.tsx`
- `src/vite-env.d.ts`
- Type-only files

## Interpreting Output

The summary table shows per-file metrics:
```
File                | % Stmts | % Branch | % Funcs | % Lines | Uncovered Line #s
```

Priority order when reading the table:
1. **% Branch** — this is the column that most often falls short. A line can be covered while a branch in it is not.
2. **Uncovered Line #s** — maps to specific lines; check what construct is on that line (conditional, ternary, null check).
3. **% Stmts / % Lines** — secondary; if branch is 100%, these usually follow.

A row with `% Lines: 100 | % Branch: 80` means the code runs but some decision paths are never exercised — this is a real gap.

## Diagnosing uncovered branches

For each file with less than 100% branch coverage:

1. Look up the uncovered line numbers
2. Read that line in the source file
3. Identify the construct: `if`, `ternary`, `??`, `?.`, `&&` in JSX, early `return`
4. Determine which arm is never taken (usually the `false`/`null`/`error` arm)
5. Write a test that exercises that arm

## Common Actions

### Find which files need more tests
After running, identify files where `% Branch < 100`. Write tests for the uncovered branches — do not just write tests for uncovered lines, as that misses decision-path gaps.

### Verify a specific file
```bash
cd client && npm run test:coverage -- --run -- --coverage.include="src/path/to/file.ts"
```

### Run tests in watch mode (dev)
```bash
cd client && npm run test
```

### Close the loop
After identifying uncovered branches, write the missing tests (following `kanjika-testing` conventions), then re-run coverage to verify. Do not stop until `% Branch` is 100% for every changed file.

## Project Test Conventions

- Test files live in `client/test/` mirroring `client/src/`
- Framework: Vitest + @testing-library/react
- See `kanjika-testing` skill for full conventions
