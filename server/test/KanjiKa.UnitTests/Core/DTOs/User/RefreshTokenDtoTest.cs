using KanjiKa.Core.DTOs.User;

namespace KanjiKa.UnitTests.Core.DTOs.User;

public class RefreshTokenDtoTest
{
    [Fact]
    public void RefreshTokenDto_Constructor_ShouldInitializeProperties()
    {
        var refreshToken = new RefreshTokenDto
        {
            Token = "test_token",
        };

        Assert.Equal("test_token", refreshToken.Token);
    }
}
