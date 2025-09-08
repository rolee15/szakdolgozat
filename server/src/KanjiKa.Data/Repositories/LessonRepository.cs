using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly KanjiKaDbContext _db;

    public LessonRepository(KanjiKaDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserWithProficienciesAsync(int userId)
    {
        return await _db.Users
            .Include(u => u.Proficiencies)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<int> CountLessonsCompletedTodayAsync(int userId)
    {
        DateTimeOffset today = DateTimeOffset.Now;
        return await _db.LessonCompletions.CountAsync(lc => lc.UserId == userId && lc.CompletionDate.Date == today.Date);
    }

    public async Task<List<Character>> GetAllCharactersAsync()
    {
        return await _db.Characters.ToListAsync();
    }

    public async Task<List<Character>> GetNewCharactersAsync(User user)
    {
        return await _db.Characters
            .Where(ch =>
                user.Proficiencies.All(p => p.CharacterId != ch.Id))
            .ToListAsync();
    }

    public async Task<Character?> GetCharacterByIdAsync(int characterId)
    {
        return await _db.Characters.FirstOrDefaultAsync(c => c.Id == characterId);
    }

    public async Task<Character?> GetCharacterBySymbolAsync(string symbol)
    {
        return await _db.Characters.FirstOrDefaultAsync(c => c.Symbol == symbol);
    }

    public async Task<Proficiency?> GetProficiencyAsync(int userId, int characterId)
    {
        return await _db.Proficiencies.FirstOrDefaultAsync(p => p.UserId == userId && p.CharacterId == characterId);
    }

    public async Task AddProficiencyAsync(Proficiency proficiency)
    {
        await _db.Proficiencies.AddAsync(proficiency);
    }

    public async Task AddLessonCompletionAsync(LessonCompletion completion)
    {
        await _db.LessonCompletions.AddAsync(completion);
    }

    public async Task<List<LessonCompletion>> GetLessonCompletionsByUserAsync(int userId)
    {
        return await _db.LessonCompletions
            .Include(lc => lc.Character)
            .Where(lc => lc.UserId == userId).ToListAsync();
    }

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}
