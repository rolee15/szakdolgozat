using KanjiKa.Application.DTOs.User;
using KanjiKa.Application.Interfaces;
using KanjiKa.Application.Services;
using KanjiKa.Domain.Entities.Users;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class UserSettingsServiceTest
{
    [Fact]
    public async Task GetSettings_WhenSettingsExist_ReturnsDto()
    {
        var repo = new Mock<IUserRepository>();
        var existing = new UserSettings { Id = 1, UserId = 1, DailyLessonLimit = 20, ReviewBatchSize = 100, JlptLevel = "N3" };
        repo.Setup(r => r.GetSettingsAsync(1)).ReturnsAsync(existing);

        var service = new UserSettingsService(repo.Object);
        UserSettingsDto result = await service.GetSettingsAsync(1);

        Assert.Equal(20, result.DailyLessonLimit);
        Assert.Equal(100, result.ReviewBatchSize);
        Assert.Equal("N3", result.JlptLevel);
    }

    [Fact]
    public async Task GetSettings_WhenSettingsNull_ReturnsDefaults()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetSettingsAsync(1)).ReturnsAsync((UserSettings?)null);

        var service = new UserSettingsService(repo.Object);
        UserSettingsDto result = await service.GetSettingsAsync(1);

        Assert.Equal(10, result.DailyLessonLimit);
        Assert.Equal(50, result.ReviewBatchSize);
        Assert.Equal("N5", result.JlptLevel);
    }

    [Fact]
    public async Task UpdateSettings_WhenSettingsExist_UpdatesFields()
    {
        var repo = new Mock<IUserRepository>();
        var existing = new UserSettings { Id = 5, UserId = 1, DailyLessonLimit = 10, ReviewBatchSize = 50, JlptLevel = "N5" };
        repo.Setup(r => r.GetSettingsAsync(1)).ReturnsAsync(existing);
        repo.Setup(r => r.SaveSettingsAsync(existing)).Returns(Task.CompletedTask);

        var service = new UserSettingsService(repo.Object);
        var dto = new UpdateUserSettingsDto { DailyLessonLimit = 15, ReviewBatchSize = 75, JlptLevel = "N4" };
        await service.UpdateSettingsAsync(1, dto);

        Assert.Equal(15, existing.DailyLessonLimit);
        Assert.Equal(75, existing.ReviewBatchSize);
        Assert.Equal("N4", existing.JlptLevel);
    }

    [Fact]
    public async Task UpdateSettings_WhenSettingsNull_CreatesNewSettings()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetSettingsAsync(2)).ReturnsAsync((UserSettings?)null);
        UserSettings? saved = null;
        repo.Setup(r => r.SaveSettingsAsync(It.IsAny<UserSettings>()))
            .Callback<UserSettings>(s => saved = s)
            .Returns(Task.CompletedTask);

        var service = new UserSettingsService(repo.Object);
        var dto = new UpdateUserSettingsDto { DailyLessonLimit = 5, ReviewBatchSize = 30, JlptLevel = "N2" };
        await service.UpdateSettingsAsync(2, dto);

        Assert.NotNull(saved);
        Assert.Equal(2, saved.UserId);
        Assert.Equal(0, saved.Id);
    }

    [Fact]
    public async Task UpdateSettings_SetsCorrectDailyLessonLimit()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetSettingsAsync(1)).ReturnsAsync((UserSettings?)null);
        UserSettings? saved = null;
        repo.Setup(r => r.SaveSettingsAsync(It.IsAny<UserSettings>()))
            .Callback<UserSettings>(s => saved = s)
            .Returns(Task.CompletedTask);

        var service = new UserSettingsService(repo.Object);
        var dto = new UpdateUserSettingsDto { DailyLessonLimit = 25, ReviewBatchSize = 50, JlptLevel = "N5" };
        await service.UpdateSettingsAsync(1, dto);

        Assert.Equal(25, saved!.DailyLessonLimit);
    }

    [Fact]
    public async Task UpdateSettings_SetsCorrectJlptLevel()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetSettingsAsync(1)).ReturnsAsync((UserSettings?)null);
        UserSettings? saved = null;
        repo.Setup(r => r.SaveSettingsAsync(It.IsAny<UserSettings>()))
            .Callback<UserSettings>(s => saved = s)
            .Returns(Task.CompletedTask);

        var service = new UserSettingsService(repo.Object);
        var dto = new UpdateUserSettingsDto { DailyLessonLimit = 10, ReviewBatchSize = 50, JlptLevel = "N1" };
        await service.UpdateSettingsAsync(1, dto);

        Assert.Equal("N1", saved!.JlptLevel);
    }

    [Fact]
    public async Task UpdateSettings_CallsSaveSettings()
    {
        var repo = new Mock<IUserRepository>();
        var existing = new UserSettings { Id = 3, UserId = 1, DailyLessonLimit = 10, ReviewBatchSize = 50, JlptLevel = "N5" };
        repo.Setup(r => r.GetSettingsAsync(1)).ReturnsAsync(existing);
        repo.Setup(r => r.SaveSettingsAsync(existing)).Returns(Task.CompletedTask).Verifiable();

        var service = new UserSettingsService(repo.Object);
        var dto = new UpdateUserSettingsDto { DailyLessonLimit = 10, ReviewBatchSize = 50, JlptLevel = "N5" };
        await service.UpdateSettingsAsync(1, dto);

        repo.Verify(r => r.SaveSettingsAsync(existing), Times.Once);
    }
}
