using KanjiKa.Api.Services;
using KanjiKa.Core.DTOs.Path;
using KanjiKa.Core.Entities.Path;
using KanjiKa.Core.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class PathServiceTest
{
    private static LearningUnit MakeUnit(int id, string title = "Unit") => new()
    {
        Id = id,
        Title = title,
        Description = $"Description {id}",
        SortOrder = id
    };

    private static UnitTest MakeTest(int id, char correct) => new()
    {
        Id = id,
        LearningUnitId = 1,
        QuestionText = $"Question {id}?",
        OptionA = "A",
        OptionB = "B",
        OptionC = "C",
        OptionD = "D",
        CorrectOption = correct,
        ContentType = ContentType.Kana,
        SortOrder = id
    };

    // ── GetPathAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetPathAsync_FirstUnit_IsAlwaysUnlocked()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var units = new List<LearningUnit> { MakeUnit(1) };
        repo.Setup(r => r.GetAllUnitsAsync()).ReturnsAsync(units);
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>());

        var service = new PathService(repo.Object);

        // Act
        var result = await service.GetPathAsync(1);

        // Assert
        Assert.Single(result);
        Assert.True(result[0].IsUnlocked);
    }

    [Fact]
    public async Task GetPathAsync_SecondUnit_LockedWhenPreviousUnitNotPassed()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var units = new List<LearningUnit> { MakeUnit(1), MakeUnit(2) };
        repo.Setup(r => r.GetAllUnitsAsync()).ReturnsAsync(units);
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>
            {
                [1] = new UnitProgress { Id = 1, UserId = 1, LearningUnitId = 1, IsPassed = false }
            });

        var service = new PathService(repo.Object);

        // Act
        var result = await service.GetPathAsync(1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.False(result[1].IsUnlocked);
    }

    [Fact]
    public async Task GetPathAsync_SecondUnit_UnlockedWhenPreviousUnitPassed()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var units = new List<LearningUnit> { MakeUnit(1), MakeUnit(2) };
        repo.Setup(r => r.GetAllUnitsAsync()).ReturnsAsync(units);
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>
            {
                [1] = new UnitProgress { Id = 1, UserId = 1, LearningUnitId = 1, IsPassed = true }
            });

        var service = new PathService(repo.Object);

        // Act
        var result = await service.GetPathAsync(1);

        // Assert
        Assert.True(result[1].IsUnlocked);
    }

    [Fact]
    public async Task GetPathAsync_SecondUnit_LockedWhenNoPreviousProgress()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var units = new List<LearningUnit> { MakeUnit(1), MakeUnit(2) };
        repo.Setup(r => r.GetAllUnitsAsync()).ReturnsAsync(units);
        // No progress at all
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>());

        var service = new PathService(repo.Object);

        // Act
        var result = await service.GetPathAsync(1);

        // Assert
        Assert.True(result[0].IsUnlocked);  // first always unlocked
        Assert.False(result[1].IsUnlocked); // second locked without prev progress
    }

    [Fact]
    public async Task GetPathAsync_MapsProgressValues()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var units = new List<LearningUnit> { MakeUnit(1) };
        repo.Setup(r => r.GetAllUnitsAsync()).ReturnsAsync(units);
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>
            {
                [1] = new UnitProgress { Id = 1, UserId = 1, LearningUnitId = 1, IsPassed = true, BestScore = 90 }
            });

        var service = new PathService(repo.Object);

        // Act
        var result = await service.GetPathAsync(1);

        // Assert
        Assert.Single(result);
        Assert.Multiple(
            () => Assert.True(result[0].IsPassed),
            () => Assert.Equal(90, result[0].BestScore)
        );
    }

    // ── GetUnitDetailAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GetUnitDetailAsync_UnitNotFound_ReturnsNull()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        repo.Setup(r => r.GetUnitByIdAsync(99)).ReturnsAsync((LearningUnit?)null);

        var service = new PathService(repo.Object);

        // Act
        var result = await service.GetUnitDetailAsync(99, 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUnitDetailAsync_UnitFound_ReturnsDetail()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var unit = MakeUnit(1, "Hiragana Vowels");
        unit.Contents.Add(new UnitContent { Id = 1, LearningUnitId = 1, ContentType = ContentType.Kana, ContentId = 1, SortOrder = 1 });

        repo.Setup(r => r.GetUnitByIdAsync(1)).ReturnsAsync(unit);
        repo.Setup(r => r.GetUnitWithTestAsync(1)).ReturnsAsync(new LearningUnit
        {
            Id = 1, Title = "Hiragana Vowels", Description = "Desc", SortOrder = 1,
            Tests = [MakeTest(1, 'A'), MakeTest(2, 'B')]
        });
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>());
        var allUnits = new List<LearningUnit> { unit };
        repo.Setup(r => r.GetAllUnitsAsync()).ReturnsAsync(allUnits);
        repo.Setup(r => r.GetContentTitleAsync(ContentType.Kana, 1))
            .ReturnsAsync("あ (a)");

        var service = new PathService(repo.Object);

        // Act
        var result = await service.GetUnitDetailAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Multiple(
            () => Assert.Equal("Hiragana Vowels", result.Title),
            () => Assert.Equal(1, result.ContentCount),
            () => Assert.Equal(2, result.TestQuestionCount)
        );
    }

    // ── GetUnitTestAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetUnitTestAsync_UnitNotFound_ReturnsNull()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        repo.Setup(r => r.GetUnitWithTestAsync(99)).ReturnsAsync((LearningUnit?)null);

        var service = new PathService(repo.Object);

        // Act
        var result = await service.GetUnitTestAsync(99, 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUnitTestAsync_UnitFound_ReturnsShuffledOptions()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var unit = MakeUnit(1);
        unit.Tests.Add(MakeTest(1, 'A'));
        unit.Tests.Add(MakeTest(2, 'B'));
        repo.Setup(r => r.GetUnitWithTestAsync(1)).ReturnsAsync(unit);

        var service = new PathService(repo.Object);

        // Act
        var result = await service.GetUnitTestAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Questions.Count);
        Assert.All(result.Questions, q => Assert.Equal(4, q.Options.Count));
    }

    // ── SubmitTestAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task SubmitTestAsync_UnitNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        repo.Setup(r => r.GetUnitWithTestAsync(99)).ReturnsAsync((LearningUnit?)null);

        var service = new PathService(repo.Object);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.SubmitTestAsync(1, 99, new UnitSubmitDto()));
    }

    [Fact]
    public async Task SubmitTestAsync_AllCorrect_ScoreIsHundredAndIsPassed()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var unit = MakeUnit(1);
        unit.Tests.Add(MakeTest(1, 'A'));
        unit.Tests.Add(MakeTest(2, 'B'));
        repo.Setup(r => r.GetUnitWithTestAsync(1)).ReturnsAsync(unit);
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>());
        repo.Setup(r => r.UpsertProgressAsync(It.IsAny<UnitProgress>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new PathService(repo.Object);
        var submitDto = new UnitSubmitDto
        {
            Answers = new Dictionary<int, string> { [1] = "A", [2] = "B" }
        };

        // Act
        var result = await service.SubmitTestAsync(1, 1, submitDto);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(100, result.Score),
            () => Assert.True(result.IsPassed),
            () => Assert.Equal(2, result.CorrectCount),
            () => Assert.Equal(2, result.TotalQuestions)
        );
    }

    [Fact]
    public async Task SubmitTestAsync_AllIncorrect_ScoreIsZeroAndNotPassed()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var unit = MakeUnit(1);
        unit.Tests.Add(MakeTest(1, 'A'));
        unit.Tests.Add(MakeTest(2, 'A'));
        repo.Setup(r => r.GetUnitWithTestAsync(1)).ReturnsAsync(unit);
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>());
        repo.Setup(r => r.UpsertProgressAsync(It.IsAny<UnitProgress>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new PathService(repo.Object);
        var submitDto = new UnitSubmitDto
        {
            Answers = new Dictionary<int, string> { [1] = "B", [2] = "B" }
        };

        // Act
        var result = await service.SubmitTestAsync(1, 1, submitDto);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(0, result.Score),
            () => Assert.False(result.IsPassed),
            () => Assert.Equal(0, result.CorrectCount)
        );
    }

    [Fact]
    public async Task SubmitTestAsync_ExistingProgress_IncrementsAttemptCount()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var unit = MakeUnit(1);
        unit.Tests.Add(MakeTest(1, 'A'));
        repo.Setup(r => r.GetUnitWithTestAsync(1)).ReturnsAsync(unit);
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>
            {
                [1] = new UnitProgress { Id = 1, UserId = 1, LearningUnitId = 1, BestScore = 60, AttemptCount = 2, IsPassed = false }
            });

        UnitProgress? captured = null;
        repo.Setup(r => r.UpsertProgressAsync(It.IsAny<UnitProgress>()))
            .Callback<UnitProgress>(p => captured = p)
            .Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new PathService(repo.Object);
        var submitDto = new UnitSubmitDto { Answers = new Dictionary<int, string> { [1] = "A" } };

        // Act
        await service.SubmitTestAsync(1, 1, submitDto);

        // Assert
        Assert.NotNull(captured);
        Assert.Equal(3, captured.AttemptCount);
    }

    [Fact]
    public async Task SubmitTestAsync_NoExistingProgress_AttemptCountIsOne()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var unit = MakeUnit(1);
        unit.Tests.Add(MakeTest(1, 'A'));
        repo.Setup(r => r.GetUnitWithTestAsync(1)).ReturnsAsync(unit);
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>());

        UnitProgress? captured = null;
        repo.Setup(r => r.UpsertProgressAsync(It.IsAny<UnitProgress>()))
            .Callback<UnitProgress>(p => captured = p)
            .Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new PathService(repo.Object);
        var submitDto = new UnitSubmitDto { Answers = new Dictionary<int, string> { [1] = "A" } };

        // Act
        await service.SubmitTestAsync(1, 1, submitDto);

        // Assert
        Assert.NotNull(captured);
        Assert.Equal(1, captured.AttemptCount);
    }

    [Fact]
    public async Task SubmitTestAsync_NewScoreHigher_BestScoreIsUpdated()
    {
        // Arrange
        var repo = new Mock<IPathRepository>();
        var unit = MakeUnit(1);
        unit.Tests.Add(MakeTest(1, 'A'));
        repo.Setup(r => r.GetUnitWithTestAsync(1)).ReturnsAsync(unit);
        repo.Setup(r => r.GetProgressForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, UnitProgress>
            {
                [1] = new UnitProgress { Id = 1, UserId = 1, LearningUnitId = 1, BestScore = 50, AttemptCount = 1, IsPassed = false }
            });

        UnitProgress? captured = null;
        repo.Setup(r => r.UpsertProgressAsync(It.IsAny<UnitProgress>()))
            .Callback<UnitProgress>(p => captured = p)
            .Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new PathService(repo.Object);
        // 1 correct out of 1 = 100
        var submitDto = new UnitSubmitDto { Answers = new Dictionary<int, string> { [1] = "A" } };

        // Act
        var result = await service.SubmitTestAsync(1, 1, submitDto);

        // Assert — the UpsertProgressAsync receives the new score; the repository handles best-score comparison
        Assert.NotNull(captured);
        Assert.Equal(100, captured.BestScore);
    }
}
