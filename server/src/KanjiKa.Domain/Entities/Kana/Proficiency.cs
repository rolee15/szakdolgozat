using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Domain.Entities.Kana;

public class Proficiency
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public User? User { get; init; }
    public int CharacterId { get; init; }
    public Character? Character { get; init; }

    public SrsStage SrsStage { get; set; } = SrsStage.Apprentice1;
    public DateTimeOffset? NextReviewDate { get; set; }
    public DateTimeOffset LearnedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastPracticed { get; set; } = DateTimeOffset.UtcNow;

    // Computed for backward compatibility: maps SRS stage to 0-100 scale
    public int Level => SrsStage == SrsStage.Burned ? 100 : (int)SrsStage * 100 / 9;

    public void AnswerCorrectly(DateTimeOffset? now = null)
    {
        SrsStage = SrsIntervals.Advance(SrsStage);
        NextReviewDate = SrsIntervals.GetNextReviewDate(SrsStage);
        LastPracticed = now ?? DateTimeOffset.UtcNow;
    }

    public void AnswerIncorrectly(DateTimeOffset? now = null)
    {
        SrsStage = SrsIntervals.Regress(SrsStage);
        NextReviewDate = SrsIntervals.GetNextReviewDate(SrsStage);
        LastPracticed = now ?? DateTimeOffset.UtcNow;
    }
}
