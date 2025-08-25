using KanjiKa.Core.Services;

namespace KanjiKa.UnitTests.Core.Services;

public class TokenServiceTest
{
    [Fact]
    public void TokenService_GenerateToken_ReturnsTokens()
    {
        // Arrange
        var tokenService = new TokenService();
        const int userId = 1;

        // Act
        (string token, string refreshToken) = tokenService.GenerateToken(userId);

        // Assert
        Assert.Multiple(
            () => Assert.Equal("test12345", token),
            () => Assert.Equal("refreshToken12345", refreshToken)
        );
    }
}
