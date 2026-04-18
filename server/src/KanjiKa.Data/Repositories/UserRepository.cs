using KanjiKa.Domain.Entities.Users;
using KanjiKa.Domain.Exceptions;
using KanjiKa.Application.Interfaces;
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

    public Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return _db.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public Task<User?> GetByActivationTokenAsync(string token)
    {
        return _db.Users.FirstOrDefaultAsync(u => u.ActivationToken == token);
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

    public async Task<(List<UserSummary> Users, int TotalCount)> GetUsersPagedAsync(int page, int pageSize, string? search)
    {
        IQueryable<User> query = _db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u => u.Username.Contains(search));
        }

        int totalCount = await query.CountAsync();
        List<UserSummary> users = await query
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserSummary
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role,
                MustChangePassword = u.MustChangePassword,
                ProficiencyCount = u.Proficiencies.Count(),
                LessonCompletionCount = u.LessonCompletions.Count()
            })
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
        User? user = await _db.Users.FindAsync(userId);
        if (user == null) return;

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiry;
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        User? user = await _db.Users.FindAsync(id);
        if (user == null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("23505") == true
            || ex.InnerException?.Message.Contains("unique") == true)
        {
            throw new DuplicateUsernameException();
        }
    }

    public Task<UserSettings?> GetSettingsAsync(int userId)
    {
        return _db.UserSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    public async Task SaveSettingsAsync(UserSettings settings)
    {
        if (settings.Id == 0)
            _db.UserSettings.Add(settings);
        else
            _db.UserSettings.Update(settings);

        await _db.SaveChangesAsync();
    }
}
