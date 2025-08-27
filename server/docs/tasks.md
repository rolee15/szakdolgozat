# KanjiKa Improvement Tasks Checklist

Below is an ordered, actionable checklist of improvements covering both architecture and code-level aspects. Each task
is intentionally concise and framed to be independently checkable. Check off [ ] as items are completed.

1. [ ] Establish solution-wide technical vision and conventions
    - [ ] Document architectural overview (Clean/Onion layering: Api -> Core -> Data) and dependencies in
      docs/architecture.md
    - [ ] Define coding standards (C# style, nullability, async/await, logging, error handling) in docs/conventions.md
    - [ ] Enable nullable reference types across all projects and address warnings

2. [ ] Introduce robust configuration management
    - [ ] Add strongly-typed options classes (e.g., JwtOptions, SmtpOptions) and bind via IConfiguration
    - [ ] Replace magic strings ("DefaultConnection", CORS policy names) with constants/options
    - [ ] Create appsettings.Development.json and appsettings.Benchmark.json with appropriate overrides

3. [ ] Implement proper authentication and authorization
    - [ ] Replace Dummy TokenService with JWT-based implementation (signing key, issuer, audience, expiration, refresh
      tokens)
    - [ ] Add ASP.NET Core Authentication/Authorization middleware and decorate endpoints with [Authorize] as needed
    - [ ] Store refresh tokens securely (DB table or cache) and implement rotation/invalidation

4. [ ] Harden API contracts and validation
    - [ ] Define request/response DTOs under Core.DTOs.Api and stop using Microsoft.AspNetCore.Identity.Data models
      directly
    - [ ] Align naming: service methods expect username but controllers use email; standardize to a single identity
      field (e.g., Username or Email) across DTOs and services
    - [ ] Add data annotations/FluentValidation for request validation and return ProblemDetails for validation errors
    - [ ] Ensure consistent response shapes; wrap primitives in DTOs where applicable (e.g., counts already present,
      keep consistent)

5. [ ] Standardize routing and REST semantics
    - [ ] Align query vs route parameters (e.g., lessons count/reviews endpoints use query userId while perf test uses
      /count/1). Choose one style (prefer route param) and update controllers/perf tests
    - [ ] Introduce API versioning (e.g., aspnet-api-versioning) and prefix routes with /api/v1
    - [ ] Ensure correct HTTP verbs and status codes (e.g., 201 for resource creation, 400/404/409 as applicable)

6. [ ] Centralize error handling and problem responses
    - [ ] Add global exception handling middleware (use RFC 7807 ProblemDetails)
    - [ ] Replace throw ArgumentException in services with domain-specific exceptions and map them in middleware to
      4xx/5xx responses
    - [ ] Ensure sensitive details are not leaked in error messages

7. [ ] Improve logging and observability
    - [ ] Introduce structured logging (Serilog or built-in ILogger scopes) with correlation IDs
    - [ ] Log key events at appropriate levels (auth, data seeding, failures) without PII leakage
    - [ ] Add minimal health checks (/health) and readiness/liveness endpoints

8. [ ] Strengthen data access patterns and EF Core usage
    - [ ] Add AsNoTracking for read-only queries and projections to DTOs server-side
    - [ ] Replace multi-step queries with single round-trips where possible; add indexes for frequent lookups (
      User.Username, Character.Symbol, Proficiency composite key already in place)
    - [ ] Evaluate and configure EF Core batching and connection pooling; use cancellation tokens in all db calls
    - [ ] Revisit seeding: avoid EnsureDeleted in Development; use EF migrations + conditional seeding

9. [ ] Adopt database migrations and lifecycle management
    - [ ] Add initial EF Core migrations and ensure CI applies migrations for test env
    - [ ] Provide scripts for local dev (PowerShell) to reset/apply migrations with seed data

10. [ ] Secure CORS and transport settings
    - [ ] Restrict CORS origins/methods/headers per environment; avoid AllowAnyOrigin in production
    - [ ] Re-enable HTTPS redirection and configure dev certificates or reverse proxy trust in containers

11. [ ] Introduce pagination, filtering, and sorting standards
    - [ ] Create a common PageRequest/PageResult DTO and apply consistently across listing endpoints (lessons,
      characters)
    - [ ] Validate bounds (pageSize limits) and default values centrally

12. [ ] Consistency and performance in Lesson domain
    - [ ] Extract "15 lessons per day" as configuration setting; move date/time to IClock abstraction for testability
    - [ ] Avoid N+1 in GetLessonsAsync by projecting with user proficiency in one query
    - [ ] Evaluate concurrency on LearnLessonAsync (unique constraint exists via composite key; ensure graceful 409 on
      duplicates)

13. [ ] Kana endpoints refinements
    - [ ] Validate and handle invalid KanaType parsing; avoid Enum.Parse throwing with try-parse + 400 response
    - [ ] Move mapping logic to dedicated mappers or Mapster/AutoMapper profile
    - [ ] Consider adding search/filter by proficiency level and type

14. [ ] Users and identity flow improvements
    - [ ] Normalize on username vs email; store and index whichever is chosen; update all services and DTOs
    - [ ] Implement password reset tokens (time-bound, single-use) instead of a hardcoded "12345"
    - [ ] HashService: review PBKDF2 parameters; make configurable and add unit tests for regression

15. [ ] Introduce cancellation tokens and timeouts
    - [ ] Add CancellationToken parameters to all async service methods/controllers and pass to EF calls
    - [ ] Configure HttpClient policies (timeouts/retries) where outbound calls may be added (e.g., email provider)

16. [ ] Testing strategy enhancements
    - [ ] Expand unit tests for services (happy paths, edge cases, error mapping)
    - [ ] Add integration tests covering controllers with in-memory test server and real EF Core provider (SQLite/Npgsql
      test container)
    - [ ] Fix performance tests to match actual routes and add BenchmarkDotNet configs with meaningful baselines

17. [ ] CI/CD and quality gates
    - [ ] Add GitHub Actions/Azure Pipelines to build, run tests, and publish coverage
    - [ ] Enable analyzers and StyleCop/FxCop, treat warnings as errors in CI
    - [ ] Add Dependabot or Renovate for dependency updates

18. [ ] Security review and hardening
    - [ ] Threat model authentication flows; store secrets via user-secrets or environment variables
    - [ ] Validate all inputs; protect against enumeration and timing attacks (hash verify already uses FixedTimeEquals)
    - [ ] Ensure no sensitive data is logged and that DTOs don’t leak internals

19. [ ] API documentation and discoverability
    - [ ] Enhance Swagger with XML comments, example payloads, response types, and auth configuration
    - [ ] Add README sections for running locally, seeding data, and invoking endpoints

20. [ ] Performance and scalability
    - [ ] Add caching where beneficial (e.g., Kana character lists) with proper invalidation
    - [ ] Consider read models/projections for frequently accessed queries
    - [ ] Add basic load/perf test scenarios and track regressions over time

21. [ ] Domain and DDD considerations
    - [ ] Move domain logic (proficiency increase/decrease thresholds, review scheduling) into domain services/value
      objects
    - [ ] Add invariants and guard clauses; replace magic numbers (SkillUp/SkillDown) with configuration or domain
      policies

22. [ ] Repository cleanup and structure
    - [ ] Ensure folder naming is consistent (Controllers vs KanaCharactersController class name)
    - [ ] Separate Api/Infrastructure/Core concerns cleanly; avoid Core referencing ASP.NET types
    - [ ] Add solution-level Directory.Build.props/targets for shared settings (nullable, analyzers, langversion)

23. [ ] Observability extras
    - [ ] Add basic metrics (request rate, latency, error rates) via OpenTelemetry and exporters (Console/OTLP)
    - [ ] Correlate DB timings and include EF query tags for slow query diagnostics

24. [ ] Containerization and deployment readiness
    - [ ] Review Dockerfile for multi-stage build, environment variables, ports, and non-root user
    - [ ] Externalize configuration via env vars and secrets; document deployment profiles
