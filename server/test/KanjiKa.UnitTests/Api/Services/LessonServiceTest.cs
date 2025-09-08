using KanjiKa.Api.Services;
using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class LessonServiceTest
{
    [Fact]
    public async Task LearnLessonAsync_Success_CreatesProficiencyAndCompletionAndSaves()
    {
        // Arrange
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        var character = new Character { Id = 10, Symbol = "あ", Romanization = "a" };

        var repo = new Mock<ILessonRepository>(MockBehavior.Strict);
        repo.Setup(r => r.GetUserAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.GetCharacterByIdAsync(10)).ReturnsAsync(character);
        repo.Setup(r => r.GetProficiencyAsync(1, 10)).ReturnsAsync((Proficiency?)null);
        repo.Setup(r => r.AddProficiencyAsync(It.IsAny<Proficiency>())).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.AddLessonCompletionAsync(It.IsAny<LessonCompletion>())).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
        var service = new LessonService(repo.Object);

        // Act
        Proficiency proficiency = await service.LearnLessonAsync(1, 10);

        // Assert
        Assert.Equal(1, proficiency.UserId);
        Assert.Equal(10, proficiency.CharacterId);
        Assert.Equal(0, proficiency.Level);
        repo.Verify();
    }

    [Fact]
    public async Task GetLessonReviewsCountAsync_ReturnsCountFromRepository()
    {
        // Arrange
        const int userId = 1;
        var list = new List<LessonCompletion> { new(), new(), new() };
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetLessonCompletionsByUserAsync(userId)).ReturnsAsync(list);
        var service = new LessonService(repo.Object);

        // Act
        LessonReviewsCountDto result = await service.GetLessonReviewsCountAsync(userId);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetLessonReviewsAsync_ReturnsOrderedByCompletionDate_WithSymbolAsQuestion()
    {
        // Arrange
        const int userId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var list = new List<LessonCompletion>
        {
            new() { CompletionDate = now.AddMinutes(5), Character = new Character { Id = 1, Symbol = "び", Romanization = "bi" } },
            new() { CompletionDate = now.AddMinutes(-2), Character = new Character { Id = 2, Symbol = "あ", Romanization = "a" } },
            new() { CompletionDate = now.AddMinutes(1), Character = new Character { Id = 3, Symbol = "ち", Romanization = "chi" } },
        };

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetLessonCompletionsByUserAsync(userId)).ReturnsAsync(list);
        var service = new LessonService(repo.Object);

        // Act
        List<LessonReviewDto> result = (await service.GetLessonReviewsAsync(userId)).ToList();

        // Assert
        Assert.Equal(["あ", "ち", "び"], result.Select(r => r.Question));
    }

    [Fact]
    public async Task CheckLessonReviewAnswerAsync_Correct_IncreasesProficiencyAndSaves()
    {
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        var character = new Character { Id = 2, Symbol = "ら", Romanization = "ra" };
        var proficiency = new Proficiency { UserId = user.Id, CharacterId = 2, Level = 0, LearnedAt = DateTimeOffset.UtcNow };

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetCharacterBySymbolAsync("ら")).ReturnsAsync(character);
        repo.Setup(r => r.GetProficiencyAsync(1, 2)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
        var service = new LessonService(repo.Object);


        LessonReviewAnswerResultDto result = await service.CheckLessonReviewAnswerAsync(1, new LessonReviewAnswerDto { Question = "ら", Answer = "ra" });

        Assert.True(result.IsCorrect);
        Assert.Equal("ra", result.CorrectAnswer);
        Assert.True(proficiency.Level > 0);
        repo.Verify();
    }

    [Fact]
    public async Task CheckLessonReviewAnswerAsync_Incorrect_DecreasesProficiencyAndSaves()
    {
        // Arrange
        var character = new Character { Id = 2, Symbol = "ら", Romanization = "ra" };
        var proficiency = new Proficiency { UserId = 1, CharacterId = 2, Level = 50, LearnedAt = DateTimeOffset.UtcNow };

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetCharacterBySymbolAsync("ら")).ReturnsAsync(character);
        repo.Setup(r => r.GetProficiencyAsync(1, 2)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
        var service = new LessonService(repo.Object);

        // Act
        LessonReviewAnswerResultDto result = await service.CheckLessonReviewAnswerAsync(1, new LessonReviewAnswerDto { Question = "ら", Answer = "WRONG" });

        // Assert
        Assert.False(result.IsCorrect);
        Assert.Equal("ra", result.CorrectAnswer);
        Assert.True(proficiency.Level < 50);
        repo.Verify();
    }

    [Fact]
    public async Task CheckLessonReviewAnswerAsync_CharacterNotFound_Throws()
    {
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetCharacterBySymbolAsync("x")).ReturnsAsync((Character?)null);
        var service = new LessonService(repo.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CheckLessonReviewAnswerAsync(1, new LessonReviewAnswerDto { Question = "x", Answer = "?" }));
    }
}
