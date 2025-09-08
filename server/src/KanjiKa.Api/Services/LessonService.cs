using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;

namespace KanjiKa.Api.Services;

internal class LessonService : ILessonService
{
    // Move to SRS algorithm settings
    private const int SkillUp = 10;
    private const int SkillDown = 5;

    // Move to user settings when implemented
    private const int LessonsPerDayCount = 15;

    private readonly ILessonRepository _repo;

    public LessonService(ILessonRepository repo)
    {
        _repo = repo;
    }

    // BUG: Lessons count should be dependent on the number of new characters still not learned
    public async Task<LessonsCountDto> GetLessonsCountAsync(int userId)
    {
        User? user = await _repo.GetUserAsync(userId);
        if (user == null) throw new ArgumentException("User not found", nameof(userId));

        int lessonsLearnedToday = await _repo.CountLessonsCompletedTodayAsync(userId);
        int count = LessonsPerDayCount - lessonsLearnedToday;

        return new LessonsCountDto
        {
            // Maybe throw exception to be consistent with other methods?
            Count = Math.Min(Math.Max(count, 0), 15)
        };
    }

    // Improvement: no need to load all characters, just new ones
    public async Task<IEnumerable<LessonDto>> GetLessonsAsync(int userId, int pageIndex, int pageSize)
    {
        if (pageIndex < 0)
            throw new ArgumentException("Page index cannot be less than zero", nameof(pageIndex));
        if (pageSize < 1)
            throw new ArgumentException("Page size cannot be less than one", nameof(pageSize));

        User? user = await _repo.GetUserWithProficienciesAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found", nameof(userId));

        int completedTodayCount = await _repo.CountLessonsCompletedTodayAsync(userId);
        int count = LessonsPerDayCount - completedTodayCount;

        if (count <= 0)
            return [];

        int takeSize = Math.Min(count, pageSize);
        IEnumerable<LessonDto> lessons = (await _repo.GetNewCharactersAsync(user.Proficiencies))
            .Skip(pageIndex * takeSize)
            .Take(takeSize)
            .Select(MapToLessonDto);

        return lessons;
    }

    public async Task<Proficiency> LearnLessonAsync(int userId, int characterId)
    {
        User? user = await _repo.GetUserAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found", nameof(userId));

        Character? character = await _repo.GetCharacterByIdAsync(characterId);
        if (character == null)
            throw new ArgumentException("Character not found", nameof(characterId));

        Proficiency? existingProficiency = await _repo.GetProficiencyAsync(userId, characterId);
        if (existingProficiency != null)
            throw new ArgumentException("Character already learned", nameof(characterId));

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
        List<LessonCompletion> completedLessons = await _repo.GetLessonCompletionsByUserAsync(userId);

        return new LessonReviewsCountDto
        {
            Count = completedLessons.Count
        };
    }

    public async Task<IEnumerable<LessonReviewDto>> GetLessonReviewsAsync(int userId)
    {
        List<LessonCompletion> completedLessons = await _repo.GetLessonCompletionsByUserAsync(userId);
        List<LessonReviewDto> ordered = completedLessons
            .OrderBy(lc => lc.CompletionDate)
            .Select(lc => new LessonReviewDto
            {
                Question = lc.Character.Symbol
            })
            .ToList();

        return ordered;
    }

    public async Task<LessonReviewAnswerResultDto> CheckLessonReviewAnswerAsync(int userId, LessonReviewAnswerDto answer)
    {
        Character? character = await _repo.GetCharacterBySymbolAsync(answer.Question);
        if (character == null)
            throw new ArgumentException("Character not found", nameof(answer));

        Proficiency proficiency = await _repo.GetProficiencyAsync(userId, character.Id)
                                  ?? await LearnLessonAsync(userId, character.Id);

        if (answer.Answer == character.Romanization)
        {
            proficiency.Increase(SkillUp);
            await _repo.SaveChangesAsync();
            return new LessonReviewAnswerResultDto
            {
                IsCorrect = true,
                CorrectAnswer = character.Romanization
            };
        }

        proficiency.Decrease(SkillDown);
        await _repo.SaveChangesAsync();
        return new LessonReviewAnswerResultDto
        {
            IsCorrect = false,
            CorrectAnswer = character.Romanization
        };
    }

    private static LessonDto MapToLessonDto(Character character)
    {
        return new LessonDto
        {
            CharacterId = character.Id,
            Symbol = character.Symbol,
            Romanization = character.Romanization,
            Type = character.Type,
        };
    }
}
