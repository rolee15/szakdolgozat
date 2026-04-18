using System.ComponentModel.DataAnnotations;

namespace KanjiKa.Application.DTOs.User;

public class KanjiKaLoginRequest
{
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}
