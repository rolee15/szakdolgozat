namespace KanjiKa.Application.DTOs.Grammar;

public class GrammarExerciseResultDto
{
    public bool IsCorrect { get; set; }
    public string CorrectAnswer { get; set; } = string.Empty;
    public int CorrectCount { get; set; }
    public int AttemptCount { get; set; }
    public bool IsCompleted { get; set; }
}
