using KanjiKa.Core.DTOs;
using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.Interfaces;

public interface ILessonService
{
    Task<LessonsCountDto> GetLessonsCountAsync(int userId);
    Task<IEnumerable<LessonDto>> GetLessonsAsync(int userId, int pageIndex, int pageSize);
    Task<Proficiency> LearnLessonAsync(int userId, int characterId);
    Task<LessonReviewsCountDto> GetLessonReviewsCountAsync(int userId);
    Task<IEnumerable<LessonReviewDto>> GetLessonReviewsAsync(int userId);
    Task<LessonReviewAnswerResultDto> CheckLessonReviewAnswerAsync(int userId, LessonReviewAnswerDto answer);
}
