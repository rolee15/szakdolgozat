namespace KanjiKa.Core.Entities.Kana;

public class Proficiency
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int CharacterId { get; set; }
    public Character Character { get; set; }
    public int Level { get; set; }
    public DateTime LastPracticed { get; set; }
}
