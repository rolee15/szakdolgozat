using KanjiKa.Core.Entities.Grammar;
using KanjiKa.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Repositories;

public class GrammarRepository : IGrammarRepository
{
    private readonly KanjiKaDbContext _context;

    public GrammarRepository(KanjiKaDbContext context)
    {
        _context = context;
    }

    public async Task<List<GrammarPoint>> GetAllAsync()
    {
        return await _context.GrammarPoints
            .OrderBy(g => g.SortOrder)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<GrammarPoint?> GetByIdAsync(int id)
    {
        return await _context.GrammarPoints
            .Include(g => g.Examples)
            .Include(g => g.Exercises)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<GrammarExercise?> GetExerciseByIdAsync(int exerciseId)
    {
        return await _context.GrammarExercises
            .FirstOrDefaultAsync(e => e.Id == exerciseId);
    }

    public async Task<Dictionary<int, GrammarProficiency>> GetProficienciesForUserAsync(int userId, List<int> grammarPointIds)
    {
        return await _context.GrammarProficiencies
            .Where(gp => gp.UserId == userId && grammarPointIds.Contains(gp.GrammarPointId))
            .AsNoTracking()
            .ToDictionaryAsync(gp => gp.GrammarPointId);
    }

    public async Task<GrammarProficiency?> GetProficiencyAsync(int userId, int grammarPointId)
    {
        return await _context.GrammarProficiencies
            .FirstOrDefaultAsync(gp => gp.UserId == userId && gp.GrammarPointId == grammarPointId);
    }

    public async Task AddProficiencyAsync(GrammarProficiency proficiency)
    {
        _context.GrammarProficiencies.Add(proficiency);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
