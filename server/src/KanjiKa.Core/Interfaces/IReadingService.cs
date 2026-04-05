using KanjiKa.Core.DTOs.Reading;

namespace KanjiKa.Core.Interfaces;

public interface IReadingService
{
    Task<List<ReadingPassageDto>> GetPassagesAsync(int userId);
    Task<ReadingPassageDetailDto?> GetPassageDetailAsync(int id, int userId);
    Task<ReadingResultDto> SubmitAnswersAsync(int userId, ReadingSubmitDto submitDto);
}
