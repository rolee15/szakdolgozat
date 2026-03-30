using KanjiKa.Api.Services;
using KanjiKa.Core.DTOs.Kanji;
using KanjiKa.Core.Entities.Kanji;
using KanjiKa.Core.Interfaces;
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
}
