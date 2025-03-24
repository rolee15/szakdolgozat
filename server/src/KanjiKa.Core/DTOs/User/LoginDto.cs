namespace KanjiKa.Core.DTOs.User;

public class LoginDto
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}