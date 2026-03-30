using KanjiKa.Core.Entities.Kanji;
using KanjiKa.Core.Interfaces;
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
}
