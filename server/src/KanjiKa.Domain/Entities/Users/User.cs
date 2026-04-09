using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Learning;

namespace KanjiKa.Domain.Entities.Users;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }

    // storing the hashing algorithm and parameters would make it possible to phase out
    // the outdated passwords gradually, but it's not necessary for this project
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }

    public UserRole Role { get; set; } = UserRole.User;
    public bool MustChangePassword { get; set; }

    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiry { get; set; }

    public string? PasswordResetCode { get; set; }
    public DateTimeOffset? PasswordResetExpiry { get; set; }

    public List<Proficiency> Proficiencies { get; set; } = new();
    public List<LessonCompletion> LessonCompletions { get; set; } = new();
    public UserSettings? Settings { get; set; }
}
