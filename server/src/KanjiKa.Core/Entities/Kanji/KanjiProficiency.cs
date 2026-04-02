using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Users;

namespace KanjiKa.Core.Entities.Kanji;

public class KanjiProficiency
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int KanjiId { get; set; }
    public Kanji? Kanji { get; set; }
    public SrsStage SrsStage { get; set; } = SrsStage.Apprentice1;
}
