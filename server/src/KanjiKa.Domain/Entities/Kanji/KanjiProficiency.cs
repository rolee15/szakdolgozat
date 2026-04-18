using KanjiKa.Domain.Entities.Common;

namespace KanjiKa.Domain.Entities.Kanji;

public class KanjiProficiency : Proficiency<Kanji>
{
    public int KanjiId { get; set; }
    public Kanji? Kanji { get; set; }
}
