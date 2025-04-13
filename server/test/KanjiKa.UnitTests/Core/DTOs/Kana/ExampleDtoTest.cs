using KanjiKa.Core.DTOs.Kana;

namespace KanjiKa.UnitTests.Core.DTOs.Kana;

public class ExampleDtoTest
{
    [Fact]
    public void ExampleDto_Constructor_ShouldInitializeProperties()
    {
        var example = new ExampleDto()
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
