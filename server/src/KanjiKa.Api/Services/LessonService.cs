using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Interfaces;

namespace KanjiKa.Api.Services;

internal class LessonService : ILessonService
{
    private const int SkillUp = 10;
    private const int SkillDown = 5;

    private readonly ILessonRepository _repo;

    public LessonService(ILessonRepository repo)
    {
        _repo = repo;
    }

    public async Task<LessonsCountDto> GetLessonsCountAsync(int userId)
    {
        var user = await _repo.GetUserWithProficienciesAsync(userId);

        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        var lessonsLearnedToday = await _repo.CountLessonsCompletedTodayAsync(userId, DateTimeOffset.UtcNow);
        var count = 15 - lessonsLearnedToday;

        return new LessonsCountDto
        {
            Count = Math.Max(count, 0)
        };
    }

    public async Task<IEnumerable<LessonDto>> GetLessonsAsync(int userId, int pageIndex, int pageSize)
    {
        var user = await _repo.GetUserWithProficienciesAsync(userId);

        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        var lessonsLearnedToday = await _repo.CountLessonsCompletedTodayAsync(userId, DateTimeOffset.UtcNow);
        var count = 15 - lessonsLearnedToday;

        if (count <= 0)
        {
            return Array.Empty<LessonDto>();
        }

        var allCharacters = await _repo.GetAllCharactersAsync();
        var newCharacters = allCharacters
            .Where(ch => user.Proficiencies.All(p => p.CharacterId != ch.Id))
            .ToList();

        var takeSize = Math.Min(count, pageSize);
        var lessons = newCharacters
            .Skip(pageIndex * pageSize)
            .Take(takeSize)
            .Select(MapToLessonDto);

        return lessons;
    }

    public async Task<Proficiency> LearnLessonAsync(int userId, int characterId)
    {
        var user = await _repo.GetUserWithProficienciesAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        var character = await _repo.GetCharacterByIdAsync(characterId);
        if (character == null)
        {
            throw new ArgumentException("Character not found", nameof(characterId));
        }

        var existingProficiency = await _repo.GetProficiencyAsync(userId, characterId);
        if (existingProficiency != null)
        {
            throw new ArgumentException("Character already learned", nameof(characterId));
        }

        var proficiency = new Proficiency
        {
            UserId = user.Id,
            CharacterId = character.Id,
            Level = 0,
            LearnedAt = DateTimeOffset.UtcNow
        };
        await _repo.AddProficiencyAsync(proficiency);

        var lessonCompletion = new LessonCompletion
        {
            UserId = user.Id,
            CharacterId = character.Id,
            CompletionDate = DateTimeOffset.UtcNow
        };
        await _repo.AddLessonCompletionAsync(lessonCompletion);

        await _repo.SaveChangesAsync();

        return proficiency;
    }

    public async Task<LessonReviewsCountDto> GetLessonReviewsCountAsync(int userId)
    {
        var completedLessons = await _repo.GetLessonCompletionsByUserAsync(userId);

        return new LessonReviewsCountDto
        {
            Count = completedLessons.Count
        };
    }

    public async Task<IEnumerable<LessonReviewDto>> GetLessonReviewsAsync(int userId)
    {
        var completedLessons = await _repo.GetLessonCompletionsByUserAsync(userId);
        var ordered = completedLessons
            .OrderBy(lc => lc.CompletionDate)
            .Select(lc => new LessonReviewDto { Question = lc.Character.Symbol })
            .ToList();

        return ordered;
    }

    public async Task<LessonReviewAnswerResultDto> CheckLessonReviewAnswerAsync(int userId, LessonReviewAnswerDto answer)
    {
        var character = await _repo.GetCharacterBySymbolAsync(answer.Question);
        if (character == null)
        {
            throw new ArgumentException("Character not found", nameof(answer));
        }

        var proficiency = await _repo.GetProficiencyAsync(userId, character.Id) ?? await LearnLessonAsync(userId, character.Id);

        if (answer.Answer == character.Romanization)
        {
            proficiency.Increase(SkillUp);
            await _repo.SaveChangesAsync();
            return new LessonReviewAnswerResultDto { IsCorrect = true, CorrectAnswer = character.Romanization };
        }

        proficiency.Decrease(SkillDown);
        await _repo.SaveChangesAsync();
        return new LessonReviewAnswerResultDto { IsCorrect = false, CorrectAnswer = character.Romanization };
    }

    private static LessonDto MapToLessonDto(Character character)
    {
        return new LessonDto
        {
            CharacterId = character.Id,
            Symbol = character.Symbol,
            Romanization = character.Romanization,
            Type = character.Type,
            //TODO: Solve the problem of circular reference for Examples
            // Examples = character.Examples
        };
    }
}
