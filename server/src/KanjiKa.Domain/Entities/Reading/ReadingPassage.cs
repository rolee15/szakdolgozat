namespace KanjiKa.Domain.Entities.Reading;

public class ReadingPassage
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public int JlptLevel { get; set; }
    public required string Source { get; set; }
    public int SortOrder { get; set; }
    public List<ComprehensionQuestion> Questions { get; set; } = [];
}
