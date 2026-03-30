namespace KanjiKa.Core.DTOs.Learning;

public class LessonReviewAnswerResultDto
{
    public bool IsCorrect { get; set; }
    public required string CorrectAnswer { get; set; }
    public int SrsStage { get; set; }
    public string SrsStageName { get; set; } = string.Empty;
    public DateTimeOffset? NextReviewDate { get; set; }
}
