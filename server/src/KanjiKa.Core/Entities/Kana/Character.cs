namespace KanjiKa.Core.Entities.Kana;

public class Character
{
    public int Id { get; set; }
    public required string Symbol { get; set; }
    public required string Romanization { get; set; }
    public KanaType Type { get; set; }
    public List<Example> Examples { get; set; } = new();
}

public enum KanaType
{
    Hiragana,
    Katakana
}
