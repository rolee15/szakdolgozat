using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Users;

namespace KanjiKa.Core.Entities.Learning;

public class LessonCompletion
{
    public DateTimeOffset CompletionDate { get; set; }

    public int UserId { get; init; }
    public User User { get; init; }
    public int CharacterId { get; init; }
    public Character Character { get; init; }
}
