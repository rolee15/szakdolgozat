namespace KanjiKa.Core.DTOs.Reading;

public class ComprehensionQuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public Dictionary<string, string> Options { get; set; } = [];
}
