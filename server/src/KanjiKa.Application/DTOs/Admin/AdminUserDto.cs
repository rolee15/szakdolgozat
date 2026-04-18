using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.DTOs.Admin;

public class AdminUserDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public UserRole Role { get; set; }
    public bool MustChangePassword { get; set; }
    public int ProficiencyCount { get; set; }
    public int LessonCompletionCount { get; set; }
}
