using System.Diagnostics.CodeAnalysis;

namespace KanjiKa.Core.DTOs.User;

public class RegisterDto
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    [MemberNotNullWhen(true, nameof(Token))]
    [MemberNotNullWhen(true, nameof(RefreshToken))]
    public bool IsSuccess { get; set; }

    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}
