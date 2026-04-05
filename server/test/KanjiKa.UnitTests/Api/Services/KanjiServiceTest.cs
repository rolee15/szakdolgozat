using KanjiKa.Application.Services;
using KanjiKa.Application.DTOs.Kanji;
using KanjiKa.Domain.Entities.Kanji;
using KanjiKa.Application.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class KanjiServiceTest
{
    [Fact]
    public async Task GetKanjiByLevelAsync_ReturnsKanjiFromRepository()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        var kanjis = new List<Kanji>
        {
            new() { Id = 1, Character = "日", Meaning = "sun", OnyomiReading = "ニチ", KunyomiReading = "ひ", JlptLevel = 5, StrokeCount = 4 },
            new() { Id = 2, Character = "月", Meaning = "moon", OnyomiReading = "ゲツ", KunyomiReading = "つき", JlptLevel = 5, StrokeCount = 4 }
        };
        repo.Setup(r => r.GetByJlptLevelAsync(5)).ReturnsAsync(kanjis);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, KanjiProficiency>());

        var service = new KanjiService(repo.Object);

        // Act
        List<KanjiDto> result = await service.GetKanjiByLevelAsync(5, 1);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetKanjiByLevelAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        repo.Setup(r => r.GetByJlptLevelAsync(5)).ReturnsAsync(new List<Kanji>());
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, KanjiProficiency>());

        var service = new KanjiService(repo.Object);

        // Act
        List<KanjiDto> result = await service.GetKanjiByLevelAsync(5, 1);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetKanjiDetailAsync_ReturnsDetailWithExamples()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        var kanji = new Kanji
        {
            Id = 1,
            Character = "日",
            Meaning = "sun",
            OnyomiReading = "ニチ",
            KunyomiReading = "ひ",
            StrokeCount = 4,
            JlptLevel = 5,
            Grade = 1,
            Examples =
            [
                new KanjiExample { Id = 1, Word = "日本", Reading = "にほん", Meaning = "Japan", KanjiId = 1 },
                new KanjiExample { Id = 2, Word = "日曜日", Reading = "にちようび", Meaning = "Sunday", KanjiId = 1 }
            ]
        };
        repo.Setup(r => r.GetByCharacterAsync("日")).ReturnsAsync(kanji);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, KanjiProficiency>());

        var service = new KanjiService(repo.Object);

        // Act
        KanjiDetailDto? result = await service.GetKanjiDetailAsync("日", 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Examples.Count);
    }

    [Fact]
    public async Task GetKanjiDetailAsync_CharacterNotFound_ReturnsNull()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        repo.Setup(r => r.GetByCharacterAsync("X")).ReturnsAsync((Kanji?)null);

        var service = new KanjiService(repo.Object);

        // Act
        KanjiDetailDto? result = await service.GetKanjiDetailAsync("X", 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetKanjiDetailAsync_MapsAllFields()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        var kanji = new Kanji
        {
            Id = 3,
            Character = "水",
            Meaning = "water",
            OnyomiReading = "スイ",
            KunyomiReading = "みず",
            StrokeCount = 4,
            JlptLevel = 5,
            Grade = 1
        };
        repo.Setup(r => r.GetByCharacterAsync("水")).ReturnsAsync(kanji);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, KanjiProficiency>());

        var service = new KanjiService(repo.Object);

        // Act
        KanjiDetailDto? result = await service.GetKanjiDetailAsync("水", 1);

        // Assert
        Assert.NotNull(result);
        Assert.Multiple(
            () => Assert.Equal("水", result.Character),
            () => Assert.Equal("water", result.Meaning),
            () => Assert.Equal("スイ", result.OnyomiReading),
            () => Assert.Equal("みず", result.KunyomiReading),
            () => Assert.Equal(4, result.StrokeCount),
            () => Assert.Equal(5, result.JlptLevel),
            () => Assert.Equal(1, result.Grade)
        );
    }

    [Fact]
    public async Task GetDueReviewsCountAsync_ReturnsDueCount()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        repo.Setup(r => r.GetDueReviewsAsync(1)).ReturnsAsync(
        [
            new KanjiProficiency { UserId = 1, KanjiId = 1, SrsStage = KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice1 },
            new KanjiProficiency { UserId = 1, KanjiId = 2, SrsStage = KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice2 },
            new KanjiProficiency { UserId = 1, KanjiId = 3, SrsStage = KanjiKa.Domain.Entities.Kana.SrsStage.Guru1 }
        ]);
        var service = new KanjiService(repo.Object);

        // Act
        int count = await service.GetDueReviewsCountAsync(1);

        // Assert
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task GetDueReviewsCountAsync_NoDueReviews_ReturnsZero()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        repo.Setup(r => r.GetDueReviewsAsync(1)).ReturnsAsync([]);
        var service = new KanjiService(repo.Object);

        // Act
        int count = await service.GetDueReviewsCountAsync(1);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task GetDueReviewsAsync_ReturnsMappedDtos()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        repo.Setup(r => r.GetDueReviewsAsync(1)).ReturnsAsync(
        [
            new KanjiProficiency { UserId = 1, KanjiId = 10, SrsStage = KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice1,
                Kanji = new Kanji { Id = 10, Character = "日", Meaning = "sun", OnyomiReading = "ニチ", KunyomiReading = "ひ", JlptLevel = 5, StrokeCount = 4 } },
            new KanjiProficiency { UserId = 1, KanjiId = 20, SrsStage = KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice2,
                Kanji = new Kanji { Id = 20, Character = "月", Meaning = "moon", OnyomiReading = "ゲツ", KunyomiReading = "つき", JlptLevel = 5, StrokeCount = 4 } }
        ]);
        var service = new KanjiService(repo.Object);

        // Act
        List<KanjiReviewDto> result = await service.GetDueReviewsAsync(1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Multiple(
            () => Assert.Equal(10, result[0].KanjiId),
            () => Assert.Equal("日", result[0].Character),
            () => Assert.Equal("sun", result[0].Meaning),
            () => Assert.Equal(20, result[1].KanjiId),
            () => Assert.Equal("月", result[1].Character),
            () => Assert.Equal("moon", result[1].Meaning)
        );
    }

    [Fact]
    public async Task LearnKanjiAsync_NewKanji_CreatesProficiency()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        repo.Setup(r => r.GetProficiencyAsync(1, 5)).ReturnsAsync((KanjiProficiency?)null);
        repo.Setup(r => r.AddProficiencyAsync(It.IsAny<KanjiProficiency>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        var service = new KanjiService(repo.Object);

        // Act
        KanjiProficiency result = await service.LearnKanjiAsync(1, 5);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice1, result.SrsStage),
            () => Assert.NotNull(result.NextReviewDate)
        );
        repo.Verify(r => r.AddProficiencyAsync(It.IsAny<KanjiProficiency>()), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LearnKanjiAsync_AlreadyLearned_ThrowsInvalidOperationException()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        repo.Setup(r => r.GetProficiencyAsync(1, 5))
            .ReturnsAsync(new KanjiProficiency { UserId = 1, KanjiId = 5, SrsStage = KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice1 });
        var service = new KanjiService(repo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.LearnKanjiAsync(1, 5));
    }

    [Fact]
    public async Task CheckReviewAsync_CorrectAnswer_AdvancesSrsStage()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        var proficiency = new KanjiProficiency { UserId = 1, KanjiId = 7, SrsStage = KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice1 };
        repo.Setup(r => r.GetProficiencyAsync(1, 7)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        var service = new KanjiService(repo.Object);
        var answer = new KanjiReviewAnswerDto { KanjiId = 7, IsCorrect = true };

        // Act
        KanjiReviewResultDto result = await service.CheckReviewAsync(1, answer);

        // Assert
        Assert.Multiple(
            () => Assert.True(result.IsCorrect),
            () => Assert.Equal((int)KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice2, result.SrsStage)
        );
    }

    [Fact]
    public async Task CheckReviewAsync_IncorrectAnswer_RegressesSrsStage()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        var proficiency = new KanjiProficiency { UserId = 1, KanjiId = 7, SrsStage = KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice3 };
        repo.Setup(r => r.GetProficiencyAsync(1, 7)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        var service = new KanjiService(repo.Object);
        var answer = new KanjiReviewAnswerDto { KanjiId = 7, IsCorrect = false };

        // Act
        KanjiReviewResultDto result = await service.CheckReviewAsync(1, answer);

        // Assert
        Assert.Multiple(
            () => Assert.False(result.IsCorrect),
            () => Assert.Equal((int)KanjiKa.Domain.Entities.Kana.SrsStage.Apprentice1, result.SrsStage)
        );
    }

    [Fact]
    public async Task CheckReviewAsync_ProficiencyNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var repo = new Mock<IKanjiRepository>();
        repo.Setup(r => r.GetProficiencyAsync(1, 99)).ReturnsAsync((KanjiProficiency?)null);
        var service = new KanjiService(repo.Object);
        var answer = new KanjiReviewAnswerDto { KanjiId = 99, IsCorrect = true };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.CheckReviewAsync(1, answer));
    }
}
