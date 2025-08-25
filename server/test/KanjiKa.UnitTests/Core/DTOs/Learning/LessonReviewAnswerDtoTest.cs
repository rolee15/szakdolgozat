using KanjiKa.Core.DTOs.Learning;

namespace KanjiKa.UnitTests.Core.DTOs.Learning;

public class LessonReviewAnswerDtoTest
{
    [Fact]
    public void LessonReviewAnswerDto_Constructor_ShouldInitializeProperties()
    {
        var lessonReviewAnswer = new LessonReviewAnswerDto
        {
            Question = "あ",
            Answer = "a"
        };

        Assert.Multiple(
            () => Assert.Equal("あ", lessonReviewAnswer.Question),
            () => Assert.Equal("a", lessonReviewAnswer.Answer)
        );
    }
}
