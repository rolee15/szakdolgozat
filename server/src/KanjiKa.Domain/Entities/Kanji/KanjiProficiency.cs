using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Domain.Entities.Kanji;

public class KanjiProficiency
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int KanjiId { get; set; }
    public Kanji? Kanji { get; set; }
    public SrsStage SrsStage { get; set; } = SrsStage.Apprentice1;
    public DateTimeOffset? NextReviewDate { get; set; }
    public DateTimeOffset LearnedAt { get; init; } = DateTimeOffset.UtcNow;

    public void AnswerCorrectly()
    {
        SrsStage = SrsIntervals.Advance(SrsStage);
        NextReviewDate = SrsIntervals.GetNextReviewDate(SrsStage);
    }

    public void AnswerIncorrectly()
    {
        SrsStage = SrsIntervals.Regress(SrsStage);
        NextReviewDate = SrsIntervals.GetNextReviewDate(SrsStage);
    }
}
