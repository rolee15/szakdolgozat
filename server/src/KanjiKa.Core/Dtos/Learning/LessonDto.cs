using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.Dtos.Learning;

public class LessonDto
{
    public int CharacterId { get; set; }
    public string Symbol { get; set; }
    public string Romanization { get; set; }
    public KanaType Type { get; set; }
    public List<Example> Examples { get; set; }
}
