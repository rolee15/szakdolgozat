using KanjiKa.Core.Entities.Users;

namespace KanjiKa.Core.Entities.Path;

public class UnitProgress
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int LearningUnitId { get; set; }
    public LearningUnit? LearningUnit { get; set; }
    public bool IsPassed { get; set; }
    public int BestScore { get; set; }
    public int AttemptCount { get; set; }
    public DateTimeOffset LastAttemptAt { get; set; } = DateTimeOffset.UtcNow;
}
