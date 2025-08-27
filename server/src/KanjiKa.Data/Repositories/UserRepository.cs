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

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public Task UpdateAsync(User user)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}
