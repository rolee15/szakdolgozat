using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.DTOs.Kana;

public class KanaCharacterDetailDto
{
    public required string Character { get; set; }
    public required string Romanization { get; set; }
    public KanaType Type { get; set; }
    public int Proficiency { get; set; }
}
