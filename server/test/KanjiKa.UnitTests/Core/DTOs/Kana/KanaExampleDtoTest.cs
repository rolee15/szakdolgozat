using KanjiKa.Application.DTOs.Kana;

namespace KanjiKa.UnitTests.Core.DTOs.Kana;

public class KanaExampleDtoTest
{
    [Fact]
    public void ExampleDto_Constructor_ShouldInitializeProperties()
    {
        var example = new KanaExampleDto
        {
            Word = "こんにちは",
            Romanization = "konnichiwa",
            Meaning = "Hello"
        };

        Assert.Multiple(
            () => Assert.Equal("こんにちは", example.Word),
            () => Assert.Equal("konnichiwa", example.Romanization),
            () => Assert.Equal("Hello", example.Meaning)
        );
    }
}
