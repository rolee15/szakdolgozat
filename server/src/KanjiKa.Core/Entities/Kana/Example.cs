namespace KanjiKa.Core.Entities.Kana;

public class Example
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public Character Character { get; set; }
    public required string Word { get; set; }
    public required string Romanization { get; set; }
    public required string Meaning { get; set; }
}
