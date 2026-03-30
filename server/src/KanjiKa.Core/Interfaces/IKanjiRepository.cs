using KanjiKa.Core.Entities.Kanji;

namespace KanjiKa.Core.Interfaces;

public interface IKanjiRepository
{
    Task<List<Kanji>> GetByJlptLevelAsync(int jlptLevel);
    Task<Kanji?> GetByCharacterAsync(string character);
}
