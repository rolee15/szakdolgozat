using KanjiKa.Application.Services;
using KanjiKa.Application.DTOs.Kana;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Users;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class KanaServiceTest
{
    private static User MakeUser(int id = 1) => new()
    {
        Id = id,
        Username = $"user{id}@test.com",
        PasswordHash = [],
        PasswordSalt = []
    };

    private static Character MakeCharacter(int id, string symbol, string romanization, KanaType type) => new()
    {
        Id = id,
        Symbol = symbol,
        Romanization = romanization,
        Type = type
    };

    // ── GetKanaCharacters ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetKanaCharacters_UserNotFound_ThrowsArgumentException()
    {
        var repo = new Mock<IKanaRepository>();
        repo.Setup(r => r.GetUserWithProficienciesAsync(99)).ReturnsAsync((User?)null);
        repo.Setup(r => r.GetCharactersByType(KanaType.Hiragana)).Returns([]);

        var service = new KanaService(repo.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => service.GetKanaCharacters(KanaType.Hiragana, 99));
    }

    [Fact]
    public async Task GetKanaCharacters_UserHasNoProficiencies_ReturnsProficiencyZero()
    {
        var repo = new Mock<IKanaRepository>();
        User user = MakeUser();
        var characters = new List<Character>
        {
            MakeCharacter(1, "あ", "a", KanaType.Hiragana),
            MakeCharacter(2, "い", "i", KanaType.Hiragana)
        };
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.GetCharactersByType(KanaType.Hiragana)).Returns(characters);

        var service = new KanaService(repo.Object);

        IEnumerable<KanaDto> result = await service.GetKanaCharacters(KanaType.Hiragana, 1);

        Assert.All(result, dto => Assert.Equal(0, dto.Proficiency));
    }

    [Fact]
    public async Task GetKanaCharacters_UserHasProficiency_ReturnsMappedLevel()
    {
        var repo = new Mock<IKanaRepository>();
        User user = MakeUser();
        Character character = MakeCharacter(1, "あ", "a", KanaType.Hiragana);
        user.Proficiencies.Add(new Proficiency { CharacterId = 1, SrsStage = SrsStage.Guru1 });

        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.GetCharactersByType(KanaType.Hiragana)).Returns([character]);

        var service = new KanaService(repo.Object);

        IEnumerable<KanaDto> result = await service.GetKanaCharacters(KanaType.Hiragana, 1);

        KanaDto dto = result.Single();
        Assert.Equal(new Proficiency { SrsStage = SrsStage.Guru1 }.Level, dto.Proficiency);
    }

    [Fact]
    public async Task GetKanaCharacters_ReturnsCorrectType()
    {
        var repo = new Mock<IKanaRepository>();
        User user = MakeUser();
        var characters = new List<Character>
        {
            MakeCharacter(1, "ア", "a", KanaType.Katakana)
        };
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);
        repo.Setup(r => r.GetCharactersByType(KanaType.Katakana)).Returns(characters);

        var service = new KanaService(repo.Object);

        IEnumerable<KanaDto> result = await service.GetKanaCharacters(KanaType.Katakana, 1);

        Assert.All(result, dto => Assert.Equal(KanaType.Katakana, dto.Type));
    }

    // ── GetCharacterDetail ────────────────────────────────────────────────────

    [Fact]
    public async Task GetCharacterDetail_CharacterNotFound_ThrowsArgumentException()
    {
        var repo = new Mock<IKanaRepository>();
        repo.Setup(r => r.GetCharacterBySymbolAndTypeAsync("x", KanaType.Hiragana))
            .ReturnsAsync((Character?)null);

        var service = new KanaService(repo.Object);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.GetCharacterDetail("x", KanaType.Hiragana, 1));
    }

    [Fact]
    public async Task GetCharacterDetail_UserNotFound_ThrowsArgumentException()
    {
        var repo = new Mock<IKanaRepository>();
        Character character = MakeCharacter(1, "あ", "a", KanaType.Hiragana);
        repo.Setup(r => r.GetCharacterBySymbolAndTypeAsync("あ", KanaType.Hiragana))
            .ReturnsAsync(character);
        repo.Setup(r => r.GetUserWithProficienciesAsync(99)).ReturnsAsync((User?)null);

        var service = new KanaService(repo.Object);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.GetCharacterDetail("あ", KanaType.Hiragana, 99));
    }

    [Fact]
    public async Task GetCharacterDetail_NoProficiency_ReturnsLockedStageAndZeroLevel()
    {
        var repo = new Mock<IKanaRepository>();
        Character character = MakeCharacter(1, "あ", "a", KanaType.Hiragana);
        User user = MakeUser();
        repo.Setup(r => r.GetCharacterBySymbolAndTypeAsync("あ", KanaType.Hiragana))
            .ReturnsAsync(character);
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);

        var service = new KanaService(repo.Object);

        KanaDetailDto result = await service.GetCharacterDetail("あ", KanaType.Hiragana, 1);

        Assert.Multiple(
            () => Assert.Equal(0, result.Proficiency),
            () => Assert.Equal((int)SrsStage.Locked, result.SrsStage),
            () => Assert.Equal("Locked", result.SrsStageName)
        );
    }

    [Fact]
    public async Task GetCharacterDetail_WithProficiency_ReturnsMappedSrsStage()
    {
        var repo = new Mock<IKanaRepository>();
        Character character = MakeCharacter(1, "あ", "a", KanaType.Hiragana);
        User user = MakeUser();
        user.Proficiencies.Add(new Proficiency { CharacterId = 1, SrsStage = SrsStage.Master });
        repo.Setup(r => r.GetCharacterBySymbolAndTypeAsync("あ", KanaType.Hiragana))
            .ReturnsAsync(character);
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);

        var service = new KanaService(repo.Object);

        KanaDetailDto result = await service.GetCharacterDetail("あ", KanaType.Hiragana, 1);

        Assert.Multiple(
            () => Assert.Equal((int)SrsStage.Master, result.SrsStage),
            () => Assert.Equal("Master", result.SrsStageName)
        );
    }

    [Fact]
    public async Task GetCharacterDetail_ReturnsCorrectSymbolAndRomanization()
    {
        var repo = new Mock<IKanaRepository>();
        Character character = MakeCharacter(1, "あ", "a", KanaType.Hiragana);
        User user = MakeUser();
        repo.Setup(r => r.GetCharacterBySymbolAndTypeAsync("あ", KanaType.Hiragana))
            .ReturnsAsync(character);
        repo.Setup(r => r.GetUserWithProficienciesAsync(1)).ReturnsAsync(user);

        var service = new KanaService(repo.Object);

        KanaDetailDto result = await service.GetCharacterDetail("あ", KanaType.Hiragana, 1);

        Assert.Multiple(
            () => Assert.Equal("あ", result.Character),
            () => Assert.Equal("a", result.Romanization),
            () => Assert.Equal(KanaType.Hiragana, result.Type)
        );
    }

    // ── GetExamples ───────────────────────────────────────────────────────────

    [Fact]
    public async Task GetExamples_CharacterNotFound_ThrowsArgumentException()
    {
        var repo = new Mock<IKanaRepository>();
        repo.Setup(r => r.GetCharacterWithExamplesBySymbolAndTypeAsync("x", KanaType.Hiragana))
            .ReturnsAsync((Character?)null);

        var service = new KanaService(repo.Object);

        await Assert.ThrowsAsync<ArgumentException>(
            () => service.GetExamples("x", KanaType.Hiragana));
    }

    [Fact]
    public async Task GetExamples_CharacterHasExamples_ReturnsMappedDtos()
    {
        var repo = new Mock<IKanaRepository>();
        Character character = MakeCharacter(1, "あ", "a", KanaType.Hiragana);
        character.Examples.Add(new Example { Word = "あめ", Romanization = "ame", Meaning = "rain", Character = character });
        character.Examples.Add(new Example { Word = "あお", Romanization = "ao", Meaning = "blue", Character = character });
        repo.Setup(r => r.GetCharacterWithExamplesBySymbolAndTypeAsync("あ", KanaType.Hiragana))
            .ReturnsAsync(character);

        var service = new KanaService(repo.Object);

        IEnumerable<KanaExampleDto> result = await service.GetExamples("あ", KanaType.Hiragana);

        Assert.Collection(result,
            e => { Assert.Equal("あめ", e.Word); Assert.Equal("ame", e.Romanization); Assert.Equal("rain", e.Meaning); },
            e => { Assert.Equal("あお", e.Word); Assert.Equal("ao", e.Romanization); Assert.Equal("blue", e.Meaning); }
        );
    }

    [Fact]
    public async Task GetExamples_CharacterHasNoExamples_ReturnsEmpty()
    {
        var repo = new Mock<IKanaRepository>();
        Character character = MakeCharacter(1, "あ", "a", KanaType.Hiragana);
        repo.Setup(r => r.GetCharacterWithExamplesBySymbolAndTypeAsync("あ", KanaType.Hiragana))
            .ReturnsAsync(character);

        var service = new KanaService(repo.Object);

        IEnumerable<KanaExampleDto> result = await service.GetExamples("あ", KanaType.Hiragana);

        Assert.Empty(result);
    }
}
