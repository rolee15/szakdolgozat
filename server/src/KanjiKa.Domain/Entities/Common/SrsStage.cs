namespace KanjiKa.Domain.Entities.Common;

public enum SrsStage
{
    Locked = 0,
    Apprentice1 = 1,
    Apprentice2 = 2,
    Apprentice3 = 3,
    Apprentice4 = 4,
    Guru1 = 5,
    Guru2 = 6,
    Master = 7,
    Enlightened = 8,
    Burned = 9
}

public static class SrsIntervals
{
    private static readonly Dictionary<SrsStage, TimeSpan> Intervals = new()
    {
        { SrsStage.Apprentice1, TimeSpan.FromHours(4) },
        { SrsStage.Apprentice2, TimeSpan.FromHours(8) },
        { SrsStage.Apprentice3, TimeSpan.FromDays(1) },
        { SrsStage.Apprentice4, TimeSpan.FromDays(2) },
        { SrsStage.Guru1, TimeSpan.FromDays(7) },
        { SrsStage.Guru2, TimeSpan.FromDays(14) },
        { SrsStage.Master, TimeSpan.FromDays(30) },
        { SrsStage.Enlightened, TimeSpan.FromDays(120) }
    };

    public static SrsStage Advance(SrsStage current)
    {
        return current < SrsStage.Burned ? current + 1 : SrsStage.Burned;
    }

    public static SrsStage Regress(SrsStage current)
    {
        int newStage = (int)current - 2;
        return newStage < (int)SrsStage.Apprentice1
            ? SrsStage.Apprentice1
            : (SrsStage)newStage;
    }

    public static DateTimeOffset? GetNextReviewDate(SrsStage stage)
    {
        if (stage == SrsStage.Locked || stage == SrsStage.Burned) return null;

        return Intervals.TryGetValue(stage, out TimeSpan interval)
            ? DateTimeOffset.UtcNow + interval
            : null;
    }

    public static string GetStageName(SrsStage stage)
    {
        return stage switch
        {
            SrsStage.Locked => "Locked",
            SrsStage.Apprentice1 => "Apprentice 1",
            SrsStage.Apprentice2 => "Apprentice 2",
            SrsStage.Apprentice3 => "Apprentice 3",
            SrsStage.Apprentice4 => "Apprentice 4",
            SrsStage.Guru1 => "Guru 1",
            SrsStage.Guru2 => "Guru 2",
            SrsStage.Master => "Master",
            SrsStage.Enlightened => "Enlightened",
            SrsStage.Burned => "Burned",
            _ => "Unknown"
        };
    }
}
