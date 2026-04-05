namespace KanjiKa.Application.DTOs.Path;

public class UnitTestQuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public Dictionary<string, string> Options { get; set; } = [];
}
