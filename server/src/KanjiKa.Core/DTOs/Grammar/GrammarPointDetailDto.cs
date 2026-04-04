namespace KanjiKa.Core.DTOs.Grammar;

public class GrammarPointDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public int JlptLevel { get; set; }
    public int CorrectCount { get; set; }
    public int AttemptCount { get; set; }
    public bool IsCompleted { get; set; }
    public List<GrammarExampleDto> Examples { get; set; } = [];
    public List<GrammarExerciseDto> Exercises { get; set; } = [];
}
