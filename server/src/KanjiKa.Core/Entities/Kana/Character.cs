namespace KanjiKa.Core.Entities;

public class KanaCharacter
{
    public string Id { get; set; }
    public string Symbol { get; set; }
    public string Romanization { get; set; }
    public KanaType Type { get; set; }
    public List<Example> Examples { get; set; } = new();
    public List<CharacterProficiency> Proficiencies { get; set; } = new();
}

public enum KanaType
{
    Hiragana,
    Katakana
}
