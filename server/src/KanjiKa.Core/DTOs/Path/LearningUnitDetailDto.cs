namespace KanjiKa.Core.DTOs.Path;

public class LearningUnitDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public int ContentCount { get; set; }
    public bool IsPassed { get; set; }
    public int BestScore { get; set; }
    public bool IsUnlocked { get; set; }
    public List<UnitContentSummaryDto> Contents { get; set; } = [];
    public int TestQuestionCount { get; set; }
}
