using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public List<Proficiency> Proficiencies { get; set; } = new();
}
