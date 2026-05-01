namespace KanjiKa.Application.DTOs.Path;

public class UnitTestResultDto
{
    public int Score { get; set; }
    public bool IsPassed { get; set; }
    public int CorrectCount { get; set; }
    public int TotalQuestions { get; set; }
    public List<UnitTestWrongAnswerDto> WrongAnswers { get; set; } = [];
}

public class UnitTestWrongAnswerDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? UserAnswer { get; set; }
    public string CorrectAnswer { get; set; } = string.Empty;
}
