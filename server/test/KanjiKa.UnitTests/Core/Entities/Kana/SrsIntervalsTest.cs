using KanjiKa.Domain.Entities.Common;

namespace KanjiKa.UnitTests.Core.Entities.Kana;

public class SrsIntervalsTest
{
    [Fact]
    public void Advance_FromApprentice4_ReturnsGuru1()
    {
        // Arrange
        var stage = SrsStage.Apprentice4;

        // Act
        SrsStage result = SrsIntervals.Advance(stage);

        // Assert
        Assert.Equal(SrsStage.Guru1, result);
    }

    [Fact]
    public void Regress_FromGuru1_ReturnsApprentice3()
    {
        // Arrange
        // Guru1 = 5, regress by 2 → stage 3 = Apprentice3
        var stage = SrsStage.Guru1;

        // Act
        SrsStage result = SrsIntervals.Regress(stage);

        // Assert
        Assert.Equal(SrsStage.Apprentice3, result);
    }

    [Fact]
    public void Regress_FromApprentice1_ReturnsApprentice1()
    {
        // Arrange
        var stage = SrsStage.Apprentice1;

        // Act
        SrsStage result = SrsIntervals.Regress(stage);

        // Assert
        Assert.Equal(SrsStage.Apprentice1, result);
    }

    [Fact]
    public void GetNextReviewDate_ForApprentice1_Returns4Hours()
    {
        // Arrange
        DateTimeOffset before = DateTimeOffset.UtcNow;

        // Act
        DateTimeOffset? result = SrsIntervals.GetNextReviewDate(SrsStage.Apprentice1);

        // Assert
        Assert.NotNull(result);
        DateTimeOffset expected = before.AddHours(4);
        TimeSpan tolerance = TimeSpan.FromMinutes(1);
        Assert.True(result.Value >= expected - tolerance && result.Value <= expected + tolerance);
    }

    [Fact]
    public void GetNextReviewDate_ForBurned_ReturnsNull()
    {
        // Arrange + Act
        DateTimeOffset? result = SrsIntervals.GetNextReviewDate(SrsStage.Burned);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(SrsStage.Locked, "Locked")]
    [InlineData(SrsStage.Apprentice1, "Apprentice 1")]
    [InlineData(SrsStage.Apprentice2, "Apprentice 2")]
    [InlineData(SrsStage.Apprentice3, "Apprentice 3")]
    [InlineData(SrsStage.Apprentice4, "Apprentice 4")]
    [InlineData(SrsStage.Guru1, "Guru 1")]
    [InlineData(SrsStage.Guru2, "Guru 2")]
    [InlineData(SrsStage.Master, "Master")]
    [InlineData(SrsStage.Enlightened, "Enlightened")]
    [InlineData(SrsStage.Burned, "Burned")]
    public void GetStageName_ForEachStage_ReturnsCorrectLabel(SrsStage stage, string expectedLabel)
    {
        // Act
        string result = SrsIntervals.GetStageName(stage);

        // Assert
        Assert.Equal(expectedLabel, result);
    }
}
