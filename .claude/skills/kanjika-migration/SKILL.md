---
name: kanjika-migration
description: Create and apply EF Core database migrations for KanjiKa. Use when asked to "add a migration", "update the database", "create a migration", or whenever a DB schema change is needed.
license: project-specific
---

# KanjiKa EF Core Migrations

See `CLAUDE.md` for DB port and connection defaults. Migrations live in `server/src/KanjiKa.Data/Migrations/`.

## Prerequisites

```bash
dotnet tool install --global dotnet-ef     # or: dotnet tool update --global dotnet-ef
dotnet ef --version
```

## Standard commands

Run from `server/src/KanjiKa.Api/` (startup project). Copy the connection string from `appsettings.Development.json` — the example below matches the default dev config:

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5433;Database=kanjika;Username=postgres;Password=postgres"

# Add a migration
dotnet ef migrations add <MigrationName> \
  --project ../KanjiKa.Data/KanjiKa.Data.csproj \
  --startup-project .

# Apply pending migrations
dotnet ef database update \
  --project ../KanjiKa.Data/KanjiKa.Data.csproj \
  --startup-project .

# List migrations
dotnet ef migrations list \
  --project ../KanjiKa.Data/KanjiKa.Data.csproj \
  --startup-project .

# Roll back to a previous migration, then remove the unwanted file
dotnet ef database update <PreviousMigrationName> --project ../KanjiKa.Data/KanjiKa.Data.csproj --startup-project .
dotnet ef migrations remove --project ../KanjiKa.Data/KanjiKa.Data.csproj --startup-project .
```

Migration name style: descriptive PascalCase, e.g. `AddSrsFieldsToProficiency`, `AddKanjiTable`.

## Naming conventions

`UseSnakeCaseNamingConvention()` converts `UserId` → `user_id`, `PasswordHash` → `password_hash`, etc. Don't override with `[Column(...)]` unless you have a specific reason.

## New entity checklist

1. Create the entity class under `server/src/KanjiKa.Domain/Entities/` (correct sub-folder).
2. Add a `DbSet<T>` in `server/src/KanjiKa.Data/KanjiKaDbContext.cs`; configure relationships / composite keys in `OnModelCreating`.
3. `dotnet ef migrations add <Name>` → review the generated migration file.
4. `dotnet ef database update`.
5. Seed data in the appropriate seeder under `server/src/KanjiKa.Data/Seeders/` if needed.

Never hand-edit `KanjiKaDbContextModelSnapshot.cs` — EF regenerates it.
