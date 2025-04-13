using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.UnitTests.Core.Entities.Kana;

public class CharacterTest
{
    [Fact]
    public void Character_Constructor_ShouldInitializeProperties()
    {
        var character = new Character
        {
            Id = 1,
            Symbol = "あ",
            Romanization = "a",
            Type = KanaType.Hiragana,
        };

        Assert.Multiple(
            () => Assert.Equal(1, character.Id),
            () => Assert.Equal("あ", character.Symbol),
            () => Assert.Equal("a", character.Romanization),
            () => Assert.Equal(KanaType.Hiragana, character.Type),
            () => Assert.NotNull(character.Examples),
            () => Assert.Empty(character.Examples)
        );
    }
}
