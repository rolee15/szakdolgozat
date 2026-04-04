namespace KanjiKa.Core.DTOs.Grammar;

public class GrammarPointDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public int JlptLevel { get; set; }
    public int CorrectCount { get; set; }
    public int AttemptCount { get; set; }
    public bool IsCompleted { get; set; }
}
