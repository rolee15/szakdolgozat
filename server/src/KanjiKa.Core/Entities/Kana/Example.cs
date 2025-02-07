namespace KanjiKa.Core.Entities.Kana;

public class Example
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public Character Character { get; set; }
    public string Word { get; set; }
    public string Romanization { get; set; }
    public string Meaning { get; set; }
}
