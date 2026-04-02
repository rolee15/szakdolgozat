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

The summary table shows per-file metrics. Look for:
- **Red/failing rows** — files below threshold
- **Uncovered lines** — shown in the `Uncovered Line #s` column

## Common Actions

### Find which files need more tests
After running coverage, identify files with low statement/branch coverage and delegate to `test-writer` agent to add tests.

### Verify a specific file
```bash
cd client && npm run test:coverage -- --run -- --coverage.include="src/path/to/file.ts"
```

### Run tests in watch mode (dev)
```bash
cd client && npm run test
```

## Project Test Conventions

- Test files live in `client/test/` mirroring `client/src/`
- Framework: Vitest + @testing-library/react
- See `kanjika-testing` skill for full conventions
