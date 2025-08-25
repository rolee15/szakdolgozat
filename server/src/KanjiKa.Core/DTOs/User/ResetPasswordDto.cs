using System.Diagnostics.CodeAnalysis;

namespace KanjiKa.Core.DTOs.User;

public class ResetPasswordDto
{
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public bool IsSuccess { get; set; }

    public string? ErrorMessage { get; set; }
}
