using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Learning;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.UnitTests.Core.Entities.Kana;

public class ProficiencyTest
{
    [Fact]
    public void ProficiencyTest_Constructor_ShouldInitializeProperties()
    {
        // Arrange
        const int userId = 1;
        const int characterId = 1;
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var character = new Character
        {
            Id = characterId,
            Symbol = "あ",
            Romanization = "a",
            Type = KanaType.Hiragana
        };
        var user = new User
        {
            Id = userId,
            Username = "testUser",
            PasswordHash = "passwordHash"u8.ToArray(),
            PasswordSalt = "passwordSalt"u8.ToArray()
        };
        var lessonCompletion = new LessonCompletion
        {
            CompletionDate = now,
            UserId = userId,
            User = user,
            CharacterId = characterId,
            Character = character
        };
        user.LessonCompletions.Add(lessonCompletion);

        // Act
        var proficiency = new Proficiency
        {
            Id = 1,
            UserId = userId,
            User = user,
            CharacterId = characterId,
            Character = character,
            SrsStage = SrsStage.Guru1,
            LearnedAt = now,
            LastPracticed = now
        };

        // Assert
        Assert.Multiple(
            () => Assert.Equal(1, proficiency.Id),
            () => Assert.Equal(userId, proficiency.UserId),
            () => Assert.Equal(user, proficiency.User),
            () => Assert.Equal(characterId, proficiency.CharacterId),
            () => Assert.Equal(character, proficiency.Character),
            () => Assert.Equal(SrsStage.Guru1, proficiency.SrsStage),
            () => Assert.Equal(now, proficiency.LearnedAt),
            () => Assert.Equal(now, proficiency.LastPracticed)
        );
    }

    [Fact]
    public void AnswerCorrectly_AdvancesStage()
    {
        // Arrange
        var proficiency = new Proficiency { SrsStage = SrsStage.Apprentice1 };

        // Act
        proficiency.AnswerCorrectly();

        // Assert
        Assert.Equal(SrsStage.Apprentice2, proficiency.SrsStage);
    }

    [Fact]
    public void AnswerCorrectly_FromBurned_StaysAtBurned()
    {
        // Arrange
        var proficiency = new Proficiency { SrsStage = SrsStage.Burned };

        // Act
        proficiency.AnswerCorrectly();

        // Assert
        Assert.Equal(SrsStage.Burned, proficiency.SrsStage);
    }

    [Fact]
    public void AnswerCorrectly_SetsNextReviewDate()
    {
        // Arrange
        var proficiency = new Proficiency { SrsStage = SrsStage.Apprentice1 };

        // Act
        proficiency.AnswerCorrectly();

        // Assert
        Assert.NotNull(proficiency.NextReviewDate);
    }

    [Fact]
    public void AnswerIncorrectly_RegressesByTwo()
    {
        // Arrange
        // Guru1 = 5, regress by 2 → stage 3 = Apprentice3
        var proficiency = new Proficiency { SrsStage = SrsStage.Guru1 };

        // Act
        proficiency.AnswerIncorrectly();

        // Assert
        Assert.Equal(SrsStage.Apprentice3, proficiency.SrsStage);
    }

    [Fact]
    public void AnswerIncorrectly_CannotGoBelowApprentice1()
    {
        // Arrange
        var proficiency = new Proficiency { SrsStage = SrsStage.Apprentice1 };

        // Act
        proficiency.AnswerIncorrectly();

        // Assert
        Assert.Equal(SrsStage.Apprentice1, proficiency.SrsStage);
    }

    [Fact]
    public void AnswerIncorrectly_SetsNextReviewDate()
    {
        // Arrange
        var proficiency = new Proficiency { SrsStage = SrsStage.Guru2 };

        // Act
        proficiency.AnswerIncorrectly();

        // Assert
        Assert.NotNull(proficiency.NextReviewDate);
    }

    [Fact]
    public void Level_MapsCorrectly_ForApprentice1()
    {
        // Arrange
        var proficiency = new Proficiency { SrsStage = SrsStage.Apprentice1 };

        // Act
        int level = proficiency.Level;

        // Assert
        // SrsStage.Apprentice1 = 1, Level = 1 * 100 / 9 = 11
        Assert.Equal(11, level);
    }

    [Fact]
    public void Level_Is100_WhenBurned()
    {
        // Arrange
        var proficiency = new Proficiency { SrsStage = SrsStage.Burned };

        // Act
        int level = proficiency.Level;

        // Assert
        Assert.Equal(100, level);
    }

    [Fact]
    public void Proficiency_Level_ShouldLockedReturnZero()
    {
        // Arrange
        var proficiency = new Proficiency { SrsStage = SrsStage.Locked };

        // Act + Assert
        Assert.Equal(0, proficiency.Level);
    }

    [Fact]
    public void Proficiency_AnswerCorrectly_ShouldUpdateLastPracticed()
    {
        // Arrange
        var fixedNow = new DateTimeOffset(2025, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var proficiency = new Proficiency
        {
            SrsStage = SrsStage.Apprentice2,
            LastPracticed = fixedNow.AddHours(-1)
        };

        // Act
        proficiency.AnswerCorrectly(fixedNow);

        // Assert
        Assert.Equal(fixedNow, proficiency.LastPracticed);
    }

    [Fact]
    public void Proficiency_AnswerIncorrectly_ShouldUpdateLastPracticed()
    {
        // Arrange
        var fixedNow = new DateTimeOffset(2025, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var proficiency = new Proficiency
        {
            SrsStage = SrsStage.Guru2,
            LastPracticed = fixedNow.AddHours(-1)
        };

        // Act
        proficiency.AnswerIncorrectly(fixedNow);

        // Assert
        Assert.Equal(fixedNow, proficiency.LastPracticed);
    }
}
