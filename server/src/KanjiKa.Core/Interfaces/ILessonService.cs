using KanjiKa.Core.Dtos;
using KanjiKa.Core.Dtos.Learning;
using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.Interfaces;

public interface ILessonService
{
    Task<TodayLessonCountDto> GetTodayLessonCountAsync(int userId);
    Task<IEnumerable<LessonDto>> GetNewLessonsAsync(int userId, int pageIndex, int pageSize);
    Task<Proficiency> LearnLessonAsync(int userId, int characterId);
    Task<IEnumerable<LessonReviewDto>> GetLessonReviews(int userId);
    Task<bool> CheckReviewItemAnswerAsync(int userId, string character, LessonReviewAnswerDto answer);
}
