using KanjiKa.Core.Entities.Users;

namespace KanjiKa.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);

    Task AddAsync(User user);

    Task UpdateAsync(User user);

    Task SaveChangesAsync();
}
