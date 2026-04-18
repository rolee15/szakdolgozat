# KanjiKa Azure Deployment Plan

Target: low-cost production deployment for the ELTE Bachelor thesis demo.

---

## Service Selection

| Component | Azure Service | Tier | Cost |
|-----------|--------------|------|------|
| Frontend (SPA) | Azure Static Web Apps | **Free** | €0/mo |
| Backend (.NET API) | Azure Container Apps | Consumption (scale-to-zero) | €0/mo (free grant covers thesis traffic) |
| Database | Azure DB for PostgreSQL Flexible Server | Burstable B1ms, 32 GiB | €0 first 12 mo (new account), ~€12 after |
| Container registry | GitHub Container Registry (ghcr.io) | Free for public images | €0/mo |
| Log Analytics | Pay-as-you-go | First 5 GiB/mo free | €0/mo |
| **Total** | | | **€0 during thesis, ~€12 after** |

### Why these services

- **Static Web Apps** — zero config for Vite SPAs: free HTTPS cert, CDN, SPA fallback routing, built-in GH Actions integration.
- **Container Apps (Consumption)** — the API already ships as a Docker image; ACA runs it unchanged. Scale-to-zero eliminates idle billing. Free monthly grant (180k vCPU-s, 2M requests) covers demo traffic.
- **PostgreSQL Flexible Server B1ms** — managed, persistent, automated backups, Postgres 17 (exact match with dev). 12-month free offer on new Azure accounts covers the entire thesis window.
- **GHCR** — free for the repo owner; ACA can pull with a PAT stored as a secret.

---

## Prerequisites

1. Azure subscription (new account: 12-month free offer + €170/30-day credit).
2. `az` CLI ≥ 2.60, logged in: `az login`.
3. GitHub repo admin rights (to add secrets).
4. Strong random `JWT_KEY` (64+ bytes), SMTP credentials.

**Required code change before first deploy (see review-fix-plan.md Fix 8):**
CORS origins in `server/src/KanjiKa.Api/Program.cs:53–61` must be read from configuration, not hardcoded, so the Azure SWA hostname can be injected via env var.

---

## Shell Variables

```bash
RG=kanjika-rg
LOC=westeurope
ACA_ENV=kanjika-aca-env
ACA_APP=kanjika-api
SWA_NAME=kanjika-web
PG_SERVER=kanjika-pg
PG_DB=kanjika
PG_ADMIN=pgadmin
PG_PASSWORD='<generate-32-char-random>'
LAW=kanjika-logs
GITHUB_USER=rolee15
JWT_KEY='<generate-64-char-random>'
SMTP_PASSWORD='<smtp-password>'
```

---

## Step 1 — Resource group + Log Analytics workspace

```bash
az group create -n $RG -l $LOC
az monitor log-analytics workspace create -g $RG -n $LAW -l $LOC
```

---

## Step 2 — PostgreSQL Flexible Server

```bash
az postgres flexible-server create \
  --resource-group $RG --name $PG_SERVER --location $LOC \
  --tier Burstable --sku-name Standard_B1ms \
  --storage-size 32 --version 17 \
  --admin-user $PG_ADMIN --admin-password "$PG_PASSWORD" \
  --public-access 0.0.0.0 \
  --high-availability Disabled \
  --backup-retention 7

az postgres flexible-server db create -g $RG -s $PG_SERVER -d $PG_DB

# "Allow Azure services" rule — tighten to ACA static IP after Step 4
az postgres flexible-server firewall-rule create \
  -g $RG -n $PG_SERVER --rule-name AllowAzureServices \
  --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0
```

---

## Step 3 — Container Apps environment

```bash
LAW_ID=$(az monitor log-analytics workspace show -g $RG -n $LAW --query customerId -o tsv)
LAW_KEY=$(az monitor log-analytics workspace get-shared-keys -g $RG -n $LAW --query primarySharedKey -o tsv)

az containerapp env create \
  -n $ACA_ENV -g $RG -l $LOC \
  --logs-workspace-id $LAW_ID --logs-workspace-key $LAW_KEY
```

---

## Step 4 — Deploy the API

```bash
DB_CONN="Host=${PG_SERVER}.postgres.database.azure.com;Port=5432;Database=${PG_DB};Username=${PG_ADMIN};Password=${PG_PASSWORD};SSL Mode=Require;Trust Server Certificate=true"

az containerapp create \
  -n $ACA_APP -g $RG --environment $ACA_ENV \
  --image ghcr.io/${GITHUB_USER}/kanjika-api:latest \
  --target-port 8080 --ingress external \
  --min-replicas 0 --max-replicas 1 \
  --cpu 0.25 --memory 0.5Gi \
  --registry-server ghcr.io \
  --registry-username $GITHUB_USER \
  --registry-password "<PAT-with-read:packages>" \
  --secrets \
      db-connection="$DB_CONN" \
      jwt-key="$JWT_KEY" \
      smtp-password="$SMTP_PASSWORD" \
  --env-vars \
      ASPNETCORE_ENVIRONMENT=Production \
      ConnectionStrings__DefaultConnection=secretref:db-connection \
      Jwt__Key=secretref:jwt-key \
      Jwt__Issuer=https://${ACA_APP}.<region>.azurecontainerapps.io \
      Jwt__Audience=https://${SWA_NAME}.azurestaticapps.net \
      Email__Smtp__Host=<smtp-host> \
      Email__Smtp__Port=587 \
      Email__Smtp__Username=<smtp-user> \
      Email__Smtp__Password=secretref:smtp-password \
      Email__From=<from-address> \
      Cors__AllowedOrigins=https://${SWA_NAME}.azurestaticapps.net

# Capture API hostname for later
API_FQDN=$(az containerapp show -n $ACA_APP -g $RG --query properties.configuration.ingress.fqdn -o tsv)
echo "API: https://$API_FQDN/api"
```

