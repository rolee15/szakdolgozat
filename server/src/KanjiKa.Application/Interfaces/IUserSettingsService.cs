using KanjiKa.Application.DTOs.User;

namespace KanjiKa.Application.Interfaces;

public interface IUserSettingsService
{
    Task<UserSettingsDto> GetSettingsAsync(int userId);

    Task UpdateSettingsAsync(int userId, UpdateUserSettingsDto dto);
}
