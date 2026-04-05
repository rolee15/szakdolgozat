using KanjiKa.Application.DTOs.Kana;
using KanjiKa.Domain.Entities.Kana;

namespace KanjiKa.UnitTests.Core.DTOs.Kana;

public class KanaCharacterDetailDtoTest
{
    [Fact]
    public void KanaCharacterDetailDto_Constructor_ShouldInitializeProperties()
    {
        var character = new KanaCharacterDetailDto
        {
            Character = "あ",
            Romanization = "a",
            Type = KanaType.Hiragana,
            Proficiency = 80,
            SrsStage = (int)SrsStage.Apprentice1,
            SrsStageName = "Apprentice 1"
        };

        Assert.Multiple(
            () => Assert.Equal("あ", character.Character),
            () => Assert.Equal("a", character.Romanization),
            () => Assert.Equal(KanaType.Hiragana, character.Type),
            () => Assert.Equal(80, character.Proficiency),
            () => Assert.Equal((int)SrsStage.Apprentice1, character.SrsStage),
            () => Assert.Equal("Apprentice 1", character.SrsStageName)
        );
    }
}
