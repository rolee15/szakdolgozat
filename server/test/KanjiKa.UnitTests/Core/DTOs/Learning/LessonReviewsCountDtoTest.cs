using KanjiKa.Core.DTOs.Learning;

namespace KanjiKa.UnitTests.Core.DTOs.Learning;

public class LessonReviewsCountDtoTest
{
    [Fact]
    public void LessonReviewsCountDto_Constructor_ShouldInitializeProperties()
    {
        var dto = new LessonReviewsCountDto
        {
            Count = 10
        };

        Assert.Equal(10, dto.Count);
    }
}
