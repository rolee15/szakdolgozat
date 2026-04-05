namespace KanjiKa.Application.DTOs.Path;

public class LearningUnitDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public int ContentCount { get; set; }
    public bool IsPassed { get; set; }
    public int BestScore { get; set; }
    public bool IsUnlocked { get; set; }
}
