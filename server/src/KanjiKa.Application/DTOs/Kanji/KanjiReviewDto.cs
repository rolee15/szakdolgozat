namespace KanjiKa.Application.DTOs.Kanji;

public class KanjiReviewDto
{
    public int KanjiId { get; set; }
    public required string Character { get; set; }
    public required string Meaning { get; set; }
    public string? OnyomiReading { get; set; }
    public string? KunyomiReading { get; set; }
}
