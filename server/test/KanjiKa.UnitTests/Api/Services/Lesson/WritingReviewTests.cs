using KanjiKa.Application.Services;
using KanjiKa.Application.DTOs.Learning;
using KanjiKa.Domain.Entities.Common;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Learning;
using KanjiKa.Domain.Entities.Users;
using KanjiKa.Application.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services.Lesson;

public class WritingReviewTests
{
    [Fact]
    public async Task GetWritingReviewsCountAsync_ReturnsDueCount()
    {
        // Arrange
        const int userId = 1;
        var dueReviews = new List<KanaProficiency>
        {
            new() { UserId = userId, CharacterId = 1 },
            new() { UserId = userId, CharacterId = 2 },
            new() { UserId = userId, CharacterId = 3 }
        };
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetDueReviewsAsync(userId)).ReturnsAsync(dueReviews);
        var service = new LessonService(repo.Object);

        // Act
        LessonReviewsCountDto result = await service.GetWritingReviewsCountAsync(userId);

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetWritingReviewsCountAsync_WhenNoDue_ReturnsZero()
    {
        // Arrange
        const int userId = 1;
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetDueReviewsAsync(userId)).ReturnsAsync(new List<KanaProficiency>());
        var service = new LessonService(repo.Object);

        // Act
        LessonReviewsCountDto result = await service.GetWritingReviewsCountAsync(userId);

        // Assert
        Assert.Equal(0, result.Count);
    }

    [Fact]
    public async Task GetWritingReviewsAsync_MapsProficienciesToDto()
    {
        // Arrange
        const int userId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var dueReviews = new List<KanaProficiency>
        {
            new()
            {
                UserId = userId, CharacterId = 1, NextReviewDate = now.AddMinutes(-5),
                Character = new Character { Id = 1, Symbol = "あ", Romanization = "a", Type = KanaType.Hiragana }
            },
            new()
            {
                UserId = userId, CharacterId = 2, NextReviewDate = now.AddMinutes(-1),
                Character = new Character { Id = 2, Symbol = "ア", Romanization = "a", Type = KanaType.Katakana }
            }
        };
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetDueReviewsAsync(userId)).ReturnsAsync(dueReviews);
        var service = new LessonService(repo.Object);

        // Act
        List<WritingReviewDto> result = (await service.GetWritingReviewsAsync(userId)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("a", result[0].Romanization);
        Assert.Equal("hiragana", result[0].CharacterType);
        Assert.Equal(1, result[0].CharacterId);
        Assert.Equal("a", result[1].Romanization);
        Assert.Equal("katakana", result[1].CharacterType);
        Assert.Equal(2, result[1].CharacterId);
    }

    [Fact]
    public async Task GetWritingReviewsAsync_WhenNoDue_ReturnsEmptyList()
    {
        // Arrange
        const int userId = 1;
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetDueReviewsAsync(userId)).ReturnsAsync(new List<KanaProficiency>());
        var service = new LessonService(repo.Object);

        // Act
        List<WritingReviewDto> result = (await service.GetWritingReviewsAsync(userId)).ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task CheckWritingReviewAnswerAsync_CorrectAnswer_ReturnsIsCorrectTrue()
    {
        // Arrange
        var character = new Character { Id = 5, Symbol = "あ", Romanization = "a" };
        var proficiency = new KanaProficiency { UserId = 1, CharacterId = 5, LearnedAt = DateTimeOffset.UtcNow };

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetCharacterByIdAsync(5)).ReturnsAsync(character);
        repo.Setup(r => r.GetProficiencyAsync(1, 5)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
        var service = new LessonService(repo.Object);

        // Act
        LessonReviewAnswerResultDto result = await service.CheckWritingReviewAnswerAsync(
            1, new WritingReviewAnswerDto { CharacterId = 5, TypedCharacter = "あ" });

        // Assert
        Assert.True(result.IsCorrect);
        Assert.Equal("あ", result.CorrectAnswer);
        repo.Verify();
    }

    [Fact]
    public async Task CheckWritingReviewAnswerAsync_IncorrectAnswer_ReturnsIsCorrectFalse()
    {
        // Arrange
        var character = new Character { Id = 5, Symbol = "あ", Romanization = "a" };
        var proficiency = new KanaProficiency { UserId = 1, CharacterId = 5, SrsStage = SrsStage.Guru1, LearnedAt = DateTimeOffset.UtcNow };

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetCharacterByIdAsync(5)).ReturnsAsync(character);
        repo.Setup(r => r.GetProficiencyAsync(1, 5)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
        var service = new LessonService(repo.Object);

        // Act
        LessonReviewAnswerResultDto result = await service.CheckWritingReviewAnswerAsync(
            1, new WritingReviewAnswerDto { CharacterId = 5, TypedCharacter = "い" });

        // Assert
        Assert.False(result.IsCorrect);
        Assert.Equal("あ", result.CorrectAnswer);
        repo.Verify();
    }

    [Fact]
    public async Task CheckWritingReviewAnswerAsync_CharacterNotFound_ThrowsException()
    {
        // Arrange
        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetCharacterByIdAsync(99)).ReturnsAsync((Character?)null);
        var service = new LessonService(repo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CheckWritingReviewAnswerAsync(1, new WritingReviewAnswerDto { CharacterId = 99, TypedCharacter = "あ" }));
    }

    [Fact]
    public async Task CheckWritingReviewAnswerAsync_ProficiencyNotFound_ThrowsArgumentException()
    {
        // Arrange
        var character = new Character { Id = 5, Symbol = "あ", Romanization = "a" };

        var repo = new Mock<ILessonRepository>();
        repo.Setup(r => r.GetCharacterByIdAsync(5)).ReturnsAsync(character);
        repo.Setup(r => r.GetProficiencyAsync(1, 5)).ReturnsAsync((KanaProficiency?)null);
        var service = new LessonService(repo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CheckWritingReviewAnswerAsync(1, new WritingReviewAnswerDto { CharacterId = 5, TypedCharacter = "あ" }));
    }
}
