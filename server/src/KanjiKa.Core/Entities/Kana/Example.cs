namespace KanjiKa.Core.Entities.Kana;

public class Example
{
    public string Id { get; set; }
    public string CharacterId { get; set; }
    public Character Character { get; set; }
    public string Word { get; set; }
    public string Romanization { get; set; }
    public string Meaning { get; set; }
}
