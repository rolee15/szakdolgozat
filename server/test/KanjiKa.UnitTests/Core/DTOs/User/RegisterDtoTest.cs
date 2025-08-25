using KanjiKa.Core.DTOs.User;

namespace KanjiKa.UnitTests.Core.DTOs.User;

public class RegisterDtoTest
{
    [Fact]
    public void RegisterDto_Constructor_ShouldInitializeProperties()
    {
        var registerDto = new RegisterDto
        {
            IsSuccess = true,
            ErrorMessage = "No error",
            Token = "test_token",
            RefreshToken = "test_refresh_token"
        };

        Assert.Multiple(
            () => Assert.True(registerDto.IsSuccess),
            () => Assert.Equal("No error", registerDto.ErrorMessage),
            () => Assert.Equal("test_token", registerDto.Token),
            () => Assert.Equal("test_refresh_token", registerDto.RefreshToken)
        );
    }
}
