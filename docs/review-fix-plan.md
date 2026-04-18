# PR #38 Review Fix Plan

Ordered by severity. Critical issues block merge; major/minor issues should be resolved before the milestone 2 branch is cut.

---

## Critical — fix before merging PR #38

### Fix 1: refreshToken DTO (broken endpoint)
**Files:** `server/src/KanjiKa.Api/Controllers/UsersController.cs`, new DTO

Create `server/src/KanjiKa.Application/DTOs/User/RefreshTokenRequest.cs`:
```csharp
public record RefreshTokenRequest(string Token, string RefreshToken);
```
Change the action signature:
```csharp
public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
```
Pass `request.Token` and `request.RefreshToken` to the service. Update `client/src/services/userService.ts` to send `{ token, refreshToken }`.

### Fix 2: resetCode field mismatch (reset password broken)
**File:** `client/src/services/userService.ts:88`

Change the client payload key from `code` to `resetCode` to match `ResetPasswordRequest.ResetCode`.
Alternatively, replace the framework DTO with a custom one that uses `Code`.

### Fix 3: unique index on username
**File:** `server/src/KanjiKa.Data/KanjiKaDbContext.cs`

Add in `OnModelCreating`:
```csharp
modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
```
Then run `dotnet ef migrations add AddUniqueUsernameIndex` and handle `DbUpdateException` in `UserService.Register`:
```csharp
catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("23505") == true)
    return RegisterDto { IsSuccess = false, ErrorMessage = "Username already exists." };
```

---

## Critical-security — fix promptly after merge

### Fix 4: user-enumeration oracle in ResetPassword
**File:** `UserService.cs:170–177`

Replace:
```csharp
if (user == null) return new ResetPasswordDto { IsSuccess = false, ErrorMessage = "User not found" };
```
With:
```csharp
if (user == null) return new ResetPasswordDto { IsSuccess = false, ErrorMessage = "Invalid reset code." };
```

### Fix 5: cryptographic reset code + rate limiting
**File:** `UserService.cs:149–151`

Replace `Random.Shared.Next(100000, 1000000).ToString()` with a hex token from `RandomNumberGenerator`:
```csharp
var bytes = RandomNumberGenerator.GetBytes(4);
var code = BitConverter.ToUInt32(bytes) % 900000 + 100000;
```
Add `builder.Services.AddRateLimiter(...)` in `Program.cs` with a fixed-window policy on `POST /resetPassword`.

### Fix 6: refresh-token flow — implement or remove
**Files:** `client/src/services/apiClient.ts`, `userService.ts`

Option A (implement): intercept 401 in `apiFetch`, call `/refreshToken`, store new tokens, retry the original request once, then log out on second 401.
Option B (remove): delete `refreshToken` from `userService.ts`, remove refresh-token storage from `AuthContext`, keep tokens short-lived with re-login.

---

## Major — fix in milestone 2

### Fix 7: invalidate refresh tokens on password change
**File:** `UserService.cs` — `ChangePassword` and `ResetPassword` methods

Add after `user.PasswordHash = ...`:
```csharp
user.RefreshToken = null;
user.RefreshTokenExpiry = null;
```

### Fix 8: CORS origins from configuration
**File:** `Program.cs:53–61`

```csharp
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:5173"];
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(p => p.WithOrigins(allowedOrigins)
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
```
Add `"Cors": { "AllowedOrigins": ["http://localhost:5173", "http://localhost:3000"] }` to `appsettings.Development.json` and set the Azure SWA hostname in production config / env var.

### Fix 9: replace IConfiguration in UserService with IOptions<T>
**Files:** `UserService.cs`, new `AuthOptions.cs`, `Program.cs`

Create `server/src/KanjiKa.Application/Options/AuthOptions.cs`:
```csharp
public class AuthOptions
{
    [Required] public string Key { get; init; } = "";
    [Required] public string Issuer { get; init; } = "";
    [Required] public string Audience { get; init; } = "";
    [Range(1, 365)] public int RefreshTokenExpirationDays { get; init; } = 7;
    [Required] public string FrontendBaseUrl { get; init; } = "";
}
```
Register in `Program.cs`:
```csharp
builder.Services.AddOptions<AuthOptions>()
    .Bind(builder.Configuration.GetSection("Auth"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
```
Inject `IOptions<AuthOptions>` into `UserService`.

### Fix 10: admin paged query — project counts in SQL
**File:** `UserRepository.cs:44–63`

Replace `.Include(u => u.Proficiencies).Include(u => u.LessonCompletions)` with a `Select()` projection:
```csharp
.Select(u => new AdminUserDto
{
    Id = u.Id, Username = u.Username, Role = u.Role,
    MustChangePassword = u.MustChangePassword,
    IsActive = u.IsActive,
    ProficiencyCount = u.Proficiencies.Count(),
    LessonCompletionCount = u.LessonCompletions.Count()
})
```

### Fix 11: UseForwardedHeaders for reverse-proxy HTTPS
**File:** `Program.cs`

Add before `UseAuthentication()`:
```csharp
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
```

### Fix 12: [AllowAnonymous] on public auth endpoints
**File:** `UsersController.cs`

Add `[AllowAnonymous]` to: `Login`, `Register`, `ForgotPassword`, `ResetPassword`, `RefreshToken`.

### Fix 13: server-side password length validation
**Files:** `ChangePasswordRequest.cs`, `RegisterDto.cs`

Add `[Required, MinLength(8)]` on `NewPassword`, `Password` properties, or validate in the service layer.

### Fix 14: safe JWT sub claim parsing
**Files:** `UsersController.cs`, `AdminController.cs`, `LessonsController.cs`

Extract a helper in a base controller or extension:
```csharp
protected IActionResult? TryGetUserId(out int userId)
{
    var raw = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (!int.TryParse(raw, out userId))
        return Unauthorized();
    return null;
}
```

### Fix 15: check JWT expiry client-side on mount
**File:** `client/src/context/AuthContext.tsx`

In the `INITIALIZE` effect, after `decodeJwtPayload(token)`, add:
```ts
const nowSec = Math.floor(Date.now() / 1000);
if (payload.exp && payload.exp < nowSec) {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    dispatch({ type: 'LOGOUT' });
    return;
}
```

---

## Minor — fix when convenient

| # | File | Fix |
|---|------|-----|
| 16 | `UserService.cs` | `catch (Exception ex)` + `_logger.LogWarning(ex, "Email send failed")` in Register and ForgotPassword |
| 17 | `UserService.cs:103` | Drop `username` param from `SendActivationEmailAsync` or derive a display name |
| 18 | DB | Add a cron job / Hangfire task to delete `IsActive = false` accounts older than 24 h |
| 19 | `userService.ts:29–37` | Delete the dead `resetPassword(email)` function |
| 20 | `App.tsx` | Move `<QueryClientProvider>` outside `<AuthProvider>` |
| 21 | `User.cs:27` + migrations | Change `DateTime` activation expiry fields to `DateTimeOffset` |
| 22 | `Program.cs` | Add `ValidateOnStart()` for JWT options to get a clear startup error instead of NRE |
| 23 | `CLAUDE.md` | Change "bcrypt" to "PBKDF2-SHA512" |

---

## Security tests to add

These should be added before milestone 2 in `KanjiKa.IntegrationTests/`:

- `ForgotPassword` returns same DTO for registered and unregistered email
- `ResetPassword` with expired code returns failure
- `ResetPassword` rate-limits after 5 attempts within 15 min
- `POST /admin/users` returns 403 with non-admin JWT
- `RefreshToken` with a revoked/null refresh token returns failure DTO
