using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Entities.Users;

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
            PasswordSalt = "passwordSalt"u8.ToArray(),
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
            Level = 50,
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
            () => Assert.Equal(50, proficiency.Level),
            () => Assert.Equal(now, proficiency.LearnedAt),
            () => Assert.Equal(now, proficiency.LastPracticed)
        );
    }

    [Fact]
    public void Proficiency_Increase_ShouldIncreaseLevel()
    {
        // Arrange
        var proficiency = new Proficiency
        {
            Level = 50,
            LastPracticed = DateTimeOffset.UtcNow
        };

        // Act
        proficiency.Increase(20);

        // Assert
        Assert.Equal(70, proficiency.Level);
    }

    [Fact]
    public void Proficiency_Increase_ShouldNotExceedMaxLevel()
    {
        // Arrange
        var proficiency = new Proficiency
        {
            Level = 90,
            LastPracticed = DateTimeOffset.UtcNow
        };

        // Act
        proficiency.Increase(20);

        // Assert
        Assert.Equal(Proficiency.MaxLevel, proficiency.Level);
    }

    [Fact]
    public void Proficiency_Increase_ShouldUpdateLastPracticed()
    {
        // Arrange
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var proficiency = new Proficiency
        {
            Level = 50,
            LastPracticed = now
        };

        // Act
        proficiency.Increase(20);

        // Assert
        // TODO: Should inject a DateTimeOffsetProvider so the test can be controlled
        Assert.True(proficiency.LastPracticed > now);
    }

    [Fact]
    public void Proficiency_Decrease_ShouldDecreaseLevel()
    {
        // Arrange
        var proficiency = new Proficiency
        {
            Level = 50,
            LastPracticed = DateTimeOffset.UtcNow
        };

        // Act
        proficiency.Decrease(20);

        // Assert
        Assert.Equal(30, proficiency.Level);
    }

    [Fact]
    public void Proficiency_Decrease_ShouldNotGoBelowMinLevel()
    {
        // Arrange
        var proficiency = new Proficiency
        {
            Level = 10,
            LastPracticed = DateTimeOffset.UtcNow
        };

        // Act
        proficiency.Decrease(20);

        // Assert
        Assert.Equal(Proficiency.MinLevel, proficiency.Level);
    }

    [Fact]
    public void Proficiency_Decrease_ShouldUpdateLastPracticed()
    {
        // Arrange
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var proficiency = new Proficiency
        {
            Level = 50,
            LastPracticed = now
        };

        // Act
        proficiency.Decrease(20);

        // Assert
        // TODO: Should inject a DateTimeOffsetProvider so the test can be controlled
        Assert.True(proficiency.LastPracticed > now);
    }
}
