# KanjiKa - Japanese Kana Learning Platform

KanjiKa is a full-stack web application for learning Japanese Hiragana and Katakana characters. It consists of a React TypeScript frontend (client) and a .NET 8 Web API backend (server) with PostgreSQL database.

**ALWAYS follow these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Working Effectively

### Prerequisites and Environment Setup
- Ensure you have Node.js 20+ (22 recommended), .NET 8 SDK, Docker, and docker compose available
- Use absolute paths when working with files: `/home/runner/work/szakdolgozat/szakdolgozat/`

### Initial Setup and Build Commands
**IMPORTANT: NEVER CANCEL builds or long-running commands. Build times are significant.**

1. **Client Setup (React + TypeScript + Vite):**
   ```bash
   cd /home/runner/work/szakdolgozat/szakdolgozat/client
   npm install  # Takes 1-2 minutes. NEVER CANCEL.
   ```

2. **Server Setup (.NET 8 API):**
   ```bash
   cd /home/runner/work/szakdolgozat/szakdolgozat/server
   dotnet restore  # Takes 1-2 minutes. NEVER CANCEL.
   ```

### Build Commands
**CRITICAL: Set timeout to 120+ minutes for Docker builds. DO NOT use default timeouts.**

1. **Client Build:**
   ```bash
   cd /home/runner/work/szakdolgozat/szakdolgozat/client
   npm run build  # Takes 2-3 minutes. NEVER CANCEL.
   ```

2. **Server Build:**
   ```bash
   cd /home/runner/work/szakdolgozat/szakdolgozat/server
   dotnet build --configuration Release  # Takes 1-2 minutes. NEVER CANCEL.
   ```

3. **Docker Builds (EXTREMELY SLOW):**
   ```bash
   # Client Docker build - MAY FAIL due to npm/Node 22 compatibility issues
   docker build -t kanjika-client ./client  # Takes 15+ minutes if successful. NEVER CANCEL.
   
   # Server Docker build - MAY FAIL due to network/NuGet issues
   docker build -t kanjika-server ./server  # Takes 15+ minutes if successful. NEVER CANCEL.
   ```

### Test Commands
1. **Client Tests (Fast):**
   ```bash
   cd /home/runner/work/szakdolgozat/szakdolgozat/client
   npm run test:coverage -- --run  # Takes 10-15 seconds
   npm run lint  # Takes 5 seconds
   ```

2. **Server Tests (Fast):**
   ```bash
   cd /home/runner/work/szakdolgozat/szakdolgozat/server
   dotnet test --collect:"XPlat Code Coverage"  # Takes 10-15 seconds
   ```

## Development Workflow

### Running the Application Locally

1. **Start Development Database:**
   ```bash
   cd /home/runner/work/szakdolgozat/szakdolgozat
   POSTGRES_DB=kanjika POSTGRES_USER=postgres POSTGRES_PASSWORD=postgres \
   docker compose -f docker-compose.dev.yaml --profile dev up db -d
   
   # Wait 15-20 seconds for database to be ready
   docker exec kanjika-dev-db pg_isready -U postgres
   ```

2. **Start API Server (KNOWN ISSUE with data seeding):**
   ```bash
   cd /home/runner/work/szakdolgozat/szakdolgozat/server/src/KanjiKa.Api
   ConnectionStrings__DefaultConnection="Host=localhost;Port=5433;Database=kanjika;Username=postgres;Password=postgres" \
   dotnet run
   ```
   **WARNING:** The API server may fail during startup due to database seeding issues with `EnsureDeletedAsync()`. This is a known issue that needs investigation.

3. **Start Client Development Server:**
   ```bash
   cd /home/runner/work/szakdolgozat/szakdolgozat/client
   npm run dev  # Starts on http://localhost:5173/
   ```

### Production Deployment
```bash
cd /home/runner/work/szakdolgozat/szakdolgozat
POSTGRES_DB=kanjika POSTGRES_USER=postgres POSTGRES_PASSWORD=postgres \
docker compose --profile prod up -d
```

## Validation and Testing

### CRITICAL: Manual Validation Requirements
After making changes, ALWAYS perform these validation steps:

