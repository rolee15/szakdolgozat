using KanjiKa.Domain.Entities.Common;

namespace KanjiKa.Domain.Entities.Reading;

public class ReadingProficiency : Proficiency<ReadingPassage>
{
    public int ReadingPassageId { get; set; }
    public ReadingPassage? ReadingPassage { get; set; }
    public int Score { get; set; }
    public int AttemptCount { get; set; }
    public bool IsPassed { get; set; }
}
