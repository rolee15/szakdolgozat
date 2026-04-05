using KanjiKa.Application.DTOs.Path;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Path;

namespace KanjiKa.Application.Services;

public class PathService : IPathService
{
    private const int PassThreshold = 70;

    private readonly IPathRepository _pathRepository;

    public PathService(IPathRepository pathRepository)
    {
        _pathRepository = pathRepository;
    }

    public async Task<List<LearningUnitDto>> GetPathAsync(int userId)
    {
        var units = await _pathRepository.GetAllUnitsAsync();
        var ids = units.Select(u => u.Id).ToList();
        var progress = await _pathRepository.GetProgressForUserAsync(userId, ids);

        var result = new List<LearningUnitDto>();
        for (int i = 0; i < units.Count; i++)
        {
            var unit = units[i];
            progress.TryGetValue(unit.Id, out var prog);

            bool isUnlocked = i == 0;
            if (i > 0)
            {
                var prevUnit = units[i - 1];
                progress.TryGetValue(prevUnit.Id, out var prevProg);
                isUnlocked = prevProg?.IsPassed ?? false;
            }

            result.Add(new LearningUnitDto
            {
                Id = unit.Id,
                Title = unit.Title,
                Description = unit.Description,
                SortOrder = unit.SortOrder,
                ContentCount = unit.Contents.Count,
                IsPassed = prog?.IsPassed ?? false,
                BestScore = prog?.BestScore ?? 0,
                IsUnlocked = isUnlocked
            });
        }

        return result;
    }

    public async Task<LearningUnitDetailDto?> GetUnitDetailAsync(int unitId, int userId)
    {
        var unit = await _pathRepository.GetUnitByIdAsync(unitId);
        if (unit == null) return null;

        var unitWithTest = await _pathRepository.GetUnitWithTestAsync(unitId);
        var progress = await _pathRepository.GetProgressForUserAsync(userId, [unitId]);
        progress.TryGetValue(unitId, out var prog);

        var allUnits = await _pathRepository.GetAllUnitsAsync();
        var sortedUnits = allUnits.OrderBy(u => u.SortOrder).ToList();
        var index = sortedUnits.FindIndex(u => u.Id == unitId);

        bool isUnlocked = index == 0;
        if (index > 0)
        {
            var prevUnit = sortedUnits[index - 1];
            var prevProgress = await _pathRepository.GetProgressForUserAsync(userId, [prevUnit.Id]);
            prevProgress.TryGetValue(prevUnit.Id, out var prevProg);
            isUnlocked = prevProg?.IsPassed ?? false;
        }

        var contentSummaries = new List<UnitContentSummaryDto>();
        foreach (var content in unit.Contents)
        {
            var title = await _pathRepository.GetContentTitleAsync(content.ContentType, content.ContentId);
            contentSummaries.Add(new UnitContentSummaryDto
            {
                ContentType = content.ContentType.ToString(),
                ContentId = content.ContentId,
                Title = title ?? string.Empty
            });
        }

        return new LearningUnitDetailDto
        {
            Id = unit.Id,
            Title = unit.Title,
            Description = unit.Description,
            SortOrder = unit.SortOrder,
            ContentCount = unit.Contents.Count,
            IsPassed = prog?.IsPassed ?? false,
            BestScore = prog?.BestScore ?? 0,
            IsUnlocked = isUnlocked,
            Contents = contentSummaries,
            TestQuestionCount = unitWithTest?.Tests.Count ?? 0
        };
    }

    public async Task<UnitTestDto?> GetUnitTestAsync(int unitId, int userId)
    {
        var unit = await _pathRepository.GetUnitWithTestAsync(unitId);
        if (unit == null) return null;

        return new UnitTestDto
        {
            Questions = unit.Tests.Select(t => new UnitTestQuestionDto
            {
                Id = t.Id,
                QuestionText = t.QuestionText,
                Options = new[] { ("A", t.OptionA), ("B", t.OptionB), ("C", t.OptionC), ("D", t.OptionD) }
                    .OrderBy(_ => Guid.NewGuid())
                    .ToDictionary(pair => pair.Item1, pair => pair.Item2)
            }).ToList()
        };
    }

    public async Task<UnitTestResultDto> SubmitTestAsync(int userId, int unitId, UnitSubmitDto submitDto)
    {
        var unit = await _pathRepository.GetUnitWithTestAsync(unitId);
        if (unit == null)
            throw new KeyNotFoundException($"Learning unit {unitId} not found.");

        var correctCount = unit.Tests.Count(t =>
        {
            submitDto.Answers.TryGetValue(t.Id, out var chosen);
            return string.Equals(chosen, t.CorrectOption.ToString(), StringComparison.OrdinalIgnoreCase);
        });

        var totalQuestions = unit.Tests.Count;
        var score = totalQuestions > 0 ? correctCount * 100 / totalQuestions : 0;
        var isPassed = score >= PassThreshold;

        var progressDict = await _pathRepository.GetProgressForUserAsync(userId, [unitId]);
        progressDict.TryGetValue(unitId, out var existing);

        var progress = new UnitProgress
        {
            UserId = userId,
            LearningUnitId = unitId,
            IsPassed = isPassed,
            BestScore = score,
            AttemptCount = (existing?.AttemptCount ?? 0) + 1,
            LastAttemptAt = DateTimeOffset.UtcNow
        };

        await _pathRepository.UpsertProgressAsync(progress);
        await _pathRepository.SaveChangesAsync();

        return new UnitTestResultDto
        {
            Score = score,
            IsPassed = isPassed,
            CorrectCount = correctCount,
            TotalQuestions = totalQuestions
        };
    }
}
