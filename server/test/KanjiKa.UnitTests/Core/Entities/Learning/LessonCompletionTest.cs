using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Entities.Users;

namespace KanjiKa.UnitTests.Core.Entities.Learning;

public class LessonCompletionTest
{
    [Fact]
    public void LessonCompletion_Constructor_ShouldInitializeProperties()
    {
        // Arrange
        const int userId = 1;
        var user = new User
        {
            Id = userId,
            Username = "testUser",
            PasswordHash = "passwordHash"u8.ToArray(),
            PasswordSalt = "passwordSalt"u8.ToArray(),
        };
        const int characterId = 1;
        var character = new Character
        {
            Id = characterId,
            Symbol = "あ",
            Romanization = "a",
            Type = KanaType.Hiragana
        };

        // Act
        DateTimeOffset now = DateTimeOffset.UtcNow;
        var lessonCompletion = new LessonCompletion
        {
            CompletionDate = now,
            UserId = 1,
            User = user,
            CharacterId = 1,
            Character = character
        };

        // Assert
        Assert.Multiple(
            () => Assert.Equal(now, lessonCompletion.CompletionDate),
            () => Assert.Equal(characterId, lessonCompletion.UserId),
            () => Assert.Equal(user, lessonCompletion.User),
            () => Assert.Equal(characterId, lessonCompletion.CharacterId),
            () => Assert.Equal(character, lessonCompletion.Character)
        );
    }
}
