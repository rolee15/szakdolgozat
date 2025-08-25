using KanjiKa.Core.DTOs.Learning;

namespace KanjiKa.UnitTests.Core.DTOs.Learning;

public class LessonsCountDtoTest
{
    [Fact]
    public void LessonsCountDto_Constructor_ShouldInitializeProperties()
    {
        var lessonsCountDto = new LessonsCountDto
        {
            Count = 10
        };

        Assert.Equal(10, lessonsCountDto.Count);
    }
}
