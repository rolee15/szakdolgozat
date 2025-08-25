using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Interfaces;
using KanjiKa.Data;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Api.Services;

internal class LessonService : ILessonService
{
    private const int SkillUp = 10;
    private const int SkillDown = 5;

    private readonly KanjiKaDbContext _db;

    public LessonService(KanjiKaDbContext db)
    {
        _db = db;
    }

    public async Task<LessonsCountDto> GetLessonsCountAsync(int userId)
    {
        var user = await _db.Users
            .Include(u => u.Proficiencies)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        var today = DateTimeOffset.UtcNow.Date;
        var lessonsLearnedToday = await _db.LessonCompletions
            .CountAsync(lc => lc.UserId == userId && lc.CompletionDate.Date == today);
        var count = 15 - lessonsLearnedToday;

        return new LessonsCountDto
        {
            Count = Math.Max(count, 0)
        };
    }

    public async Task<IEnumerable<LessonDto>> GetLessonsAsync(int userId, int pageIndex, int pageSize)
    {
        var user = await _db.Users
            .Include(u => u.Proficiencies)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        var today = DateTimeOffset.UtcNow.Date;
        var lessonsLearnedToday = await _db.LessonCompletions
            .CountAsync(lc => lc.UserId == userId && lc.CompletionDate.Date == today);
        var count = 15 - lessonsLearnedToday;

        if (count <= 0)
        {
            return Array.Empty<LessonDto>();
        }

        var allCharacters = await _db.Characters.ToListAsync();
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

    public async Task<Proficiency> LearnLessonAsync(int userId, int characterId)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new ArgumentException("User not found", nameof(userId));
        }

        var character = await _db.Characters.FirstOrDefaultAsync(c => c.Id == characterId);
        if (character == null)
        {
            throw new ArgumentException("Character not found", nameof(characterId));
        }

        var existingProficiency = await _db.Proficiencies
            .FirstOrDefaultAsync(p => p.UserId == userId && p.CharacterId == characterId);
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
        _db.Proficiencies.Add(proficiency);

        var lessonCompletion = new LessonCompletion
        {
            UserId = user.Id,
            CharacterId = character.Id,
            CompletionDate = DateTimeOffset.UtcNow
        };
        _db.LessonCompletions.Add(lessonCompletion);

        await _db.SaveChangesAsync();

        return proficiency;
    }

    public async Task<LessonReviewsCountDto> GetLessonReviewsCountAsync(int userId)
    {
        var completedLessons = await _db.LessonCompletions
            .Where(lc => lc.UserId == userId)
            .ToListAsync();

        return new LessonReviewsCountDto
        {
            Count = completedLessons.Count
        };
    }

    public async Task<IEnumerable<LessonReviewDto>> GetLessonReviewsAsync(int userId)
    {
        var completedLessons = await _db.LessonCompletions
            .Where(lc => lc.UserId == userId)
            .OrderBy(lc => lc.CompletionDate)
            .Select(lc => new LessonReviewDto { Question = lc.Character.Symbol })
            .ToListAsync();

        return completedLessons;
    }

    public async Task<LessonReviewAnswerResultDto> CheckLessonReviewAnswerAsync(int userId, LessonReviewAnswerDto answer)
    {
        var character = await _db.Characters.FirstOrDefaultAsync(c => c.Symbol == answer.Question);
        if (character == null)
        {
            throw new ArgumentException("Character not found", nameof(answer));
        }

        var proficiency = await _db.Proficiencies
            .FirstOrDefaultAsync(p => p.UserId == userId && p.CharacterId == character.Id) ?? await LearnLessonAsync(userId, character.Id);

        if (answer.Answer == character.Romanization)
        {
            proficiency.Increase(SkillUp);
            await _db.SaveChangesAsync();
            return new LessonReviewAnswerResultDto { IsCorrect = true };
        }

        proficiency.Decrease(SkillDown);
        await _db.SaveChangesAsync();
        return new LessonReviewAnswerResultDto { IsCorrect = false };
    }
}
