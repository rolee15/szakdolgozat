namespace KanjiKa.Application.DTOs.Learning;

public class WritingReviewDto
{
    public int CharacterId { get; set; }
    public required string Romanization { get; set; }
    public required string CharacterType { get; set; }
}
