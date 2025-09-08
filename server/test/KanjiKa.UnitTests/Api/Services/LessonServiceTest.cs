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
        var user = new User { Id = 1, Username = "user", PasswordHash = [0], PasswordSalt = [0] };
        var character = new Character { Id = 10, Symbol = "あ", Romanization = "a" };

        var repo = new Mock<ILessonRepository>(MockBehavior.Strict);
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.GetCharacterByIdAsync(10)).ReturnsAsync(character);
        repo.Setup(r => r.GetProficiencyAsync(1, 10)).ReturnsAsync((Proficiency?)null);
        repo.Setup(r => r.AddProficiencyAsync(It.IsAny<Proficiency>())).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.AddLessonCompletionAsync(It.IsAny<LessonCompletion>())).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

        var service = new LessonService(repo.Object);
        var prof = await service.LearnLessonAsync(1, 10);

        Assert.Equal(1, prof.UserId);
        Assert.Equal(10, prof.CharacterId);
        Assert.Equal(0, prof.Level);
        repo.Verify();
    }

    [Fact]
    public async Task GetLessonReviewsCountAsync_ReturnsCountFromRepository()
    {
        var userId = 1;
        var list = new List<LessonCompletion> { new(), new(), new() };
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetLessonCompletionsByUserAsync(userId)).ReturnsAsync(list);
        var service = new LessonService(repo.Object);

        var result = await service.GetLessonReviewsCountAsync(userId);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetLessonReviewsAsync_ReturnsOrderedByCompletionDate_WithSymbolAsQuestion()
    {
        var userId = 1;
        var now = DateTimeOffset.UtcNow;
        var list = new List<LessonCompletion>
        {
            new() { CompletionDate = now.AddMinutes(5), Character = new Character { Id=1, Symbol = "B", Romanization="b" } },
            new() { CompletionDate = now.AddMinutes(-2), Character = new Character { Id=2, Symbol = "A", Romanization="a" } },
            new() { CompletionDate = now.AddMinutes(1), Character = new Character { Id=3, Symbol = "C", Romanization="c" } },
        };
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetLessonCompletionsByUserAsync(userId)).ReturnsAsync(list);
        var service = new LessonService(repo.Object);

        var result = (await service.GetLessonReviewsAsync(userId)).ToList();
        Assert.Equal(new[] { "A", "C", "B" }, result.Select(r => r.Question));
    }

    [Fact]
    public async Task CheckLessonReviewAnswerAsync_Correct_IncreasesProficiencyAndSaves()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [0], PasswordSalt = [0] };
        var character = new Character { Id = 2, Symbol = "a", Romanization = "ra" };
        var proficiency = new Proficiency { UserId = 1, CharacterId = 2, Level = 0, LearnedAt = DateTimeOffset.UtcNow };

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetCharacterBySymbolAsync("a")).ReturnsAsync(character);
        repo.Setup(r => r.GetProficiencyAsync(1, 2)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

        var service = new LessonService(repo.Object);
        var res = await service.CheckLessonReviewAnswerAsync(1, new LessonReviewAnswerDto { Question = "a", Answer = "ra" });

        Assert.True(res.IsCorrect);
        Assert.Equal("ra", res.CorrectAnswer);
        Assert.True(proficiency.Level > 0);
        repo.Verify();
    }

    [Fact]
    public async Task CheckLessonReviewAnswerAsync_Incorrect_DecreasesProficiencyAndSaves()
    {
        var character = new Character { Id = 2, Symbol = "a", Romanization = "ra" };
        var proficiency = new Proficiency { UserId = 1, CharacterId = 2, Level = 50, LearnedAt = DateTimeOffset.UtcNow };

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetCharacterBySymbolAsync("a")).ReturnsAsync(character);
        repo.Setup(r => r.GetProficiencyAsync(1, 2)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

        var service = new LessonService(repo.Object);
        var res = await service.CheckLessonReviewAnswerAsync(1, new LessonReviewAnswerDto { Question = "a", Answer = "WRONG" });

        Assert.False(res.IsCorrect);
        Assert.Equal("ra", res.CorrectAnswer);
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
