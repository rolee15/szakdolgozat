using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly KanjiKaDbContext _db;

    public UserRepository(KanjiKaDbContext db)
    {
        _db = db;
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public Task<User?> GetByIdAsync(int id)
    {
        return _db.Users.FindAsync(id).AsTask();
    }

    public Task<User?> GetByIdWithStatsAsync(int id)
    {
        return _db.Users
            .Include(u => u.Proficiencies).ThenInclude(p => p.Character)
            .Include(u => u.LessonCompletions).ThenInclude(lc => lc.Character)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<(List<User> Users, int TotalCount)> GetUsersPagedAsync(int page, int pageSize, string? search)
    {
        var query = _db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u => u.Username.Contains(search));
        }

        var totalCount = await query.CountAsync();
        var users = await query
            .Include(u => u.Proficiencies)
            .Include(u => u.LessonCompletions)
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTimeOffset expiry)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return;
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiry;
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return false;
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}
