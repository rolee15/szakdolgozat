using KanjiKa.Core.Entities;
using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.Dtos;

public class KanaCharacterDto
{
    public string Character { get; set; }
    public string Romanization { get; set; }
    public KanaType Type { get; set; }
    public int Proficiency { get; set; }
}
