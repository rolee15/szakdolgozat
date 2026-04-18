using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Domain.Entities.Grammar;

public class GrammarProficiency
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int GrammarPointId { get; set; }
    public GrammarPoint? GrammarPoint { get; set; }
    public int CorrectCount { get; set; }
    public int AttemptCount { get; set; }
    public DateTimeOffset LastPracticedAt { get; set; } = DateTimeOffset.UtcNow;
}
