namespace KanjiKa.Core.DTOs.Reading;

public class ReadingPassageDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int JlptLevel { get; set; }
    public bool IsPassed { get; set; }
    public int Score { get; set; }
    public int AttemptCount { get; set; }
}
