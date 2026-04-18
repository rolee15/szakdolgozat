---
name: kanjika-migration
description: Create and apply EF Core database migrations for KanjiKa. Use when asked to "add a migration", "update the database", "create a migration", or whenever a DB schema change is needed.
license: project-specific
---

# KanjiKa EF Core Migrations

---

## Prerequisites

The EF Core tools must be installed globally:

```bash
dotnet tool install --global dotnet-ef
# or update if already installed:
dotnet tool update --global dotnet-ef
```

Verify: `dotnet ef --version`

---

## Adding a new migration

Always run from `server/src/KanjiKa.Api/` (the startup project):

```bash
# From server/src/KanjiKa.Api/
ConnectionStrings__DefaultConnection="Host=localhost;Port=5433;Database=kanjika;Username=postgres;Password=postgres" \
dotnet ef migrations add <MigrationName> \
  --project ../KanjiKa.Data/KanjiKa.Data.csproj \
  --startup-project .
```

Migration name conventions:
- `AddSrsFieldsToProficiency`
- `AddKanjiTable`
- `AddNextReviewDateToLessonCompletion`

This creates files in `server/src/KanjiKa.Data/Migrations/`.

---

## Applying migrations (update the database)

```bash
# From server/src/KanjiKa.Api/
ConnectionStrings__DefaultConnection="Host=localhost;Port=5433;Database=kanjika;Username=postgres;Password=postgres" \
dotnet ef database update \
  --project ../KanjiKa.Data/KanjiKa.Data.csproj \
  --startup-project .
```

The dev database runs on port **5433** (not 5432).

---

## Rolling back a migration

```bash
# Roll back to a specific migration (by name):
ConnectionStrings__DefaultConnection="..." \
dotnet ef database update <PreviousMigrationName> \
  --project ../KanjiKa.Data/KanjiKa.Data.csproj \
  --startup-project .

# Then remove the unwanted migration file:
dotnet ef migrations remove \
  --project ../KanjiKa.Data/KanjiKa.Data.csproj \
  --startup-project .
```

---

## Listing existing migrations

```bash
dotnet ef migrations list \
  --project ../KanjiKa.Data/KanjiKa.Data.csproj \
  --startup-project .
```

---

## Where migrations live

```
server/src/KanjiKa.Data/Migrations/
├── <timestamp>_InitialCreate.cs
├── <timestamp>_InitialCreate.Designer.cs
└── KanjiKaDbContextModelSnapshot.cs
```

`KanjiKaDbContextModelSnapshot.cs` is auto-generated — never edit it by hand.

---

## Naming conventions (EFCore.NamingConventions)

The project uses `UseSnakeCaseNamingConvention()`. EF Core automatically converts:
- `UserId` → `user_id`
- `PasswordHash` → `password_hash`
- `NextReviewDate` → `next_review_date`

Do NOT add `[Column("...")]` attributes unless you need to override this.

---

## Adding a new entity — checklist

1. Create entity in `server/src/KanjiKa.Core/Entities/`
2. Add `DbSet<T>` to `KanjiKaDbContext` in `server/src/KanjiKa.Data/KanjiKaDbContext.cs`
3. Configure composite keys or relationships in `OnModelCreating` if needed
4. Run `dotnet ef migrations add <Name>` (command above)
5. Run `dotnet ef database update` to apply
6. Seed data in `KanjiKaDataSeeder.SeedAsync()` if needed
