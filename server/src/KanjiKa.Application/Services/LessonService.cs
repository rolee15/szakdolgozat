using KanjiKa.Application.DTOs.Learning;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Common;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Learning;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.Services;

public class LessonService : ILessonService
{
    private const int LessonsPerDayCount = 15;

    private readonly ILessonRepository _repo;

    public LessonService(ILessonRepository repo)
    {
        _repo = repo;
    }

    public async Task<LessonsCountDto> GetLessonsCountAsync(int userId)
    {
        User? user = await _repo.GetUserWithProficienciesAsync(userId);
        if (user == null) throw new ArgumentException("User not found", nameof(userId));

        int lessonsLearnedToday = await _repo.CountLessonsCompletedTodayAsync(userId);
        int count = LessonsPerDayCount - lessonsLearnedToday;

        List<int> learnedCharacterIds = user.KanaProficiencies.Select(p => p.CharacterId).ToList();
        int unlearnedCount = await _repo.CountNewCharactersAsync(learnedCharacterIds);

        return new LessonsCountDto
        {
            Count = Math.Min(Math.Clamp(count, 0, LessonsPerDayCount), unlearnedCount)
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
        IEnumerable<LessonDto> lessons = (await _repo.GetNewCharactersAsync(user.KanaProficiencies))
            .Skip(pageIndex * takeSize)
            .Take(takeSize)
            .Select(MapToLessonDto);

        return lessons;
    }

    public async Task<KanaProficiency> LearnLessonAsync(int userId, int characterId)
    {
        User? user = await _repo.GetUserAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found", nameof(userId));

        Character? character = await _repo.GetCharacterByIdAsync(characterId);
        if (character == null)
            throw new ArgumentException("Character not found", nameof(characterId));

        KanaProficiency? existingProficiency = await _repo.GetProficiencyAsync(userId, characterId);
        if (existingProficiency != null)
            throw new ArgumentException("Character already learned", nameof(characterId));

        var proficiency = new KanaProficiency
        {
            UserId = user.Id,
            CharacterId = character.Id,
            SrsStage = SrsStage.Apprentice1,
            NextReviewDate = SrsIntervals.GetNextReviewDate(SrsStage.Apprentice1),
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
        int count = await GetDueReviewsCountAsync(userId);
        return new LessonReviewsCountDto { Count = count };
    }

    public async Task<IEnumerable<LessonReviewDto>> GetLessonReviewsAsync(int userId)
    {
        List<KanaProficiency> dueReviews = await _repo.GetDueReviewsAsync(userId);
        List<LessonReviewDto> ordered = dueReviews
            .OrderBy(p => p.NextReviewDate)
            .Select(p => new LessonReviewDto
            {
                Question = p.Character!.Symbol
            })
            .ToList();

        return ordered;
    }

    public async Task<LessonReviewAnswerResultDto> CheckLessonReviewAnswerAsync(int userId, LessonReviewAnswerDto answer)
    {
        Character? character = await _repo.GetCharacterBySymbolAsync(answer.Question);
        if (character == null)
            throw new ArgumentException("Character not found", nameof(answer));

        KanaProficiency proficiency = await _repo.GetProficiencyAsync(userId, character.Id)
                                  ?? await LearnLessonAsync(userId, character.Id);

        bool isCorrect = answer.Answer == character.Romanization;
        if (isCorrect)
            proficiency.AnswerCorrectly();
        else
            proficiency.AnswerIncorrectly();

        await _repo.SaveChangesAsync();
        return BuildReviewResult(isCorrect, character.Romanization, proficiency);
    }

    public async Task<LessonReviewsCountDto> GetWritingReviewsCountAsync(int userId)
    {
        int count = await GetDueReviewsCountAsync(userId);
        return new LessonReviewsCountDto { Count = count };
    }

    public async Task<IEnumerable<WritingReviewDto>> GetWritingReviewsAsync(int userId)
    {
        List<KanaProficiency> dueReviews = await _repo.GetDueReviewsAsync(userId);
        List<WritingReviewDto> ordered = dueReviews
            .OrderBy(p => p.NextReviewDate)
            .Select(p => new WritingReviewDto
            {
                CharacterId = p.Character!.Id,
                Romanization = p.Character.Romanization,
                CharacterType = p.Character.Type.ToString().ToLower()
            })
            .ToList();

        return ordered;
    }

    public async Task<LessonReviewAnswerResultDto> CheckWritingReviewAnswerAsync(int userId, WritingReviewAnswerDto answer)
    {
        Character? character = await _repo.GetCharacterByIdAsync(answer.CharacterId);
        if (character == null)
            throw new ArgumentException("Character not found", nameof(answer));

        KanaProficiency? proficiency = await _repo.GetProficiencyAsync(userId, character.Id);
        if (proficiency is null)
            throw new ArgumentException("No proficiency record found for this character.", nameof(answer));

        bool isCorrect = answer.TypedCharacter == character.Symbol;
        if (isCorrect)
            proficiency.AnswerCorrectly();
        else
            proficiency.AnswerIncorrectly();

        await _repo.SaveChangesAsync();
        return BuildReviewResult(isCorrect, character.Symbol, proficiency);
    }

    private async Task<int> GetDueReviewsCountAsync(int userId)
    {
        List<KanaProficiency> dueReviews = await _repo.GetDueReviewsAsync(userId);
        return dueReviews.Count;
    }

    private static LessonReviewAnswerResultDto BuildReviewResult(bool isCorrect, string correctAnswer, KanaProficiency proficiency)
    {
        return new LessonReviewAnswerResultDto
        {
            IsCorrect = isCorrect,
            CorrectAnswer = correctAnswer,
            SrsStage = (int)proficiency.SrsStage,
            SrsStageName = SrsIntervals.GetStageName(proficiency.SrsStage),
            NextReviewDate = proficiency.NextReviewDate
        };
    }

    private static LessonDto MapToLessonDto(Character character)
    {
        return new LessonDto
        {
            CharacterId = character.Id,
            Symbol = character.Symbol,
            Romanization = character.Romanization,
            Type = character.Type
        };
    }
}
