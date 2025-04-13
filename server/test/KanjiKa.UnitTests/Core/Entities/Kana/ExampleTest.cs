using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.UnitTests.Core.Entities.Kana;

public class ExampleTest
{
    [Fact]
    public void Example_Constructor_ShouldInitializeProperties()
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

        // Act
        var example = new Example
        {
            Id = 1,
            CharacterId = characterId,
            Character = character,
            Word = "こんにちは",
            Romanization = "konnichiwa",
            Meaning = "Hello"
        };

        Assert.Multiple(
            () => Assert.Equal(1, example.Id),
            () => Assert.Equal(characterId, example.CharacterId),
            () => Assert.Equal(character, example.Character),
            () => Assert.Equal("こんにちは", example.Word),
            () => Assert.Equal("konnichiwa", example.Romanization),
            () => Assert.Equal("Hello", example.Meaning)
        );
    }
}
