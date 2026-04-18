using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Repositories;

public class KanaRepository : IKanaRepository
{
    private readonly KanjiKaDbContext _db;

    public KanaRepository(KanjiKaDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetUserWithProficienciesAsync(int userId)
    {
        return await _db.Users
            .Include(x => x.KanaProficiencies)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<Character?> GetCharacterBySymbolAndTypeAsync(string symbol, KanaType type)
    {
        return await _db.Characters
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Symbol == symbol && c.Type == type);
    }

    public async Task<Character?> GetCharacterWithExamplesBySymbolAndTypeAsync(string symbol, KanaType type)
    {
        return await _db.Characters
            .Include(x => x.Examples)
            .FirstOrDefaultAsync(c => c.Symbol == symbol && c.Type == type);
    }

    public IEnumerable<Character> GetCharactersByType(KanaType type)
    {
        return _db.Characters
            .AsNoTracking()
            .Where(c => c.Type == type);
    }
}
