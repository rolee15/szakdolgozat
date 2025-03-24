using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.DTOs.Kana;

public class KanaCharacterDetailDto
{
    public string Character { get; set; }
    public string Romanization { get; set; }
    public KanaType Type { get; set; }
    public int Proficiency { get; set; }
}
