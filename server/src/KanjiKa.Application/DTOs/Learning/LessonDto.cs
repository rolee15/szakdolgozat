using KanjiKa.Domain.Entities.Kana;

namespace KanjiKa.Application.DTOs.Learning;

public class LessonDto
{
    public int CharacterId { get; set; }
    public required string Symbol { get; set; }
    public required string Romanization { get; set; }
    public KanaType Type { get; set; }
    public List<Example>? Examples { get; set; }
}
