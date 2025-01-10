using KanjiKa.Core.Entities;

namespace KanjiKa.Core.Interfaces;

internal class KanaCharacterDetailDto
{
    public string Character { get; set; }
    public string Romanization { get; set; }
    public KanaType Type { get; set; }
    public int Proficiency { get; set; }
}
