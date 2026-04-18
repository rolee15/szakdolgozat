using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Learning;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.UnitTests.Core.Entities.Users;

public class UserTest
{
    [Fact]
    public void User_Constructor_ShouldInitializeProperties()
    {
        byte[] hashBytes = "passwordHash"u8.ToArray();
        byte[] saltBytes = "passwordSalt"u8.ToArray();
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            PasswordHash = hashBytes,
            PasswordSalt = saltBytes,
            KanaProficiencies = new List<KanaProficiency>(),
            LessonCompletions = new List<LessonCompletion>()
        };

        Assert.Multiple(
            () => Assert.Equal(1, user.Id),
            () => Assert.Equal("testUser", user.Username),
            () => Assert.Equal(hashBytes, user.PasswordHash),
            () => Assert.Equal(saltBytes, user.PasswordSalt),
            () => Assert.NotNull(user.KanaProficiencies),
            () => Assert.Empty(user.KanaProficiencies),
            () => Assert.NotNull(user.LessonCompletions),
            () => Assert.Empty(user.LessonCompletions)
        );
    }
}
