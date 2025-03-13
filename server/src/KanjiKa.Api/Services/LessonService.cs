using KanjiKa.Core.Dtos;
using KanjiKa.Core.Dtos.Learning;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Data;
using KanjiKa.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKaApi.Services;

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
        var count = await _db.Characters.CountAsync(c =>
            !_db.Proficiencies
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .Distinct()
                .Contains(c.Id));

        return new TodayLessonCountDto()
        {
            //TODO decrement within a day
            Count = count > 15 ? 15 : count
        };
    }

    public async Task<IEnumerable<LessonDto>> GetNewLessonsAsync(int userId, int pageIndex, int pageSize)
    {
        var lessons=  await _db.Characters.Where(c =>
            !_db.Proficiencies
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .Distinct()
                .Contains(c.Id))
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(c => MapToLessonDto(c))
            .ToListAsync();

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
        var found = await _db.Characters.AnyAsync(c => c.Id == characterId);
        if (!found)
        {
            throw new ArgumentException("Character not found", nameof(characterId));
        }

        var proficiency = new Proficiency
        {
            UserId = userId,
            CharacterId = characterId,
            Level = 0,
            LearnedAt = DateTimeOffset.UtcNow
        };

        _db.Proficiencies.Add(proficiency);
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
