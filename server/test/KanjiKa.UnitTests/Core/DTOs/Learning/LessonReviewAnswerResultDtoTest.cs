using KanjiKa.Core.DTOs.Learning;

namespace KanjiKa.UnitTests.Core.DTOs.Learning;

public class LessonReviewAnswerResultDtoTest
{
    [Fact]
    public void LessonReviewAnswerResultDto_Constructor_ShouldInitializeProperties()
    {
        var lessonReviewAnswerResult = new LessonReviewAnswerResultDto
        {
            IsCorrect = true,
            CorrectAnswer = "correct answer"
        };

        Assert.True(lessonReviewAnswerResult.IsCorrect);
        Assert.Equal("correct answer", lessonReviewAnswerResult.CorrectAnswer);
    }
}
