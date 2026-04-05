namespace KanjiKa.Core.Entities.Path;

public class UnitTest
{
    public int Id { get; set; }
    public int LearningUnitId { get; set; }
    public LearningUnit? LearningUnit { get; set; }
    public required string QuestionText { get; set; }
    public required string OptionA { get; set; }
    public required string OptionB { get; set; }
    public required string OptionC { get; set; }
    public required string OptionD { get; set; }
    public char CorrectOption { get; set; }
    public ContentType ContentType { get; set; }
    public int SortOrder { get; set; }
}
