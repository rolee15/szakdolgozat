namespace KanjiKa.Application.DTOs.User;

public class RegisterDto
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;

    public RegisterDto(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
