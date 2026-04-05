namespace KanjiKa.Core.Entities.Reading;

public class ComprehensionQuestion
{
    public int Id { get; set; }
    public int ReadingPassageId { get; set; }
    public ReadingPassage? ReadingPassage { get; set; }
    public required string QuestionText { get; set; }
    public required string OptionA { get; set; }
    public required string OptionB { get; set; }
    public required string OptionC { get; set; }
    public required string OptionD { get; set; }
    public char CorrectOption { get; set; }
    public int SortOrder { get; set; }
}
