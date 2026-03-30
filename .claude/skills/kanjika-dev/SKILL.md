---
name: kanjika-dev
description: Start the KanjiKa local development stack. Use when asked to "run the app", "start the dev server", "launch locally", or "how do I run this".
license: project-specific
---

# KanjiKa Local Development

Follow these steps in order. Each must succeed before starting the next.

---

## Step 1 — Start the database

```bash
# From repo root
POSTGRES_DB=kanjika POSTGRES_USER=postgres POSTGRES_PASSWORD=postgres \
docker compose -f docker-compose.dev.yaml --profile dev up db -d
```

Wait ~15-20 seconds, then verify it is ready:

```bash
docker exec kanjika-dev-db pg_isready -U postgres
# Expected: /var/run/postgresql:5432 - accepting connections
```

If the container doesn't exist yet, run `docker compose -f docker-compose.dev.yaml --profile dev up db -d` first (it creates it). If it already exists and is stopped, use `docker start kanjika-dev-db`.

**Port**: 5433 (mapped from 5432 inside the container)

---

## Step 2 — Start the API

```bash
# From server/src/KanjiKa.Api/
ConnectionStrings__DefaultConnection="Host=localhost;Port=5433;Database=kanjika;Username=postgres;Password=postgres" \
dotnet run
```

API listens at: `https://localhost:7161/api`

**Known issue**: First startup may hang during data seeding (`EnsureDeletedAsync` timeout). If it stalls, kill it and run with `ASPNETCORE_ENVIRONMENT=Production dotnet run` to skip seeding. The DB will be empty but the API will start.

---

## Step 3 — Start the client

```bash
# From client/
npm run dev
```

Client listens at: `http://localhost:5173/`

The client's `VITE_API_URL` is already set to `https://localhost:7161/api` in `client/.env.development`.

---

## Port reference

| Service  | Dev URL                        |
|----------|-------------------------------|
| Client   | http://localhost:5173          |
| API      | https://localhost:7161/api     |
| Database | localhost:5433                 |
| Swagger  | https://localhost:7161/swagger |

---

## Stopping the stack

```bash
# Stop the database container
docker stop kanjika-dev-db

# Stop API / client: Ctrl+C in their respective terminals
```

---

## Troubleshooting

| Symptom | Fix |
|---------|-----|
| `pg_isready` fails after 20s | `docker logs kanjika-dev-db` to diagnose; try `docker restart kanjika-dev-db` |
| API 401 on all requests | Token expired or missing — log in again via `/login` |
| API 500 on startup | Check DB connection string; ensure port 5433 is not in use by another process |
| Client can't reach API | Accept the self-signed certificate by visiting `https://localhost:7161` directly in the browser |
