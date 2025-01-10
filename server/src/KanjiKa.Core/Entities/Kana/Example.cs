namespace KanjiKa.Core.Entities;

public class CharacterExample
{
    public string Id { get; set; }
    public string CharacterId { get; set; }
    public KanaCharacter Character { get; set; }
    public string Word { get; set; }
    public string Romanization { get; set; }
    public string Meaning { get; set; }
}
