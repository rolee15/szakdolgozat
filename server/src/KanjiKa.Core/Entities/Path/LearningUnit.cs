namespace KanjiKa.Core.Entities.Path;

public class LearningUnit
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int SortOrder { get; set; }
    public List<UnitContent> Contents { get; set; } = [];
    public List<UnitTest> Tests { get; set; } = [];
}
