using KanjiKa.Application.Services;
using KanjiKa.Application.DTOs;
using KanjiKa.Application.DTOs.Admin;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Learning;
using KanjiKa.Domain.Entities.Users;
using KanjiKa.Application.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class AdminServiceTest
{
    [Fact]
    public async Task GetUsers_ReturnsMappedPagedResult()
    {
        // Arrange
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Username = "alice",
                PasswordHash = [1],
                PasswordSalt = [2],
                Role = UserRole.User,
                MustChangePassword = false,
                Proficiencies = new List<Proficiency> { new Proficiency { CharacterId = 10 } },
                LessonCompletions = new List<LessonCompletion>
                {
                    new LessonCompletion
                    {
                        CharacterId = 10,
                        Character = new Character { Id = 10, Symbol = "あ", Romanization = "a" },
                        User = null!
                    }
                }
            },
            new User
            {
                Id = 2,
                Username = "bob",
                PasswordHash = [3],
                PasswordSalt = [4],
                Role = UserRole.Admin,
                MustChangePassword = true,
                Proficiencies = [],
                LessonCompletions = []
            }
        };
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetUsersPagedAsync(1, 10, null)).ReturnsAsync((users, 2));

        var service = new AdminService(repo.Object);

        // Act
        PagedResult<AdminUserDto> result = await service.GetUsersAsync(1, 10, null);

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(2, result.Items.Count);

        var first = result.Items[0];
        Assert.Equal(1, first.Id);
        Assert.Equal("alice", first.Username);
        Assert.Equal(UserRole.User, first.Role);
        Assert.False(first.MustChangePassword);
        Assert.Equal(1, first.ProficiencyCount);
        Assert.Equal(1, first.LessonCompletionCount);

        var second = result.Items[1];
        Assert.Equal(2, second.Id);
        Assert.Equal("bob", second.Username);
        Assert.Equal(UserRole.Admin, second.Role);
        Assert.True(second.MustChangePassword);
        Assert.Equal(0, second.ProficiencyCount);
        Assert.Equal(0, second.LessonCompletionCount);
    }

    [Fact]
    public async Task GetUsers_EmptyResult_ReturnsEmptyPagedResult()
    {
        // Arrange
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetUsersPagedAsync(1, 10, "nomatch")).ReturnsAsync((new List<User>(), 0));

        var service = new AdminService(repo.Object);

        // Act
        PagedResult<AdminUserDto> result = await service.GetUsersAsync(1, 10, "nomatch");

        // Assert
        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetUserById_UserNotFound_ReturnsNull()
    {
        // Arrange
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetByIdWithStatsAsync(99)).ReturnsAsync((User?)null);

        var service = new AdminService(repo.Object);

        // Act
        AdminUserDetailDto? result = await service.GetUserByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserById_Found_ReturnsMappedDetail()
    {
        // Arrange
        var learnedAt = DateTimeOffset.UtcNow.AddDays(-5);
        var lastPracticed = DateTimeOffset.UtcNow.AddDays(-1);
        var completionDate = DateTimeOffset.UtcNow.AddDays(-2);
        var character = new Character { Id = 7, Symbol = "い", Romanization = "i" };

        var user = new User
        {
            Id = 3,
            Username = "charlie",
            PasswordHash = [5],
            PasswordSalt = [6],
            Role = UserRole.User,
            MustChangePassword = false,
            Proficiencies = new List<Proficiency>
            {
                new Proficiency
                {
                    CharacterId = 7,
                    Character = character,
                    LearnedAt = learnedAt,
                    LastPracticed = lastPracticed
                }
            },
            LessonCompletions = new List<LessonCompletion>
            {
                new LessonCompletion
                {
                    CharacterId = 7,
                    Character = character,
                    CompletionDate = completionDate,
                    User = null!
                }
            }
        };

        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.GetByIdWithStatsAsync(3)).ReturnsAsync(user);

        var service = new AdminService(repo.Object);

        // Act
        AdminUserDetailDto? result = await service.GetUserByIdAsync(3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Id);
        Assert.Equal("charlie", result.Username);
        Assert.Equal(UserRole.User, result.Role);
        Assert.False(result.MustChangePassword);
        Assert.Equal(1, result.ProficiencyCount);
        Assert.Equal(1, result.LessonCompletionCount);

        var prof = result.Proficiencies[0];
        Assert.Equal(7, prof.CharacterId);
        Assert.Equal("い", prof.CharacterSymbol);
        Assert.Equal(learnedAt, prof.LearnedAt);
        Assert.Equal(lastPracticed, prof.LastPracticed);

        var lc = result.LessonCompletions[0];
        Assert.Equal(7, lc.CharacterId);
        Assert.Equal("い", lc.CharacterSymbol);
        Assert.Equal(completionDate, lc.CompletionDate);
    }

    [Fact]
    public async Task DeleteUser_SelfDeletion_ReturnsFalse()
    {
        // Arrange
        var repo = new Mock<IUserRepository>();

        var service = new AdminService(repo.Object);

        // Act
        bool result = await service.DeleteUserAsync(5, 5);

        // Assert
        Assert.False(result);
        repo.Verify(r => r.DeleteByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUser_OtherUser_CallsRepoAndReturnsTrue()
    {
        // Arrange
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.DeleteByIdAsync(10)).ReturnsAsync(true).Verifiable();

        var service = new AdminService(repo.Object);

        // Act
        bool result = await service.DeleteUserAsync(1, 10);

        // Assert
        Assert.True(result);
        repo.Verify(r => r.DeleteByIdAsync(10), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_UserNotFound_ReturnsFalse()
    {
        // Arrange
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.DeleteByIdAsync(42)).ReturnsAsync(false);

        var service = new AdminService(repo.Object);

        // Act
        bool result = await service.DeleteUserAsync(1, 42);

        // Assert
        Assert.False(result);
    }
}
