namespace KanjiKa.Core.DTOs.Reading;

public class ReadingResultDto
{
    public int Score { get; set; }
    public bool IsPassed { get; set; }
    public int CorrectCount { get; set; }
    public int TotalQuestions { get; set; }
    public List<QuestionResultDto> Results { get; set; } = [];
}
