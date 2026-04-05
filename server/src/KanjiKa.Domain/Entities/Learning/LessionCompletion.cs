using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Domain.Entities.Learning;

public class LessonCompletion
{
    public DateTimeOffset CompletionDate { get; set; }

    public int UserId { get; init; }
    public User User { get; init; }
    public int CharacterId { get; init; }
    public Character Character { get; init; }
}
