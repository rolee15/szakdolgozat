using KanjiKa.Api.Services;
using KanjiKa.Core.DTOs.Grammar;
using KanjiKa.Core.Entities.Grammar;
using KanjiKa.Core.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class GrammarServiceTest
{
    [Fact]
    public async Task GetGrammarPointsAsync_ReturnsAllPoints()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        var points = new List<GrammarPoint>
        {
            new() { Id = 1, Title = "は (wa)", Pattern = "Noun + は", Explanation = "Topic marker." },
            new() { Id = 2, Title = "が (ga)", Pattern = "Noun + が", Explanation = "Subject marker." }
        };
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(points);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, GrammarProficiency>());

        var service = new GrammarService(repo.Object);

        // Act
        List<GrammarPointDto> result = await service.GetGrammarPointsAsync(1);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetGrammarPointsAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<GrammarPoint>());
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, GrammarProficiency>());

        var service = new GrammarService(repo.Object);

        // Act
        List<GrammarPointDto> result = await service.GetGrammarPointsAsync(1);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetGrammarPointsAsync_WithCompletedProficiency_SetsIsCompletedTrue()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        var points = new List<GrammarPoint>
        {
            new() { Id = 1, Title = "は (wa)", Pattern = "Noun + は", Explanation = "Topic marker." }
        };
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(points);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, GrammarProficiency>
            {
                [1] = new GrammarProficiency { Id = 1, UserId = 1, GrammarPointId = 1, CorrectCount = 3, AttemptCount = 3 }
            });

        var service = new GrammarService(repo.Object);

        // Act
        List<GrammarPointDto> result = await service.GetGrammarPointsAsync(1);

        // Assert
        Assert.True(result[0].IsCompleted);
    }

    [Fact]
    public async Task GetGrammarPointsAsync_WithPartialProficiency_SetsIsCompletedFalse()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        var points = new List<GrammarPoint>
        {
            new() { Id = 1, Title = "は (wa)", Pattern = "Noun + は", Explanation = "Topic marker." }
        };
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(points);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, GrammarProficiency>
            {
                [1] = new GrammarProficiency { Id = 1, UserId = 1, GrammarPointId = 1, CorrectCount = 2, AttemptCount = 3 }
            });

        var service = new GrammarService(repo.Object);

        // Act
        List<GrammarPointDto> result = await service.GetGrammarPointsAsync(1);

        // Assert
        Assert.False(result[0].IsCompleted);
    }

    [Fact]
    public async Task GetGrammarPointDetailAsync_ReturnsDetail()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        var point = new GrammarPoint
        {
            Id = 1,
            Title = "は (wa)",
            Pattern = "Noun + は + Predicate",
            Explanation = "The particle は marks the topic.",
            JlptLevel = 5
        };
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(point);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, GrammarProficiency>());

        var service = new GrammarService(repo.Object);

        // Act
        GrammarPointDetailDto? result = await service.GetGrammarPointDetailAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Multiple(
            () => Assert.Equal("は (wa)", result.Title),
            () => Assert.Equal("Noun + は + Predicate", result.Pattern),
            () => Assert.Equal("The particle は marks the topic.", result.Explanation)
        );
    }

    [Fact]
    public async Task GetGrammarPointDetailAsync_NotFound_ReturnsNull()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((GrammarPoint?)null);

        var service = new GrammarService(repo.Object);

        // Act
        GrammarPointDetailDto? result = await service.GetGrammarPointDetailAsync(99, 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetGrammarPointDetailAsync_MapsExamplesCorrectly()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        var point = new GrammarPoint
        {
            Id = 1,
            Title = "は (wa)",
            Pattern = "Noun + は",
            Explanation = "Topic marker.",
            Examples =
            [
                new GrammarExample { Id = 1, Japanese = "これはペンです。", Reading = "Kore wa pen desu.", English = "This is a pen.", GrammarPointId = 1 },
                new GrammarExample { Id = 2, Japanese = "わたしは学生です。", Reading = "Watashi wa gakusei desu.", English = "I am a student.", GrammarPointId = 1 }
            ]
        };
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(point);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, GrammarProficiency>());

        var service = new GrammarService(repo.Object);

        // Act
        GrammarPointDetailDto? result = await service.GetGrammarPointDetailAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Examples.Count);
        Assert.Multiple(
            () => Assert.Equal("これはペンです。", result.Examples[0].Japanese),
            () => Assert.Equal("Kore wa pen desu.", result.Examples[0].Reading),
            () => Assert.Equal("This is a pen.", result.Examples[0].English)
        );
    }

    [Fact]
    public async Task GetGrammarPointDetailAsync_ExerciseOptionsHasFourItems()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        var point = new GrammarPoint
        {
            Id = 1,
            Title = "は (wa)",
            Pattern = "Noun + は",
            Explanation = "Topic marker.",
            Exercises =
            [
                new GrammarExercise
                {
                    Id = 1,
                    Sentence = "これ___ペンです。",
                    CorrectAnswer = "は",
                    Distractor1 = "が",
                    Distractor2 = "を",
                    Distractor3 = "に",
                    GrammarPointId = 1
                }
            ]
        };
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(point);
        repo.Setup(r => r.GetProficienciesForUserAsync(1, It.IsAny<List<int>>()))
            .ReturnsAsync(new Dictionary<int, GrammarProficiency>());

        var service = new GrammarService(repo.Object);

        // Act
        GrammarPointDetailDto? result = await service.GetGrammarPointDetailAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Exercises);
        Assert.Equal(4, result.Exercises[0].Options.Count);
    }

    [Fact]
    public async Task CheckExerciseAsync_CorrectAnswer_IncrementsCorrectAndAttemptCount()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        var exercise = new GrammarExercise
        {
            Id = 1,
            Sentence = "これ___ペンです。",
            CorrectAnswer = "は",
            Distractor1 = "が",
            Distractor2 = "を",
            Distractor3 = "に",
            GrammarPointId = 1
        };
        var proficiency = new GrammarProficiency { Id = 1, UserId = 1, GrammarPointId = 1, CorrectCount = 1, AttemptCount = 2 };
        repo.Setup(r => r.GetExerciseByIdAsync(1)).ReturnsAsync(exercise);
        repo.Setup(r => r.GetProficiencyAsync(1, 1)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new GrammarService(repo.Object);
        var answer = new GrammarExerciseAnswerDto { ExerciseId = 1, Answer = "は" };

        // Act
        GrammarExerciseResultDto result = await service.CheckExerciseAsync(1, 1, answer);

        // Assert
        Assert.Multiple(
            () => Assert.True(result.IsCorrect),
            () => Assert.Equal(2, result.CorrectCount),
            () => Assert.Equal(3, result.AttemptCount)
        );
    }

    [Fact]
    public async Task CheckExerciseAsync_IncorrectAnswer_IncrementsOnlyAttemptCount()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        var exercise = new GrammarExercise
        {
            Id = 1,
            Sentence = "これ___ペンです。",
            CorrectAnswer = "は",
            Distractor1 = "が",
            Distractor2 = "を",
            Distractor3 = "に",
            GrammarPointId = 1
        };
        var proficiency = new GrammarProficiency { Id = 1, UserId = 1, GrammarPointId = 1, CorrectCount = 1, AttemptCount = 2 };
        repo.Setup(r => r.GetExerciseByIdAsync(1)).ReturnsAsync(exercise);
        repo.Setup(r => r.GetProficiencyAsync(1, 1)).ReturnsAsync(proficiency);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new GrammarService(repo.Object);
        var answer = new GrammarExerciseAnswerDto { ExerciseId = 1, Answer = "が" };

        // Act
        GrammarExerciseResultDto result = await service.CheckExerciseAsync(1, 1, answer);

        // Assert
        Assert.Multiple(
            () => Assert.False(result.IsCorrect),
            () => Assert.Equal(1, result.CorrectCount),
            () => Assert.Equal(3, result.AttemptCount)
        );
    }

    [Fact]
    public async Task CheckExerciseAsync_ExerciseNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        repo.Setup(r => r.GetExerciseByIdAsync(99)).ReturnsAsync((GrammarExercise?)null);

        var service = new GrammarService(repo.Object);
        var answer = new GrammarExerciseAnswerDto { ExerciseId = 99, Answer = "は" };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.CheckExerciseAsync(1, 1, answer));
    }

    [Fact]
    public async Task CheckExerciseAsync_NewProficiency_CreatesRecord()
    {
        // Arrange
        var repo = new Mock<IGrammarRepository>();
        var exercise = new GrammarExercise
        {
            Id = 1,
            Sentence = "これ___ペンです。",
            CorrectAnswer = "は",
            Distractor1 = "が",
            Distractor2 = "を",
            Distractor3 = "に",
            GrammarPointId = 1
        };
        repo.Setup(r => r.GetExerciseByIdAsync(1)).ReturnsAsync(exercise);
        repo.Setup(r => r.GetProficiencyAsync(1, 1)).ReturnsAsync((GrammarProficiency?)null);
        repo.Setup(r => r.AddProficiencyAsync(It.IsAny<GrammarProficiency>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new GrammarService(repo.Object);
        var answer = new GrammarExerciseAnswerDto { ExerciseId = 1, Answer = "は" };

        // Act
        await service.CheckExerciseAsync(1, 1, answer);

        // Assert
        repo.Verify(r => r.AddProficiencyAsync(It.IsAny<GrammarProficiency>()), Times.Once);
    }
}
