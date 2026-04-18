using KanjiKa.Domain.Entities.Common;

namespace KanjiKa.Domain.Entities.Grammar;

public class GrammarProficiency : Proficiency<GrammarPoint>
{
    public int GrammarPointId { get; set; }
    public GrammarPoint? GrammarPoint { get; set; }
    public int CorrectCount { get; set; }
    public int AttemptCount { get; set; }
}
