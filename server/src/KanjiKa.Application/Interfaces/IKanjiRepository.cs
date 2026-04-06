using KanjiKa.Domain.Entities.Kanji;

namespace KanjiKa.Application.Interfaces;

public interface IKanjiRepository
{
    Task<List<Kanji>> GetByJlptLevelAsync(int jlptLevel);

    Task<Kanji?> GetByCharacterAsync(string character);

    Task<(List<Kanji> Items, int TotalCount)> GetPagedAsync(int? jlptLevel, int page, int pageSize);

    Task<Dictionary<int, KanjiProficiency>> GetProficienciesForUserAsync(int userId, List<int> kanjiIds);

    Task<List<KanjiProficiency>> GetDueReviewsAsync(int userId);

    Task<KanjiProficiency?> GetProficiencyAsync(int userId, int kanjiId);

    Task AddProficiencyAsync(KanjiProficiency proficiency);

    Task SaveChangesAsync();
}
