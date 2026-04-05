using KanjiKa.Application.Services;
using KanjiKa.Application.DTOs.Reading;
using KanjiKa.Domain.Entities.Reading;
using KanjiKa.Application.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class ReadingServiceTest
{
    private static ReadingPassage MakePassage(int id = 1) => new()
    {
        Id = id,
        Title = $"Passage {id}",
        Content = "Content text.",
        Source = "Original",
        JlptLevel = 5,
        SortOrder = id
    };

    private static ComprehensionQuestion MakeQuestion(int id, char correct) => new()
    {
        Id = id,
        ReadingPassageId = 1,
        QuestionText = $"Question {id}?",
        OptionA = "A answer",
        OptionB = "B answer",
        OptionC = "C answer",
        OptionD = "D answer",
        CorrectOption = correct,
        SortOrder = id
    };

    // ── GetPassagesAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetPassagesAsync_NoProficiencies_ReturnsDefaultValues()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passages = new List<ReadingPassage> { MakePassage(1), MakePassage(2) };
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(passages);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>());

        var service = new ReadingService(repo.Object);

        // Act
        var result = await service.GetPassagesAsync(1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, dto =>
        {
            Assert.False(dto.IsPassed);
            Assert.Equal(0, dto.Score);
            Assert.Equal(0, dto.AttemptCount);
        });
    }

    [Fact]
    public async Task GetPassagesAsync_WithProficiency_MapsProficiencyValues()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passage = MakePassage(1);
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync([passage]);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>
            {
                [1] = new ReadingProficiency
                {
                    Id = 1, UserId = 1, ReadingPassageId = 1,
                    Score = 80, AttemptCount = 2, IsPassed = true
                }
            });

        var service = new ReadingService(repo.Object);

        // Act
        var result = await service.GetPassagesAsync(1);

        // Assert
        Assert.Single(result);
        var dto = result[0];
        Assert.Multiple(
            () => Assert.True(dto.IsPassed),
            () => Assert.Equal(80, dto.Score),
            () => Assert.Equal(2, dto.AttemptCount)
        );
    }

    // ── GetPassageDetailAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetPassageDetailAsync_PassageNotFound_ReturnsNull()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((ReadingPassage?)null);

        var service = new ReadingService(repo.Object);

        // Act
        var result = await service.GetPassageDetailAsync(99, 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPassageDetailAsync_PassageFound_NoProficiency_ReturnsDetailWithDefaults()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passage = MakePassage(1);
        passage.Questions.Add(MakeQuestion(1, 'A'));
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(passage);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>());

        var service = new ReadingService(repo.Object);

        // Act
        var result = await service.GetPassageDetailAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Multiple(
            () => Assert.Equal("Passage 1", result.Title),
            () => Assert.False(result.IsPassed),
            () => Assert.Equal(0, result.Score),
            () => Assert.Single(result.Questions)
        );
    }

    [Fact]
    public async Task GetPassageDetailAsync_PassageFound_WithProficiency_MapsProficiencyValues()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passage = MakePassage(1);
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(passage);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>
            {
                [1] = new ReadingProficiency
                {
                    Id = 1, UserId = 1, ReadingPassageId = 1,
                    Score = 100, AttemptCount = 1, IsPassed = true
                }
            });

        var service = new ReadingService(repo.Object);

        // Act
        var result = await service.GetPassageDetailAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsPassed);
        Assert.Equal(100, result.Score);
    }

    [Fact]
    public async Task GetPassageDetailAsync_QuestionOptions_HasFourEntries()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passage = MakePassage(1);
        passage.Questions.Add(MakeQuestion(1, 'B'));
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(passage);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>());

        var service = new ReadingService(repo.Object);

        // Act
        var result = await service.GetPassageDetailAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Questions);
        Assert.Equal(4, result.Questions[0].Options.Count);
    }

    // ── SubmitAnswersAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task SubmitAnswersAsync_PassageNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((ReadingPassage?)null);

        var service = new ReadingService(repo.Object);
        var submitDto = new ReadingSubmitDto { PassageId = 99, Answers = [] };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.SubmitAnswersAsync(1, submitDto));
    }

    [Fact]
    public async Task SubmitAnswersAsync_AllCorrect_ScoreIsHundredAndIsPassed()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passage = MakePassage(1);
        passage.Questions.Add(MakeQuestion(1, 'A'));
        passage.Questions.Add(MakeQuestion(2, 'B'));
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(passage);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>());
        repo.Setup(r => r.UpsertProficiencyAsync(It.IsAny<ReadingProficiency>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new ReadingService(repo.Object);
        var submitDto = new ReadingSubmitDto
        {
            PassageId = 1,
            Answers = new Dictionary<int, string> { [1] = "A", [2] = "B" }
        };

        // Act
        var result = await service.SubmitAnswersAsync(1, submitDto);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(100, result.Score),
            () => Assert.True(result.IsPassed),
            () => Assert.Equal(2, result.CorrectCount),
            () => Assert.Equal(2, result.TotalQuestions)
        );
    }

    [Fact]
    public async Task SubmitAnswersAsync_AllIncorrect_ScoreIsZeroAndNotPassed()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passage = MakePassage(1);
        passage.Questions.Add(MakeQuestion(1, 'A'));
        passage.Questions.Add(MakeQuestion(2, 'A'));
        passage.Questions.Add(MakeQuestion(3, 'A'));
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(passage);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>());
        repo.Setup(r => r.UpsertProficiencyAsync(It.IsAny<ReadingProficiency>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new ReadingService(repo.Object);
        var submitDto = new ReadingSubmitDto
        {
            PassageId = 1,
            Answers = new Dictionary<int, string> { [1] = "B", [2] = "B", [3] = "B" }
        };

        // Act
        var result = await service.SubmitAnswersAsync(1, submitDto);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(0, result.Score),
            () => Assert.False(result.IsPassed),
            () => Assert.Equal(0, result.CorrectCount)
        );
    }

    [Fact]
    public async Task SubmitAnswersAsync_ScoreExactly70_IsPassed()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passage = MakePassage(1);
        // 7 out of 10 correct = 70 score
        for (int i = 1; i <= 10; i++)
            passage.Questions.Add(MakeQuestion(i, 'A'));

        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(passage);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>());
        repo.Setup(r => r.UpsertProficiencyAsync(It.IsAny<ReadingProficiency>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new ReadingService(repo.Object);
        var answers = new Dictionary<int, string>();
        for (int i = 1; i <= 7; i++) answers[i] = "A";  // correct
        for (int i = 8; i <= 10; i++) answers[i] = "B"; // incorrect
        var submitDto = new ReadingSubmitDto { PassageId = 1, Answers = answers };

        // Act
        var result = await service.SubmitAnswersAsync(1, submitDto);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(70, result.Score),
            () => Assert.True(result.IsPassed)
        );
    }

    [Fact]
    public async Task SubmitAnswersAsync_ExistingProficiency_IncrementsAttemptCount()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passage = MakePassage(1);
        passage.Questions.Add(MakeQuestion(1, 'A'));
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(passage);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>
            {
                [1] = new ReadingProficiency { Id = 1, UserId = 1, ReadingPassageId = 1, Score = 50, AttemptCount = 3, IsPassed = false }
            });

        ReadingProficiency? captured = null;
        repo.Setup(r => r.UpsertProficiencyAsync(It.IsAny<ReadingProficiency>()))
            .Callback<ReadingProficiency>(p => captured = p)
            .Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new ReadingService(repo.Object);
        var submitDto = new ReadingSubmitDto
        {
            PassageId = 1,
            Answers = new Dictionary<int, string> { [1] = "A" }
        };

        // Act
        await service.SubmitAnswersAsync(1, submitDto);

        // Assert
        Assert.NotNull(captured);
        Assert.Equal(4, captured.AttemptCount);
    }

    [Fact]
    public async Task SubmitAnswersAsync_NoExistingProficiency_AttemptCountIsOne()
    {
        // Arrange
        var repo = new Mock<IReadingRepository>();
        var passage = MakePassage(1);
        passage.Questions.Add(MakeQuestion(1, 'A'));
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(passage);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, ReadingProficiency>());

        ReadingProficiency? captured = null;
        repo.Setup(r => r.UpsertProficiencyAsync(It.IsAny<ReadingProficiency>()))
            .Callback<ReadingProficiency>(p => captured = p)
            .Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new ReadingService(repo.Object);
        var submitDto = new ReadingSubmitDto
        {
            PassageId = 1,
            Answers = new Dictionary<int, string> { [1] = "A" }
        };

        // Act
        await service.SubmitAnswersAsync(1, submitDto);

        // Assert
        Assert.NotNull(captured);
        Assert.Equal(1, captured.AttemptCount);
    }
}
