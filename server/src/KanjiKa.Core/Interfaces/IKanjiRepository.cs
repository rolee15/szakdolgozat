using KanjiKa.Core.Entities.Kanji;

namespace KanjiKa.Core.Interfaces;

public interface IKanjiRepository
{
    Task<List<Kanji>> GetByJlptLevelAsync(int jlptLevel);
    Task<Kanji?> GetByCharacterAsync(string character);
    Task<(List<Kanji> Items, int TotalCount)> GetPagedAsync(int? jlptLevel, int page, int pageSize);
    Task<Dictionary<int, KanjiProficiency>> GetProficienciesForUserAsync(int userId, List<int> kanjiIds);
}
