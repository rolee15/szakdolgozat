using KanjiKa.Core.DTOs.User;

namespace KanjiKa.UnitTests.Core.DTOs.User;

public class ResetPasswordDtoTest
{
    [Fact]
    public void ResetPasswordDto_Constructor_ShouldInitializeProperties()
    {
        var resetPasswordDto = new ResetPasswordDto
        {
            IsSuccess = true,
            ErrorMessage = "No error"
        };

        Assert.True(resetPasswordDto.IsSuccess);
        Assert.Equal("No error", resetPasswordDto.ErrorMessage);
    }
}
