using KanjiKa.Application.DTOs;
using KanjiKa.Application.DTOs.Kanji;
using KanjiKa.Domain.Entities.Kanji;

namespace KanjiKa.Application.Interfaces;

public interface IKanjiService
{
    Task<List<KanjiDto>> GetKanjiByLevelAsync(int jlptLevel, int userId);
    Task<KanjiDetailDto?> GetKanjiDetailAsync(string character, int userId);
    Task<PagedResult<KanjiDto>> GetKanjiPagedAsync(int? jlptLevel, int page, int pageSize, int userId);
    Task<int> GetDueReviewsCountAsync(int userId);
    Task<List<KanjiReviewDto>> GetDueReviewsAsync(int userId);
    Task<KanjiReviewResultDto> CheckReviewAsync(int userId, KanjiReviewAnswerDto answer);
    Task<KanjiProficiency> LearnKanjiAsync(int userId, int kanjiId);
}
