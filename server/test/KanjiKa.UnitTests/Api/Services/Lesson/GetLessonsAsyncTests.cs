using System.Runtime.InteropServices;
using KanjiKa.Api.Services;
using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services.Lesson;

public class GetLessonsAsyncTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public async Task GetLessonsAsync_WhenPageIndexOutOfRange_ThrowsArgumentException(int pageIndex)
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        var service = new LessonService(repo.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetLessonsAsync(1, pageIndex, 10));
        Assert.Equal("pageIndex", ex.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public async Task GetLessonsAsync_WhenPageSizeOutOfRange_ThrowsArgumentException(int pageSize)
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        var service = new LessonService(repo.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetLessonsAsync(1, 0, pageSize));
        Assert.Equal("pageSize", ex.ParamName);
    }

    [Fact]
    public async Task GetLessonsAsync_WhenUserNotFound_ThrowsArgumentException()
    {
        // Arrange
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(It.IsAny<int>())).ReturnsAsync((User?)null);
        var service = new LessonService(repo.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetLessonsAsync(1, 0, 10));
        Assert.Equal("userId", ex.ParamName);
    }

    // This one is for when the user did a different number of lessons today
    [Theory]
    [InlineData(-5, 15)]
    [InlineData(0, 15)]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(15, 0)]
    [InlineData(20, 0)]
    public async Task GetLessonsAsync_WhenLessonsLeft_ReturnsExpectedCount(int completedToday, int expected)
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        List<Character> characters = Enumerable.Range(1, 15).Select(i => new Character { Id = i, Symbol = $"Symbol{i}", Romanization = $"Romanization{i}" }).ToList();

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(completedToday);
        repo.Setup(r => r.GetNewCharactersAsync(user.Proficiencies)).ReturnsAsync(characters);
        var service = new LessonService(repo.Object);

        // Act
        int result = (await service.GetLessonsAsync(1, 0, 20)).Count();

        // Assert
        Assert.Equal(expected, result);
    }

    // This one is for when the user has no proficiency, but there are different number of characters
    [Theory]
    [InlineData(20, 15)]
    [InlineData(15, 15)]
    [InlineData(10, 10)]
    [InlineData(5, 5)]
    [InlineData(0, 0)]
    public async Task GetLessonsAsync_WhenNoProficiency_ReturnsExpectedCount(int charactersCount, int expected)
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        List<Character> characters = Enumerable.Range(1, charactersCount).Select(i => new Character { Id = i, Symbol = $"Symbol{i}", Romanization = $"Romanization{i}" }).ToList();

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        repo.Setup(r => r.GetNewCharactersAsync(user.Proficiencies)).ReturnsAsync(characters);
        var service = new LessonService(repo.Object);

        // Act
        int result = (await service.GetLessonsAsync(1, 0, 20)).Count();

        // Assert
        Assert.Equal(expected, result);
    }

    // Test edge cases of pagination
    // 1. Zero elements
    // 2. Zero page size
    // 3. Page size > elements
    // 4. Page size < elements
    // 5. Page index < 0
    // 6. Page index > elements / page size
    // 7. The last page returns fewer elements than page size
    // 8. The middle page returns page size elements

    // 1. Zero elements
    [Fact]
    public async Task GetLessonsAsync_WhenZeroElements_ReturnsEmpty()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        repo.Setup(r => r.GetNewCharactersAsync(user.Proficiencies)).ReturnsAsync([]);
        var service = new LessonService(repo.Object);

        // Act
        List<LessonDto> result = (await service.GetLessonsAsync(1, 0, 10)).ToList();

        // Assert
        Assert.Empty(result);
    }

    // 2. Zero page sizes
    [Fact]
    public async Task GetLessonsAsync_WhenZeroPageSize_ThrowsArgumentException()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        List<Character> characters = Enumerable.Range(1, 10).Select(i => new Character { Id = i, Symbol = $"Symbol{i}", Romanization = $"Romanization{i}" }).ToList();

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        repo.Setup(r => r.GetNewCharactersAsync(user.Proficiencies)).ReturnsAsync(characters);
        var service = new LessonService(repo.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetLessonsAsync(1, 0, 0));
        Assert.Equal("pageSize", ex.ParamName);
    }

    // 3. Page size > elements
    [Fact]
    public async Task GetLessonsAsync_WhenPageSizeGreaterThanElements_ReturnsAllElements()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        List<Character> characters = Enumerable.Range(1, 6).Select(i => new Character { Id = i, Symbol = $"Symbol{i}", Romanization = $"Romanization{i}" }).ToList();

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        repo.Setup(r => r.GetNewCharactersAsync(user.Proficiencies)).ReturnsAsync(characters);
        var service = new LessonService(repo.Object);

        // Act
        List<LessonDto> result = (await service.GetLessonsAsync(1, 0, 10)).ToList();

        // Assert
        Assert.Equal(6, result.Count);
        Assert.Equal([1, 2, 3, 4, 5, 6], result.Select(l => l.CharacterId));
        Assert.DoesNotContain(7, result.Select(l => l.CharacterId));
    }

    // 4. Page size < elements
    [Fact]
    public async Task GetLessonsAsync_WhenPageSizeLesserThanElements_ReturnsPageSizeElements()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        List<Character> characters = Enumerable.Range(1, 10).Select(i => new Character { Id = i, Symbol = $"Symbol{i}", Romanization = $"Romanization{i}" }).ToList();

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        repo.Setup(r => r.GetNewCharactersAsync(user.Proficiencies)).ReturnsAsync(characters);
        var service = new LessonService(repo.Object);

        // Act
        List<LessonDto> result = (await service.GetLessonsAsync(1, 0, 6)).ToList();

        // Assert
        Assert.Equal(6, result.Count);
        Assert.Equal([1, 2, 3, 4, 5, 6], result.Select(l => l.CharacterId));
        Assert.DoesNotContain(7, result.Select(l => l.CharacterId));
    }

    // 5. Page index < 0
    [Fact]
    public async Task GetLessonsAsync_WhenPageIndexLessThanZero_ThrowsArgumentException()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        var service = new LessonService(repo.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetLessonsAsync(1, -1, 10));
        Assert.Equal("pageIndex", ex.ParamName);
    }

    // 6. Page index > elements / page size
    [Fact]
    public async Task GetLessonsAsync_WhenPageIndexGreaterThanElements_ReturnsEmpty()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        List<Character> characters = Enumerable.Range(1, 10).Select(i => new Character { Id = i, Symbol = $"Symbol{i}", Romanization = $"Romanization{i}" }).ToList();

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        repo.Setup(r => r.GetNewCharactersAsync(user.Proficiencies)).ReturnsAsync(characters);
        var service = new LessonService(repo.Object);

        // Act
        List<LessonDto> result = (await service.GetLessonsAsync(1, 2, 10)).ToList();

        // Assert
        Assert.Empty(result);
    }

    // 7. The last page returns fewer elements than page size
    [Fact]
    public async Task GetLessonsAsync_WhenLastPageHasFewerElementsThanPageSize_ReturnsThatManyElements()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        List<Character> characters = Enumerable.Range(1, 26).Select(i => new Character { Id = i, Symbol = $"Symbol{i}", Romanization = $"Romanization{i}" }).ToList();

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        repo.Setup(r => r.GetNewCharactersAsync(user.Proficiencies)).ReturnsAsync(characters);
        var service = new LessonService(repo.Object);

        // Act
        List<LessonDto> result = (await service.GetLessonsAsync(1, 2, 10)).ToList();

        // Assert
        Assert.Equal(6, result.Count);
        Assert.Equal([21, 22, 23, 24, 25, 26], result.Select(l => l.CharacterId));
        Assert.DoesNotContain(1, result.Select(l => l.CharacterId));
        Assert.DoesNotContain(11, result.Select(l => l.CharacterId));
        Assert.DoesNotContain(20, result.Select(l => l.CharacterId));
        Assert.DoesNotContain(27, result.Select(l => l.CharacterId));
    }

    // 8. The middle page returns page size elements
    [Fact]
    public async Task GetLessonsAsync_WhenMiddlePageHasPageSizeElements_ReturnsPageSizeElements()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        List<Character> characters = Enumerable.Range(1, 26).Select(i => new Character { Id = i, Symbol = $"Symbol{i}", Romanization = $"Romanization{i}" }).ToList();

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.CountLessonsCompletedTodayAsync(1)).ReturnsAsync(0);
        repo.Setup(r => r.GetNewCharactersAsync(user.Proficiencies)).ReturnsAsync(characters);
        var service = new LessonService(repo.Object);

        // Act
        List<LessonDto> result = (await service.GetLessonsAsync(1, 1, 10)).ToList();

        // Assert
        Assert.Equal(10, result.Count);
        Assert.Equal([11, 12, 13, 14, 15, 16, 17, 18, 19, 20], result.Select(l => l.CharacterId));
        Assert.DoesNotContain(1, result.Select(l => l.CharacterId));
        Assert.DoesNotContain(10, result.Select(l => l.CharacterId));
        Assert.DoesNotContain(21, result.Select(l => l.CharacterId));
    }
}
