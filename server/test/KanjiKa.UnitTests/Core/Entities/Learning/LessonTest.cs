using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;

namespace KanjiKa.UnitTests.Core.Entities.Learning;

public class LessonTest
{
    [Fact]
    public void Lesson_Constructor_ShouldInitializeProperties()
    {
        // Arrange
        const int characterId = 1;
        var character = new Character
        {
            Id = characterId,
            Symbol = "あ",
            Romanization = "a",
            Type = KanaType.Hiragana
        };

        // Act
        var lesson = new Lesson
        {
            Id = 1,
            Character = character,
            Answer = "a"
        };

        // Assert
        Assert.Multiple(
            () => Assert.Equal(1, lesson.Id),
            () => Assert.Equal(character, lesson.Character),
            () => Assert.Equal("a", lesson.Answer)
        );
    }
}
