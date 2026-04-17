namespace KanjiKa.Application.DTOs.User;

public class ActivateDto
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;

    public ActivateDto(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
