using KanjiKa.Core.Entities.Users;

namespace KanjiKa.UnitTests.Core.Entities.Users;

public class UserTest
{
    [Fact]
    public void User_Constructor_ShouldInitializeProperties()
    {
        var hashBytes = "passwordHash"u8.ToArray();
        var saltBytes = "passwordSalt"u8.ToArray();
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            PasswordHash = hashBytes,
            PasswordSalt = saltBytes
        };

        Assert.Multiple(
            () => Assert.Equal(1, user.Id),
            () => Assert.Equal("testUser", user.Username),
            () => Assert.Equal(hashBytes, user.PasswordHash),
            () => Assert.Equal(saltBytes, user.PasswordSalt),
            () => Assert.NotNull(user.Proficiencies),
            () => Assert.Empty(user.Proficiencies),
            () => Assert.NotNull(user.LessonCompletions),
            () => Assert.Empty(user.LessonCompletions)
        );
    }
}
