using KanjiKa.Core.DTOs;
using KanjiKa.Core.DTOs.Kanji;

namespace KanjiKa.Core.Interfaces;

public interface IKanjiService
{
    Task<List<KanjiDto>> GetKanjiByLevelAsync(int jlptLevel, int userId);
    Task<KanjiDetailDto?> GetKanjiDetailAsync(string character, int userId);
    Task<PagedResult<KanjiDto>> GetKanjiPagedAsync(int? jlptLevel, int page, int pageSize, int userId);
}