---

## Step 5 — Static Web App (frontend)

```bash
az staticwebapp create \
  -n $SWA_NAME -g $RG -l westeurope \
  --source https://github.com/${GITHUB_USER}/szakdolgozat \
  --branch main \
  --app-location "/client" --output-location "dist" \
  --login-with-github

SWA_FQDN=$(az staticwebapp show -n $SWA_NAME -g $RG --query defaultHostname -o tsv)
echo "Frontend: https://$SWA_FQDN"
```

Create `client/staticwebapp.config.json` (new file, commit to repo):
```json
{
  "navigationFallback": { "rewrite": "/index.html" },
  "mimeTypes": { ".json": "application/json" }
}
```

---

## Step 6 — Wire frontend to the API URL

Update `client/.env.production`:
```
VITE_API_URL=https://<API_FQDN>/api
```

The SWA GitHub Actions build step also needs the env var (see Step 7).
Update the ACA env var `Cors__AllowedOrigins` if the SWA hostname differs from the placeholder.

---

## Step 7 — Database migrations on first deploy

The API calls `InitialiseDatabaseAsync()` / `EnsureCreatedAsync()` at startup — the schema is created automatically.

1. **For the first deploy only:** set `--min-replicas 1` so the container starts immediately and creates the schema.
2. Monitor: `az containerapp logs show -n $ACA_APP -g $RG --follow`
3. Once logs confirm successful startup, drop back to 0: `az containerapp update -n $ACA_APP -g $RG --min-replicas 0`
4. **Before milestone 2:** switch to `dbContext.Database.MigrateAsync()` with proper EF Core migrations for schema evolution.

---

## GitHub Actions CI/CD Additions

### Add to `.github/workflows/build.yml` — deploy-api job

```yaml
  deploy-api:
    name: Build & deploy API to Azure Container Apps
    needs: [backend-tests, frontend-tests]   # adjust to your actual job names
    if: github.ref == 'refs/heads/main' && github.event_name == 'push'
    runs-on: ubuntu-latest
    permissions:
      contents: read
      packages: write
      id-token: write
    steps:
      - uses: actions/checkout@v4

      - name: Log in to GHCR
        uses: docker/login-action@v3
        with:
          registry: ghcr.io
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}

      - name: Build & push API image
        uses: docker/build-push-action@v6
        with:
          context: ./server
          file: ./server/Dockerfile
          push: true
          tags: |
            ghcr.io/${{ github.repository_owner }}/kanjika-api:${{ github.sha }}
            ghcr.io/${{ github.repository_owner }}/kanjika-api:latest

      - name: Azure login (OIDC — no secrets needed)
        uses: azure/login@v2
        with:
          client-id:       ${{ secrets.AZURE_CLIENT_ID }}
          tenant-id:       ${{ secrets.AZURE_TENANT_ID }}
          subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}

      - name: Deploy new revision to Container App
        run: |
          az containerapp update \
            -n kanjika-api -g kanjika-rg \
            --image ghcr.io/${{ github.repository_owner }}/kanjika-api:${{ github.sha }}
```

### Extend SWA workflow (auto-generated in Step 5)

The `az staticwebapp create` command adds a workflow file `.github/workflows/azure-static-web-apps-*.yml`. Edit its build step to inject the API URL:

```yaml
      - name: Build And Deploy
        uses: Azure/static-web-apps-deploy@v1
        env:
          VITE_API_URL: https://<API_FQDN>/api
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN_... }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: upload
          app_location: "/client"
          output_location: "dist"
```

### Required GitHub Secrets

| Secret | How to get |
|--------|-----------|
| `AZURE_CLIENT_ID` | `az ad app create --display-name kanjika-deploy` then add federated credential for the repo |
| `AZURE_TENANT_ID` | `az account show --query tenantId` |
| `AZURE_SUBSCRIPTION_ID` | `az account show --query id` |
| `AZURE_STATIC_WEB_APPS_API_TOKEN_...` | Auto-added by `az staticwebapp create` |

Use OIDC (federated credentials) — no long-lived secret to rotate.

---

## Verification Checklist

- [ ] `https://<SWA_FQDN>` loads the React SPA
- [ ] Network tab shows requests to `https://<API_FQDN>/api/...` with HTTP 200
- [ ] Register → activation email arrives
- [ ] Login with activated account works, JWT is issued
- [ ] `az containerapp logs show` shows no startup errors
- [ ] `psql "host=<PG_SERVER>.postgres.database.azure.com ..."` shows expected tables
- [ ] GH Actions: push to `main` triggers both SWA and API deploy jobs

---

## Known Issues / Risks

1. **CORS** — must refactor `Program.cs` to read origins from config before first cloud deploy (Fix 8 in review-fix-plan.md).
2. **EnsureCreatedAsync hang** — known issue in CLAUDE.md; run first deploy in Production mode with `min-replicas=1`.
3. **Smtp settings naming** — verify `Email__Smtp__*` env var keys match what `SmtpEmailService` binds to before going live.
4. **JWT Issuer/Audience** — pin to actual ACA and SWA hostnames in the ACA env vars above.
5. **GHCR visibility** — if repo is private, mark the `kanjika-api` package as public in GitHub package settings, or store a PAT in ACA secrets for image pull.
6. **CI minutes** — moving the deploy job to `ubuntu-latest` saves minutes (Linux = 1× multiplier vs Windows = 2×). Current tests run on `windows-latest` — keep them there, but add the deploy job on `ubuntu-latest`.
