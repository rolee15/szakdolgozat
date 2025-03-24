namespace KanjiKa.Core.DTOs.User;

public class RegisterDto
{
    public bool isSuccess { get; set; }
    public string errorMessage { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}