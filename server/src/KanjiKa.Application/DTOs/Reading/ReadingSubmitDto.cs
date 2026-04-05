namespace KanjiKa.Application.DTOs.Reading;

public class ReadingSubmitDto
{
    public int PassageId { get; set; }
    public Dictionary<int, string> Answers { get; set; } = [];
}
