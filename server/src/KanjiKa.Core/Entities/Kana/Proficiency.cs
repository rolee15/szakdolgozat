using System.ComponentModel.DataAnnotations.Schema;
using KanjiKa.Core.Entities.Users;

namespace KanjiKa.Core.Entities.Kana;

public class Proficiency
{
    public const int MaxLevel = 100;
    public const int MinLevel = 0;

    public int Id { get; init; }
    public int UserId { get; init; }
    public User User { get; init; }
    public int CharacterId { get; init; }
    public Character Character { get; init; }

    public int Level { get; set; }
    public DateTimeOffset LearnedAt { get; init; }
    public DateTimeOffset LastPracticed { get; set; }

    public void Increase(int amount)
    {
        Level = Level + amount > MaxLevel ? MaxLevel : Level + amount;
        LastPracticed = DateTimeOffset.UtcNow;
    }

    public void Decrease(int amount)
    {
        Level = Level - amount < MinLevel ? MinLevel : Level - amount;
        LastPracticed = DateTimeOffset.UtcNow;
    }
}