1. **Build Validation:**
   - Run client build: `npm run build` (must complete successfully)
   - Run server build: `dotnet build --configuration Release` (must complete successfully)

2. **Test Validation:**
   - Run client tests: `npm run test:coverage -- --run` (all tests must pass)
   - Run server tests: `dotnet test` (all 82 tests must pass)

3. **Lint Validation:**
   - Run client lint: `npm run lint` (must pass with no errors)

4. **Functional Validation:**
   - Start development database and client server
   - Navigate to http://localhost:5173/
   - Test basic navigation: Home → Hiragana → Katakana → Lessons
   - Verify character grids load correctly
   - Test at least one learning workflow if API is available

### CI/CD Pipeline Commands
The GitHub Actions workflow (`.github/workflows/build.yml`) runs on Windows and includes:
- Client: `npm ci && npm run test:coverage -- --run`
- Server: `dotnet build && dotnet test` with coverage collection
- **Expected build time in CI: 5-10 minutes total**

## Key Project Structure

### Client (`/client/`)
- **Framework:** React 18 + TypeScript + Vite
- **Testing:** Vitest with good coverage (94%+ statements)
- **Styling:** Tailwind CSS
- **Key directories:**
  - `src/components/` - React components (layout, common, lessons)
  - `src/pages/` - Page components
  - `src/services/` - API service layer
  - `src/types/` - TypeScript type definitions
  - `test/` - Vitest test files

### Server (`/server/`)
- **Framework:** .NET 8 Web API with Entity Framework Core
- **Database:** PostgreSQL with Npgsql provider
- **Architecture:** Clean Architecture (Api → Core → Data)
- **Key directories:**
  - `src/KanjiKa.Api/` - Web API controllers and startup
  - `src/KanjiKa.Core/` - Domain entities and business logic
  - `src/KanjiKa.Data/` - EF Core DbContext and repositories
  - `test/` - xUnit test projects (Unit + Integration tests)

### Configuration Files
- `client/package.json` - Node.js dependencies and scripts
- `server/KanjiKa.sln` - .NET solution file
- `docker-compose.yaml` - Production containers
- `docker-compose.dev.yaml` - Development database
- `.github/workflows/build.yml` - CI/CD pipeline

## Known Issues and Workarounds

1. **API Server Startup Issue:** 
   - Data seeding fails with PostgreSQL timeout during `EnsureDeletedAsync()`
   - Workaround: Run API in Production mode to skip seeding, then manually set up database

2. **Docker Build Issues:**
   - Client Docker build may fail with npm errors on Node 22
   - Server Docker build may fail with NuGet network issues
   - Both builds are very slow (15+ minutes) even when successful

3. **Node Version Compatibility:**
   - Client prefers Node 22 but works with Node 20
   - Some npm warnings may appear but don't affect functionality

## Common Tasks Reference

### Repository Root Contents
```
.github/          # GitHub Actions workflows
client/           # React TypeScript frontend
server/           # .NET 8 Web API backend
docs/             # Project documentation
docker-compose.yaml         # Production containers
docker-compose.dev.yaml     # Development database
README.md         # Basic project info
```

### Environment Variables
- Client: `.env.development` sets `VITE_API_URL=https://localhost:7161/api`
- Server: Uses connection string via `ConnectionStrings__DefaultConnection`
- Database: Requires `POSTGRES_DB`, `POSTGRES_USER`, `POSTGRES_PASSWORD`

### Port Configuration
- Client dev server: `http://localhost:5173/`
- Server API: `https://localhost:7161/` (or HTTP port varies)
- Dev database: `localhost:5433`
- Prod database: `localhost:5432`

## Tips for Effective Development

1. **Always build and test before making changes** to understand current state
2. **Use long timeouts (120+ minutes) for Docker commands** in your scripts
3. **Check container status** with `docker ps` when database connection fails
4. **Wait for database readiness** after starting PostgreSQL containers
5. **Run linting before committing** to match CI requirements
6. **Document timing expectations** when you encounter slow operations
7. **Test manual scenarios** after code changes, not just automated tests

Remember: This is a learning application, so always test the actual user learning workflows when making changes to the core functionality.