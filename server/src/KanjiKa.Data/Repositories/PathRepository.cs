using KanjiKa.Core.Entities.Path;
using KanjiKa.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

// ContentType alias to avoid ambiguity with KanjiKa.Core.Entities.Path.ContentType
using PathContentType = KanjiKa.Core.Entities.Path.ContentType;

namespace KanjiKa.Data.Repositories;

public class PathRepository : IPathRepository
{
    private readonly KanjiKaDbContext _context;

    public PathRepository(KanjiKaDbContext context)
    {
        _context = context;
    }

    public async Task<List<LearningUnit>> GetAllUnitsAsync()
    {
        return await _context.LearningUnits
            .Include(u => u.Contents.OrderBy(c => c.SortOrder))
            .OrderBy(u => u.SortOrder)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<LearningUnit?> GetUnitByIdAsync(int id)
    {
        return await _context.LearningUnits
            .Include(u => u.Contents.OrderBy(c => c.SortOrder))
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<LearningUnit?> GetUnitWithTestAsync(int id)
    {
        return await _context.LearningUnits
            .Include(u => u.Tests.OrderBy(t => t.SortOrder))
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Dictionary<int, UnitProgress>> GetProgressForUserAsync(int userId, List<int> unitIds)
    {
        return await _context.UnitProgresses
            .Where(up => up.UserId == userId && unitIds.Contains(up.LearningUnitId))
            .AsNoTracking()
            .ToDictionaryAsync(up => up.LearningUnitId);
    }

    public async Task UpsertProgressAsync(UnitProgress progress)
    {
        var existing = await _context.UnitProgresses
            .FirstOrDefaultAsync(up => up.UserId == progress.UserId && up.LearningUnitId == progress.LearningUnitId);

        if (existing == null)
        {
            _context.UnitProgresses.Add(progress);
        }
        else
        {
            if (progress.BestScore > existing.BestScore)
                existing.BestScore = progress.BestScore;
            existing.IsPassed = existing.IsPassed || progress.IsPassed;
            existing.AttemptCount = progress.AttemptCount;
            existing.LastAttemptAt = progress.LastAttemptAt;
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<string?> GetContentTitleAsync(PathContentType contentType, int contentId)
    {
        return contentType switch
        {
            PathContentType.Kana => await _context.Characters
                .Where(c => c.Id == contentId)
                .Select(c => c.Symbol + " (" + c.Romanization + ")")
                .FirstOrDefaultAsync(),
            PathContentType.Kanji => await _context.Kanjis
                .Where(k => k.Id == contentId)
                .Select(k => k.Character)
                .FirstOrDefaultAsync(),
            PathContentType.Grammar => await _context.GrammarPoints
                .Where(g => g.Id == contentId)
                .Select(g => g.Title)
                .FirstOrDefaultAsync(),
            PathContentType.Reading => await _context.ReadingPassages
                .Where(r => r.Id == contentId)
                .Select(r => r.Title)
                .FirstOrDefaultAsync(),
            _ => null
        };
    }
}
