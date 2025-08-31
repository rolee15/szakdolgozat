namespace KanjiKa.Core.DTOs.Learning;

public class LessonReviewAnswerResultDto
{
    public bool IsCorrect { get; set; }
    public required string CorrectAnswer { get; set; }
}
