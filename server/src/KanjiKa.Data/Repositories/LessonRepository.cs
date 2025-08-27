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

    public async Task<User?> GetUserWithProficienciesAsync(int userId)
    {
        return await _db.Users
            .Include(u => u.Proficiencies)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<int> CountLessonsCompletedTodayAsync(int userId, DateTimeOffset todayUtcDate)
    {
        var today = todayUtcDate.Date;
        return await _db.LessonCompletions.CountAsync(lc => lc.UserId == userId && lc.CompletionDate.Date == today);
    }

    public async Task<List<Character>> GetAllCharactersAsync()
    {
        return await _db.Characters.ToListAsync();
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

    public Task AddProficiencyAsync(Proficiency proficiency)
    {
        _db.Proficiencies.Add(proficiency);
        return Task.CompletedTask;
    }

    public Task AddLessonCompletionAsync(LessonCompletion completion)
    {
        _db.LessonCompletions.Add(completion);
        return Task.CompletedTask;
    }

    public async Task<List<LessonCompletion>> GetLessonCompletionsByUserAsync(int userId)
    {
        return await _db.LessonCompletions.Where(lc => lc.UserId == userId).ToListAsync();
    }

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}
