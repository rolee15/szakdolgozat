using KanjiKa.Core.DTOs.Learning;

namespace KanjiKa.UnitTests.Core.DTOs.Learning;

public class LessonReviewDtoTest
{
    [Fact]
    public void LessonReviewDto_Constructor_ShouldInitializeProperties()
    {
        var lessonReview = new LessonReviewDto
        {
            Question = "あ"
        };

        Assert.Equal("あ", lessonReview.Question);
    }
}
