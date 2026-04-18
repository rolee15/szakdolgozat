using System.ComponentModel.DataAnnotations;

namespace KanjiKa.Application.DTOs.User;

public class KanjiKaRegisterRequest
{
    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required, MinLength(8)]
    public required string Password { get; set; }
}
