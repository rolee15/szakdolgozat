using KanjiKa.Domain.Entities.Common;

namespace KanjiKa.Domain.Entities.Kana;

public class KanaProficiency : Proficiency<Character>
{
    public int CharacterId { get; set; }
    public Character? Character { get; set; }
}
