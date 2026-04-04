using KanjiKa.Core.DTOs.Grammar;

namespace KanjiKa.UnitTests.Core.DTOs.Grammar;

public class GrammarPointDtoTest
{
    [Fact]
    public void GrammarPointDto_DefaultValues_AreCorrect()
    {
        var dto = new GrammarPointDto();

        Assert.Multiple(
            () => Assert.Equal(0, dto.Id),
            () => Assert.Equal(string.Empty, dto.Title),
            () => Assert.Equal(string.Empty, dto.Pattern),
            () => Assert.Equal(0, dto.JlptLevel),
            () => Assert.Equal(0, dto.CorrectCount),
            () => Assert.Equal(0, dto.AttemptCount),
            () => Assert.False(dto.IsCompleted)
        );
    }
}
