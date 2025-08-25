using KanjiKa.Core.DTOs.User;

namespace KanjiKa.UnitTests.Core.DTOs.User;

public class LoginDtoTest
{
    [Fact]
    public void LoginDto_Constructor_ShouldInitializeProperties()
    {
        var login = new LoginDto
        {
            IsSuccess = true,
            ErrorMessage = string.Empty,
            Token = "test_token",
            RefreshToken = "test_refresh_token"
        };

        Assert.Multiple(
            () => Assert.True(login.IsSuccess),
            () => Assert.Equal(string.Empty, login.ErrorMessage),
            () => Assert.Equal("test_token", login.Token),
            () => Assert.Equal("test_refresh_token", login.RefreshToken)
        );
    }
}
