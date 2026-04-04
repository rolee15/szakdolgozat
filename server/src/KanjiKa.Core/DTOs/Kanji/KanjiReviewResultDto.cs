namespace KanjiKa.Core.DTOs.Kanji;

public class KanjiReviewResultDto
{
    public bool IsCorrect { get; set; }
    public int SrsStage { get; set; }
    public required string SrsStageName { get; set; }
    public DateTimeOffset? NextReviewDate { get; set; }
}
