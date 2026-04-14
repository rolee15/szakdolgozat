using KanjiKa.Domain.Entities.Kanji;
using KanjiKa.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Repositories;

public class KanjiRepository : IKanjiRepository
{
    private readonly KanjiKaDbContext _context;

    public KanjiRepository(KanjiKaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Kanji>> GetByJlptLevelAsync(int jlptLevel)
    {
        return await _context.Kanjis
            .Include(k => k.Examples)
            .Where(k => k.JlptLevel == jlptLevel)
            .OrderBy(k => k.Character)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Kanji?> GetByCharacterAsync(string character)
    {
        return await _context.Kanjis
            .Include(k => k.Examples)
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.Character == character);
    }

    public async Task<(List<Kanji> Items, int TotalCount)> GetPagedAsync(int? jlptLevel, int page, int pageSize)
    {
        IQueryable<Kanji> query = _context.Kanjis.AsNoTracking();
        if (jlptLevel.HasValue)
            query = query.Where(k => k.JlptLevel == jlptLevel.Value);
        int totalCount = await query.CountAsync();
        List<Kanji> items = await query
            .OrderBy(k => k.JlptLevel == 0 ? int.MaxValue : k.JlptLevel)
            .ThenBy(k => k.Character)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<Dictionary<int, KanjiProficiency>> GetProficienciesForUserAsync(int userId, List<int> kanjiIds)
    {
        return await _context.KanjiProficiencies
            .Where(kp => kp.UserId == userId && kanjiIds.Contains(kp.KanjiId))
            .AsNoTracking()
            .ToDictionaryAsync(kp => kp.KanjiId);
    }

    public async Task<List<KanjiProficiency>> GetDueReviewsAsync(int userId)
    {
        return await _context.KanjiProficiencies
            .Include(kp => kp.Kanji)
            .Where(kp => kp.UserId == userId && kp.NextReviewDate <= DateTimeOffset.UtcNow)
            .ToListAsync();
    }

    public async Task<KanjiProficiency?> GetProficiencyAsync(int userId, int kanjiId)
    {
        return await _context.KanjiProficiencies
            .FirstOrDefaultAsync(kp => kp.UserId == userId && kp.KanjiId == kanjiId);
    }

    public Task AddProficiencyAsync(KanjiProficiency proficiency)
    {
        _context.KanjiProficiencies.Add(proficiency);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
