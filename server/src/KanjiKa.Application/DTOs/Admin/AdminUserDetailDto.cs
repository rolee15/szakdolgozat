using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.DTOs.Admin;

public class AdminUserDetailDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public UserRole Role { get; set; }
    public bool MustChangePassword { get; set; }
    public int ProficiencyCount { get; set; }
    public int LessonCompletionCount { get; set; }
    public List<ProficiencySummaryDto> Proficiencies { get; set; } = [];
    public List<LessonCompletionSummaryDto> LessonCompletions { get; set; } = [];
}

public class ProficiencySummaryDto
{
    public int CharacterId { get; set; }
    public required string CharacterSymbol { get; set; }
    public DateTimeOffset LearnedAt { get; set; }
    public DateTimeOffset LastPracticed { get; set; }
}

public class LessonCompletionSummaryDto
{
    public int CharacterId { get; set; }
    public required string CharacterSymbol { get; set; }
    public DateTimeOffset CompletionDate { get; set; }
}
