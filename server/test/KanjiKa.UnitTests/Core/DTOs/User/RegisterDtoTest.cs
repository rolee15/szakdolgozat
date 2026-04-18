using KanjiKa.Application.DTOs.User;

namespace KanjiKa.UnitTests.Core.DTOs.User;

public class RegisterDtoTest
{
    [Fact]
    public void RegisterDto_Constructor_Success_ShouldInitializeProperties()
    {
        var registerDto = new RegisterDto(true, "Registration successful.");

        Assert.Multiple(
            () => Assert.True(registerDto.Success),
            () => Assert.Equal("Registration successful.", registerDto.Message)
        );
    }

    [Fact]
    public void RegisterDto_Constructor_Failure_ShouldInitializeProperties()
    {
        var registerDto = new RegisterDto(false, "Username already exists.");

        Assert.Multiple(
            () => Assert.False(registerDto.Success),
            () => Assert.Equal("Username already exists.", registerDto.Message)
        );
    }
}
