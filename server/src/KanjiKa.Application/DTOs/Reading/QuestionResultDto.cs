namespace KanjiKa.Application.DTOs.Reading;

public class QuestionResultDto
{
    public int QuestionId { get; set; }
    public bool IsCorrect { get; set; }
    public string CorrectOption { get; set; } = string.Empty;
    public string ChosenOption { get; set; } = string.Empty;
}
