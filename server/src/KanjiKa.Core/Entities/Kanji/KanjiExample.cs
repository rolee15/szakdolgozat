namespace KanjiKa.Core.Entities.Kanji;

public class KanjiExample
{
    public int Id { get; set; }
    public required string Word { get; set; }
    public required string Reading { get; set; }
    public required string Meaning { get; set; }
    public int KanjiId { get; set; }
    public Kanji? Kanji { get; set; }
}
