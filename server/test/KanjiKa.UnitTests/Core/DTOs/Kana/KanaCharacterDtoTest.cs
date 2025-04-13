using KanjiKa.Core.DTOs.Kana;
using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.UnitTests.Core.DTOs.Kana;

public class KanaCharacterDtoTest
{
    [Fact]
    public void KanaCharacterDto_Constructor_ShouldInitializeProperties()
    {
        var character = new KanaCharacterDto
        {
            Character = "あ",
            Romanization = "a",
            Type = KanaType.Hiragana,
            Proficiency = 80
        };

        Assert.Multiple(
            () => Assert.Equal("あ", character.Character),
            () => Assert.Equal("a", character.Romanization),
            () => Assert.Equal(KanaType.Hiragana, character.Type),
            () => Assert.Equal(80, character.Proficiency)
        );
    }
}
