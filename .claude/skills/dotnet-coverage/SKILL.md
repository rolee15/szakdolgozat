---
name: dotnet-coverage
description: Run .NET xUnit test coverage for the KanjiKa backend and analyze results. Use when asked to "check backend coverage", "run dotnet tests", "show test results", or when the CI test count needs to be verified.
---

# .NET Test Coverage

See `CLAUDE.md` for the current CI test-count threshold. Framework: xUnit + Moq + coverlet.

## Run tests with coverage

From `server/`:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Reports: `server/test/*/TestResults/<guid>/coverage.cobertura.xml`.

## Human-readable branch summary

Install once:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Then:

```bash
reportgenerator \
  -reports:"server/test/**/coverage.cobertura.xml" \
  -targetdir:"server/coverage-tmp" \
  -reporttypes:TextSummary
cat server/coverage-tmp/Summary.txt
```

Open `server/coverage-tmp/index.html` for per-file drill-down. Yellow lines = partial branch coverage (higher priority than red lines).

## Targeted runs

```bash
# Unit or integration project only
dotnet test test/KanjiKa.UnitTests/
dotnet test test/KanjiKa.IntegrationTests/

# Filter by name
dotnet test --filter "FullyQualifiedName~ControllerName"

# Verbose
dotnet test --logger "console;verbosity=detailed"

# Quick test count
dotnet test --list-tests 2>&1 | tail -5
```

## Reading the results

- Confirm `Passed` count meets the CI threshold in `CLAUDE.md`; no `Failed`, no unexpected `Skipped`.
- On failure, hand the stack trace to the `debugger` agent.
- For each changed method, enumerate branches (if/else arms, `??` null/non-null, early `return`, `throw`) and check every one has a test. Missing branches → write tests in `server/test/KanjiKa.UnitTests/` and re-run until changed files are fully covered.
