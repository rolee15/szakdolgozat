using System.Text.Json;
using KanjiKa.Api.Services;
using Microsoft.Extensions.Configuration;

namespace KanjiKa.UnitTests.Core.Services;

public class TokenServiceTest
{
    private static IConfiguration BuildConfig(int expiryMinutes = 60)
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "KanjiKa-Test-Secret-Key-At-Least-32-Chars!!",
            ["Jwt:Issuer"] = "KanjiKa",
            ["Jwt:Audience"] = "KanjiKa",
            ["Jwt:AccessTokenExpirationMinutes"] = expiryMinutes.ToString(),
            ["Jwt:RefreshTokenExpirationDays"] = "7"
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

    [Fact]
    public void TokenService_GenerateToken_ReturnsNonEmptyTokens()
    {
        // Arrange
        var tokenService = new TokenService(BuildConfig());
        const int userId = 1;
        const string username = "testuser";

        // Act
        (string accessToken, string refreshToken) = tokenService.GenerateToken(userId, username);

        // Assert
        Assert.Multiple(
            () => Assert.NotEmpty(accessToken),
            () => Assert.NotEmpty(refreshToken)
        );
    }

    [Fact]
    public void TokenService_GenerateToken_ReturnsDifferentRefreshTokensEachCall()
    {
        // Arrange
        var tokenService = new TokenService(BuildConfig());

        // Act
        var (_, refresh1) = tokenService.GenerateToken(1, "user");
        var (_, refresh2) = tokenService.GenerateToken(1, "user");

        // Assert
        Assert.NotEqual(refresh1, refresh2);
    }

    [Fact]
    public void GenerateToken_ReturnsValidJwtAccessToken()
    {
        // Arrange
        var tokenService = new TokenService(BuildConfig());

        // Act
        var (accessToken, _) = tokenService.GenerateToken(1, "user");

        // Assert — a JWT has exactly 3 dot-separated base64url parts
        string[] parts = accessToken.Split('.');
        Assert.Equal(3, parts.Length);
        Assert.All(parts, part => Assert.NotEmpty(part));
    }

    [Fact]
    public void GenerateToken_ContainsCorrectUserIdClaim()
    {
        // Arrange
        var tokenService = new TokenService(BuildConfig());
        const int userId = 42;

        // Act
        var (accessToken, _) = tokenService.GenerateToken(userId, "user");

        // Assert — decode the payload (second part) and verify sub = userId
        string[] parts = accessToken.Split('.');
        string payload = parts[1];
        // Pad base64url to standard base64
        int padding = payload.Length % 4;
        if (padding != 0) payload += new string('=', 4 - padding);
        string json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(
            payload.Replace('-', '+').Replace('_', '/')));
        using var doc = JsonDocument.Parse(json);
        string sub = doc.RootElement.GetProperty("sub").GetString()!;

        Assert.Equal(userId.ToString(), sub);
    }

    [Fact]
    public void GenerateToken_ContainsUsernameClaim()
    {
        // Arrange
        var tokenService = new TokenService(BuildConfig());
        const string username = "kanjika_user";

        // Act
        var (accessToken, _) = tokenService.GenerateToken(1, username);

        // Assert — decode the payload and verify unique_name = username
        string[] parts = accessToken.Split('.');
        string payload = parts[1];
        int padding = payload.Length % 4;
        if (padding != 0) payload += new string('=', 4 - padding);
        string json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(
            payload.Replace('-', '+').Replace('_', '/')));
        using var doc = JsonDocument.Parse(json);
        string uniqueName = doc.RootElement.GetProperty("unique_name").GetString()!;

        Assert.Equal(username, uniqueName);
    }

    [Fact]
    public void GenerateToken_AccessAndRefreshTokenAreDifferent()
    {
        // Arrange
        var tokenService = new TokenService(BuildConfig());

        // Act
        var (accessToken, refreshToken) = tokenService.GenerateToken(1, "user");

        // Assert
        Assert.NotEqual(accessToken, refreshToken);
    }

    [Fact]
    public void GenerateToken_RefreshTokenIsBase64()
    {
        // Arrange
        var tokenService = new TokenService(BuildConfig());

        // Act
        var (_, refreshToken) = tokenService.GenerateToken(1, "user");

        // Assert — Convert.FromBase64String throws if not valid base64
        byte[] decoded = Convert.FromBase64String(refreshToken);
        Assert.Equal(64, decoded.Length);
    }
}
