using KanjiKa.Core.Services;

namespace KanjiKa.UnitTests.Core.Services;

public class HashServiceTest
{
    [Fact]
    public void HashService_GenerateHash_ShouldReturnHash()
    {
        // Arrange
        var hashService = new HashService();
        const string password = "testPassword";

        // Act
        (byte[] hash, byte[] salt) = hashService.Hash(password);

        // Assert
        // I have no idea how to test this
        Assert.NotNull(hash);
        Assert.NotNull(salt);
    }
}
