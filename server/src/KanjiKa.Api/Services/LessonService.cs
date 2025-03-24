using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Interfaces;
using KanjiKa.Data;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Api.Services;

internal class LessonService : ILessonService
{
    private const int LessonReviewItemCount = 5;

    private readonly KanjiKaDbContext _db;

    public LessonService(KanjiKaDbContext db)
    {
        _db = db;
    }

    public async Task<TodayLessonCountDto> GetTodayLessonCountAsync(int userId)
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

        return new TodayLessonCountDto
        {
            Count = Math.Max(count, 0)
        };
    }

    public async Task<IEnumerable<LessonDto>> GetNewLessonsAsync(int userId, int pageIndex, int pageSize)
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

    public async Task<IEnumerable<LessonReviewDto>> GetLessonReviews(int userId)
    {
        var lessons=  await _db.Characters.Where(c =>
                !_db.Proficiencies
                    .Where(p => p.UserId == userId)
                    .Select(p => p.Id)
                    .Distinct()
                    .Contains(c.Id))
            .Take(LessonReviewItemCount)
            .Select(c => MapToLessonReviewDto(c))
            .ToListAsync();

        return lessons;
    }

    private static LessonReviewDto MapToLessonReviewDto(Character character)
    {
        return new LessonReviewDto
        {
            Symbol = character.Symbol,
        };
    }

    public async Task<bool> CheckReviewItemAnswerAsync(int userId, string character, LessonReviewAnswerDto answer)
    {
        var ch = await _db.Characters.FirstOrDefaultAsync(c => c.Symbol == character);
        if (ch == null)
        {
            throw new ArgumentException("Character not found", nameof(character));
        }

        var proficiency = await _db.Proficiencies
            .FirstOrDefaultAsync(p => p.UserId == userId && p.CharacterId == ch.Id) ?? await LearnLessonAsync(userId, ch.Id);

        if (answer.Answer == ch.Romanization)
        {
            proficiency.Increase(10);
            await _db.SaveChangesAsync();
            return true;
        }

        proficiency.Decrease(5);
        await _db.SaveChangesAsync();
        return false;
    }
}
