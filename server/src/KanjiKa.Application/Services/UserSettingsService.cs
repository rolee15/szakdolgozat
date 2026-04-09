using KanjiKa.Application.DTOs.User;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly IUserRepository _userRepository;

    public UserSettingsService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserSettingsDto> GetSettingsAsync(int userId)
    {
        UserSettings? settings = await _userRepository.GetSettingsAsync(userId);

        if (settings == null)
            return new UserSettingsDto(10, 50, "N5");

        return new UserSettingsDto(settings.DailyLessonLimit, settings.ReviewBatchSize, settings.JlptLevel);
    }

    public async Task UpdateSettingsAsync(int userId, UpdateUserSettingsDto dto)
    {
        UserSettings? settings = await _userRepository.GetSettingsAsync(userId);

        if (settings == null)
        {
            settings = new UserSettings { UserId = userId };
        }

        settings.DailyLessonLimit = dto.DailyLessonLimit;
        settings.ReviewBatchSize = dto.ReviewBatchSize;
        settings.JlptLevel = dto.JlptLevel;

        await _userRepository.SaveSettingsAsync(settings);
    }
}
