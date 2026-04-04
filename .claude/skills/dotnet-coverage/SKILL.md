---
name: dotnet-coverage
description: Run .NET xUnit test coverage for the KanjiKa backend and analyze results. Use when asked to "check backend coverage", "run dotnet tests", "show test results", or when CI test count (82+ tests) needs to be verified.
---

# .NET Test Coverage

This skill runs and analyzes backend test coverage for KanjiKa using xUnit and coverlet.

## When to Use

- User asks to run backend tests or check coverage
- CI is failing due to test failures or insufficient test count (82+ tests required)
- User wants to know which services/controllers are under-covered
- User asks "do backend tests pass?" or "how many tests are there?"

## How to Run Tests with Coverage

Always run from the `server/` directory:

```bash
cd server && dotnet test --collect:"XPlat Code Coverage"
```

Coverage reports are written to:
```
server/test/*/TestResults/<guid>/coverage.cobertura.xml
```

### Generate a human-readable report (optional)

Install the report generator tool once:
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Then generate HTML:
```bash
reportgenerator -reports:"server/test/**/coverage.cobertura.xml" -targetdir:"server/coverage" -reporttypes:Html
```

## CI Requirements

The CI pipeline expects:
- **82+ tests** passing across all test projects
- All tests must pass (no failures or skips)

## Test Project Structure

```
server/test/
├── KanjiKa.UnitTests/        # Pure unit tests with Moq mocks
└── KanjiKa.IntegrationTests/ # Integration tests (may require DB)
```

## Running Specific Tests

### Run only unit tests
```bash
cd server && dotnet test test/KanjiKa.UnitTests/
```

### Run only integration tests
```bash
cd server && dotnet test test/KanjiKa.IntegrationTests/
```

### Filter by test name
```bash
cd server && dotnet test --filter "FullyQualifiedName~ControllerName"
```

### Verbose output
```bash
cd server && dotnet test --logger "console;verbosity=detailed"
```

## Interpreting Output

- **Passed** — count must be 82+
- **Failed** — read the stack trace; use the `debugger` agent
- **Skipped** — investigate why tests are being skipped

## Branch coverage analysis

The cobertura XML tracks branch hits. To get a readable summary:

```bash
reportgenerator \
  -reports:"server/test/**/coverage.cobertura.xml" \
  -targetdir:"server/coverage-tmp" \
  -reporttypes:TextSummary
cat server/coverage-tmp/Summary.txt
```

The `Summary.txt` shows `Branch coverage: X%`. Open `server/coverage-tmp/index.html` for the per-file breakdown — red lines are uncovered, yellow lines are partially covered branches.

**Reading branch gaps without reportgenerator:**
For each changed method, manually enumerate branches by reading the code:
- Every `if / else` arm
- Every `?? / ?.` null path
- Every early `return` condition
- Every `throw` path
Then check whether a test exists that exercises that path.

## Common Actions

### Find untested branches
After generating the HTML report, look for **yellow-highlighted lines** (partial branch coverage) — these are more important than red lines (missed lines entirely). Yellow means the line runs but one branch in it does not.

### Close the loop
After identifying uncovered branches, write the missing unit tests in `server/test/KanjiKa.UnitTests/` following `kanjika-testing` conventions, then re-run:
```bash
cd server && dotnet test test/KanjiKa.UnitTests/ --logger "console;verbosity=normal"
```
Do not stop until all new tests pass and every branch in changed files is covered.

### Check test count quickly
```bash
cd server && dotnet test --list-tests 2>&1 | tail -5
```

## Project Test Conventions

- Framework: xUnit + Moq
- See `kanjika-testing` skill for full conventions on mocking, assertions, and naming
