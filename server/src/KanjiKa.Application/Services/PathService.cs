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
        List<LearningUnit> units = await _pathRepository.GetAllUnitsAsync();
        List<int> ids = units.Select(u => u.Id).ToList();
        Dictionary<int, UnitProgress> progress = await _pathRepository.GetProgressForUserAsync(userId, ids);

        var result = new List<LearningUnitDto>();
        for (var i = 0; i < units.Count; i++)
        {
            LearningUnit unit = units[i];
            progress.TryGetValue(unit.Id, out UnitProgress? prog);

            bool isUnlocked = i == 0;
            if (i > 0)
            {
                LearningUnit prevUnit = units[i - 1];
                progress.TryGetValue(prevUnit.Id, out UnitProgress? prevProg);
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
        LearningUnit? unit = await _pathRepository.GetUnitByIdAsync(unitId);
        if (unit == null) return null;

        LearningUnit? unitWithTest = await _pathRepository.GetUnitWithTestAsync(unitId);
        Dictionary<int, UnitProgress> progress = await _pathRepository.GetProgressForUserAsync(userId, [unitId]);
        progress.TryGetValue(unitId, out UnitProgress? prog);

        List<LearningUnit> allUnits = await _pathRepository.GetAllUnitsAsync();
        List<LearningUnit> sortedUnits = allUnits.OrderBy(u => u.SortOrder).ToList();
        int index = sortedUnits.FindIndex(u => u.Id == unitId);

        bool isUnlocked = index == 0;
        if (index > 0)
        {
            LearningUnit prevUnit = sortedUnits[index - 1];
            Dictionary<int, UnitProgress> prevProgress = await _pathRepository.GetProgressForUserAsync(userId, [prevUnit.Id]);
            prevProgress.TryGetValue(prevUnit.Id, out UnitProgress? prevProg);
            isUnlocked = prevProg?.IsPassed ?? false;
        }

        var contentSummaries = new List<UnitContentSummaryDto>();
        foreach (UnitContent content in unit.Contents)
        {
            string? title = await _pathRepository.GetContentTitleAsync(content.ContentType, content.ContentId);
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
        LearningUnit? unit = await _pathRepository.GetUnitWithTestAsync(unitId);
        if (unit == null) return null;

        return new UnitTestDto
        {
            Questions = unit.Tests.Select(t => new UnitTestQuestionDto
            {
                Id = t.Id,
                QuestionText = t.QuestionText,
                Options = new[] { ("A", t.OptionA), ("B", t.OptionB), ("C", t.OptionC), ("D", t.OptionD) }
                    .OrderBy(_ => Guid.NewGuid())
                    .ToDictionary(keySelector: pair => pair.Item1, elementSelector: pair => pair.Item2)
            }).ToList()
        };
    }

    public async Task<UnitTestResultDto> SubmitTestAsync(int userId, int unitId, UnitSubmitDto submitDto)
    {
        LearningUnit? unit = await _pathRepository.GetUnitWithTestAsync(unitId);
        if (unit == null)
            throw new KeyNotFoundException($"Learning unit {unitId} not found.");

        int correctCount = unit.Tests.Count(t => {
            submitDto.Answers.TryGetValue(t.Id, out string? chosen);
            return string.Equals(chosen, t.CorrectOption.ToString(), StringComparison.OrdinalIgnoreCase);
        });

        int totalQuestions = unit.Tests.Count;
        int score = totalQuestions > 0 ? correctCount * 100 / totalQuestions : 0;
        bool isPassed = score >= PassThreshold;

        Dictionary<int, UnitProgress> progressDict = await _pathRepository.GetProgressForUserAsync(userId, [unitId]);
        progressDict.TryGetValue(unitId, out UnitProgress? existing);

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
