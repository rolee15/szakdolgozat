using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Domain.Entities.Common;

public abstract class Proficiency<TContent> where TContent : class
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public TContent? Content { get; set; }

    public SrsStage SrsStage { get; set; } = SrsStage.Apprentice1;
    public DateTimeOffset? NextReviewDate { get; set; }
    public DateTimeOffset LearnedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastPracticedAt { get; set; } = DateTimeOffset.UtcNow;

    public int Level => SrsStage == SrsStage.Burned ? 100 : (int)SrsStage * 100 / 9;

    public void AnswerCorrectly(DateTimeOffset? now = null)
    {
        SrsStage = SrsIntervals.Advance(SrsStage);
        NextReviewDate = SrsIntervals.GetNextReviewDate(SrsStage);
        LastPracticedAt = now ?? DateTimeOffset.UtcNow;
    }

    public void AnswerIncorrectly(DateTimeOffset? now = null)
    {
        SrsStage = SrsIntervals.Regress(SrsStage);
        NextReviewDate = SrsIntervals.GetNextReviewDate(SrsStage);
        LastPracticedAt = now ?? DateTimeOffset.UtcNow;
    }
}
