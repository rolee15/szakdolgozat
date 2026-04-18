namespace KanjiKa.Application.DTOs.Path;

public class UnitTestResultDto
{
    public int Score { get; set; }
    public bool IsPassed { get; set; }
    public int CorrectCount { get; set; }
    public int TotalQuestions { get; set; }
}
