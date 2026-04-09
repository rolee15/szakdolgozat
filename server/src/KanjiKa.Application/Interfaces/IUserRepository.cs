using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);

    Task<User?> GetByRefreshTokenAsync(string refreshToken);

    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByIdWithStatsAsync(int id);

    Task<(List<User> Users, int TotalCount)> GetUsersPagedAsync(int page, int pageSize, string? search);

    Task AddAsync(User user);

    Task UpdateAsync(User user);

    Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTimeOffset expiry);

    Task<bool> DeleteByIdAsync(int id);

    Task SaveChangesAsync();

    Task<UserSettings?> GetSettingsAsync(int userId);

    Task SaveSettingsAsync(UserSettings settings);
}
