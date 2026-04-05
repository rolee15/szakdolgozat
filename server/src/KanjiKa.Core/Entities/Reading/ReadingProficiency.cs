using KanjiKa.Core.Entities.Users;

namespace KanjiKa.Core.Entities.Reading;

public class ReadingProficiency
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int ReadingPassageId { get; set; }
    public ReadingPassage? ReadingPassage { get; set; }
    public int Score { get; set; }
    public int AttemptCount { get; set; }
    public bool IsPassed { get; set; }
    public DateTimeOffset LastAttemptAt { get; set; } = DateTimeOffset.UtcNow;
}
