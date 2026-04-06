using KanjiKa.Application.DTOs.Learning;
using KanjiKa.Domain.Entities.Kana;

namespace KanjiKa.Application.Interfaces;

public interface ILessonService
{
    Task<LessonsCountDto> GetLessonsCountAsync(int userId);

    Task<IEnumerable<LessonDto>> GetLessonsAsync(int userId, int pageIndex, int pageSize);

    Task<Proficiency> LearnLessonAsync(int userId, int characterId);

    Task<LessonReviewsCountDto> GetLessonReviewsCountAsync(int userId);

    Task<IEnumerable<LessonReviewDto>> GetLessonReviewsAsync(int userId);

    Task<LessonReviewAnswerResultDto> CheckLessonReviewAnswerAsync(int userId, LessonReviewAnswerDto answer);

    Task<LessonReviewsCountDto> GetWritingReviewsCountAsync(int userId);

    Task<IEnumerable<WritingReviewDto>> GetWritingReviewsAsync(int userId);

    Task<LessonReviewAnswerResultDto> CheckWritingReviewAnswerAsync(int userId, WritingReviewAnswerDto answer);
}
