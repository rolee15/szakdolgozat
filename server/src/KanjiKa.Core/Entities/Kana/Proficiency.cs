namespace KanjiKa.Core.Entities;

public class CharacterProficiency
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string CharacterId { get; set; }
    public Character Character { get; set; }
    public int Level { get; set; }
    public DateTime LastPracticed { get; set; }
}
