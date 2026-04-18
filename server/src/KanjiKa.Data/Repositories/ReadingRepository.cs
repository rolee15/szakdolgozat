using KanjiKa.Domain.Entities.Reading;
using KanjiKa.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Repositories;

public class ReadingRepository : IReadingRepository
{
    private readonly KanjiKaDbContext _context;

    public ReadingRepository(KanjiKaDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReadingPassage>> GetAllAsync()
    {
        return await _context.ReadingPassages
            .OrderBy(p => p.SortOrder)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ReadingPassage?> GetByIdAsync(int id)
    {
        return await _context.ReadingPassages
            .Include(p => p.Questions.OrderBy(q => q.SortOrder))
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Dictionary<int, ReadingProficiency>> GetProficienciesForUserAsync(int userId, List<int> passageIds)
    {
        return await _context.ReadingProficiencies
            .Where(rp => rp.UserId == userId && passageIds.Contains(rp.ReadingPassageId))
            .AsNoTracking()
            .ToDictionaryAsync(rp => rp.ReadingPassageId);
    }

    public async Task UpsertProficiencyAsync(ReadingProficiency proficiency)
    {
        ReadingProficiency? existing = await _context.ReadingProficiencies
            .FirstOrDefaultAsync(rp => rp.UserId == proficiency.UserId && rp.ReadingPassageId == proficiency.ReadingPassageId);

        if (existing == null)
        {
            _context.ReadingProficiencies.Add(proficiency);
        }
        else
        {
            existing.Score = proficiency.Score;
            existing.AttemptCount = proficiency.AttemptCount;
            existing.IsPassed = proficiency.IsPassed;
            existing.LastAttemptAt = proficiency.LastAttemptAt;
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
