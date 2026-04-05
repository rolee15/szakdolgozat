using KanjiKa.Application.DTOs.Reading;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Reading;

namespace KanjiKa.Application.Services;

public class ReadingService : IReadingService
{
    private const int PassThreshold = 70;

    private readonly IReadingRepository _readingRepository;

    public ReadingService(IReadingRepository readingRepository)
    {
        _readingRepository = readingRepository;
    }

    public async Task<List<ReadingPassageDto>> GetPassagesAsync(int userId)
    {
        var passages = await _readingRepository.GetAllAsync();
        var ids = passages.Select(p => p.Id).ToList();
        var proficiencies = await _readingRepository.GetProficienciesForUserAsync(userId, ids);

        return passages.Select(p =>
        {
            proficiencies.TryGetValue(p.Id, out var prof);
            return new ReadingPassageDto
            {
                Id = p.Id,
                Title = p.Title,
                JlptLevel = p.JlptLevel,
                IsPassed = prof?.IsPassed ?? false,
                Score = prof?.Score ?? 0,
                AttemptCount = prof?.AttemptCount ?? 0
            };
        }).ToList();
    }

    public async Task<ReadingPassageDetailDto?> GetPassageDetailAsync(int id, int userId)
    {
        var passage = await _readingRepository.GetByIdAsync(id);
        if (passage == null) return null;

        var proficiencies = await _readingRepository.GetProficienciesForUserAsync(userId, [passage.Id]);
        proficiencies.TryGetValue(passage.Id, out var prof);

        return new ReadingPassageDetailDto
        {
            Id = passage.Id,
            Title = passage.Title,
            Content = passage.Content,
            Source = passage.Source,
            JlptLevel = passage.JlptLevel,
            IsPassed = prof?.IsPassed ?? false,
            Score = prof?.Score ?? 0,
            AttemptCount = prof?.AttemptCount ?? 0,
            Questions = passage.Questions.Select(q => new ComprehensionQuestionDto
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                Options = new[] { ("A", q.OptionA), ("B", q.OptionB), ("C", q.OptionC), ("D", q.OptionD) }
                    .OrderBy(_ => Guid.NewGuid())
                    .ToDictionary(pair => pair.Item1, pair => pair.Item2)
            }).ToList()
        };
    }

    public async Task<ReadingResultDto> SubmitAnswersAsync(int userId, ReadingSubmitDto submitDto)
    {
        var passage = await _readingRepository.GetByIdAsync(submitDto.PassageId);
        if (passage == null)
            throw new KeyNotFoundException($"Passage {submitDto.PassageId} not found.");

        var results = passage.Questions.Select(q =>
        {
            submitDto.Answers.TryGetValue(q.Id, out var chosen);
            var isCorrect = string.Equals(chosen, q.CorrectOption.ToString(), StringComparison.OrdinalIgnoreCase);
            return new QuestionResultDto
            {
                QuestionId = q.Id,
                IsCorrect = isCorrect,
                CorrectOption = q.CorrectOption.ToString(),
                ChosenOption = chosen ?? string.Empty
            };
        }).ToList();

        var correctCount = results.Count(r => r.IsCorrect);
        var totalQuestions = passage.Questions.Count;
        var score = totalQuestions > 0 ? correctCount * 100 / totalQuestions : 0;
        var isPassed = score >= PassThreshold;

        var proficiencies = await _readingRepository.GetProficienciesForUserAsync(userId, [passage.Id]);
        proficiencies.TryGetValue(passage.Id, out var existing);

        var proficiency = new ReadingProficiency
        {
            UserId = userId,
            ReadingPassageId = passage.Id,
            Score = score,
            AttemptCount = (existing?.AttemptCount ?? 0) + 1,
            IsPassed = isPassed,
            LastAttemptAt = DateTimeOffset.UtcNow
        };

        await _readingRepository.UpsertProficiencyAsync(proficiency);
        await _readingRepository.SaveChangesAsync();

        return new ReadingResultDto
        {
            Score = score,
            IsPassed = isPassed,
            CorrectCount = correctCount,
            TotalQuestions = totalQuestions,
            Results = results
        };
    }
}
