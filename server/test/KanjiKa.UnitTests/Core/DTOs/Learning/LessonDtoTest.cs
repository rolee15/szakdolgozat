using KanjiKa.Core.DTOs.Learning;
using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.UnitTests.Core.DTOs.Learning;

public class LessonDtoTest
{
    [Fact]
    public void LessonDto_Constructor_ShouldInitializeProperties()
    {
        // Arrange
        const int characterId = 1;
        const string symbol = "あ";
        const string romanization = "a";
        const KanaType type = KanaType.Hiragana;
        var character = new Character
        {
            Id = characterId,
            Symbol = symbol,
            Romanization = romanization,
            Type = type
        };
        var examples = new List<Example>
        {
            new Example { Id = 1, CharacterId = character.Id, Character = character, Word = "あお", Romanization = "ao", Meaning = "blue" },
            new Example { Id = 2, CharacterId = character.Id, Character = character, Word = "あか", Romanization = "aka", Meaning = "red" }
        };

        // Act
        var lessonDto = new LessonDto
        {
            CharacterId = characterId,
            Symbol = symbol,
            Romanization = romanization,
            Type = type,
            Examples = examples
        };

        // Assert
        Assert.Multiple(
            () => Assert.Equal(characterId, lessonDto.CharacterId),
            () => Assert.Equal(symbol, lessonDto.Symbol),
            () => Assert.Equal(romanization, lessonDto.Romanization),
            () => Assert.Equal(type, lessonDto.Type),
            () => Assert.Equal(examples.Count, lessonDto.Examples.Count),
            () => Assert.Equal(examples[0], lessonDto.Examples[0]),
            () => Assert.Equal(examples[1], lessonDto.Examples[1])
        );
    }
}
