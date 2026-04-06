using KanjiKa.Application.DTOs.Grammar;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Grammar;

namespace KanjiKa.Application.Services;

public class GrammarService : IGrammarService
{
    private const int CompletionThreshold = 3;

    private readonly IGrammarRepository _grammarRepository;

    public GrammarService(IGrammarRepository grammarRepository)
    {
        _grammarRepository = grammarRepository;
    }

    public async Task<List<GrammarPointDto>> GetGrammarPointsAsync(int userId)
    {
        List<GrammarPoint> points = await _grammarRepository.GetAllAsync();
        List<int> ids = points.Select(p => p.Id).ToList();
        Dictionary<int, GrammarProficiency> proficiencies = await _grammarRepository.GetProficienciesForUserAsync(userId, ids);

        return points.Select(p => {
            proficiencies.TryGetValue(p.Id, out GrammarProficiency? prof);
            return new GrammarPointDto
            {
                Id = p.Id,
                Title = p.Title,
                Pattern = p.Pattern,
                JlptLevel = p.JlptLevel,
                CorrectCount = prof?.CorrectCount ?? 0,
                AttemptCount = prof?.AttemptCount ?? 0,
                IsCompleted = (prof?.CorrectCount ?? 0) >= CompletionThreshold
            };
        }).ToList();
    }

    public async Task<GrammarPointDetailDto?> GetGrammarPointDetailAsync(int grammarPointId, int userId)
    {
        GrammarPoint? point = await _grammarRepository.GetByIdAsync(grammarPointId);
        if (point == null) return null;

        Dictionary<int, GrammarProficiency> proficiencies = await _grammarRepository.GetProficienciesForUserAsync(userId, [point.Id]);
        proficiencies.TryGetValue(point.Id, out GrammarProficiency? prof);

        return new GrammarPointDetailDto
        {
            Id = point.Id,
            Title = point.Title,
            Pattern = point.Pattern,
            Explanation = point.Explanation,
            JlptLevel = point.JlptLevel,
            CorrectCount = prof?.CorrectCount ?? 0,
            AttemptCount = prof?.AttemptCount ?? 0,
            IsCompleted = (prof?.CorrectCount ?? 0) >= CompletionThreshold,
            Examples = point.Examples.Select(e => new GrammarExampleDto
            {
                Japanese = e.Japanese,
                Reading = e.Reading,
                English = e.English
            }).ToList(),
            Exercises = point.Exercises.Select(ex => new GrammarExerciseDto
            {
                Id = ex.Id,
                Sentence = ex.Sentence,
                Options = new[] { ex.CorrectAnswer, ex.Distractor1, ex.Distractor2, ex.Distractor3 }
                    .OrderBy(_ => Guid.NewGuid())
                    .ToList()
            }).ToList()
        };
    }

    public async Task<GrammarExerciseResultDto> CheckExerciseAsync(int userId, int grammarPointId, GrammarExerciseAnswerDto answer)
    {
        GrammarExercise? exercise = await _grammarRepository.GetExerciseByIdAsync(answer.ExerciseId);
        if (exercise == null)
            throw new KeyNotFoundException($"Exercise {answer.ExerciseId} not found.");

        if (exercise.GrammarPointId != grammarPointId)
            throw new ArgumentException($"Exercise {answer.ExerciseId} does not belong to grammar point {grammarPointId}.");

        bool isCorrect = string.Equals(
            answer.Answer.Trim(),
            exercise.CorrectAnswer.Trim(),
            StringComparison.OrdinalIgnoreCase);

        GrammarProficiency? proficiency = await _grammarRepository.GetProficiencyAsync(userId, grammarPointId);
        if (proficiency == null)
        {
            proficiency = new GrammarProficiency
            {
                UserId = userId,
                GrammarPointId = grammarPointId,
                CorrectCount = isCorrect ? 1 : 0,
                AttemptCount = 1,
                LastPracticedAt = DateTimeOffset.UtcNow
            };
            await _grammarRepository.AddProficiencyAsync(proficiency);
        }
        else
        {
            proficiency.AttemptCount++;
            if (isCorrect) proficiency.CorrectCount++;
            proficiency.LastPracticedAt = DateTimeOffset.UtcNow;
        }

        await _grammarRepository.SaveChangesAsync();

        return new GrammarExerciseResultDto
        {
            IsCorrect = isCorrect,
            CorrectAnswer = exercise.CorrectAnswer,
            CorrectCount = proficiency.CorrectCount,
            AttemptCount = proficiency.AttemptCount,
            IsCompleted = proficiency.CorrectCount >= CompletionThreshold
        };
    }
}
