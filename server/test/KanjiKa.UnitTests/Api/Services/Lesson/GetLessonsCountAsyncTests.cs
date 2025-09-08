using KanjiKa.Api.Services;
using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services.Lesson;

public class GetLessonsCountAsyncTests
{
    [Fact]
    public async Task GetLessonsCountAsync_WhenUserNotFound_ThrowsArgumentException()
    {
        // Arrange
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(It.IsAny<int>())).ReturnsAsync((User?)null);
        var service = new LessonService(repo.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetLessonsCountAsync(1));
        Assert.Equal("userId", ex.ParamName);
    }

    [Theory]
    [InlineData(-5, 15)]
    [InlineData(0, 15)]
    [InlineData(5, 10)]
    [InlineData(15, 0)]
    [InlineData(20, 0)]
    public async Task GetLessonsCountAsync_ReturnsRemainingLessonsCount(int completedToday, int expected)
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(completedToday);
        var service = new LessonService(repo.Object);

        // Act
        LessonsCountDto result = await service.GetLessonsCountAsync(1);

        // Assert
        Assert.Equal(expected, result.Count);
    }
}
