using KanjiKa.Domain.Entities.Kana;

namespace KanjiKa.Application.DTOs.Kana;

public class KanaCharacterDto
{
    public required string Character { get; set; }
    public required string Romanization { get; set; }
    public KanaType Type { get; set; }
    public int Proficiency { get; set; }
}
