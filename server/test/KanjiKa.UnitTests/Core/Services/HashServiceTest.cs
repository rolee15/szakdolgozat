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


    [Fact]
    public void Verify_ShouldReturnTrue_ForCorrectPasswordHashAndSalt()
    {
        // Arrange
        var service = new HashService();
        const string password = "P@ssw0rd-ä";
        var (hash, salt) = service.Hash(password);

        // Act
        var result = service.Verify(password, hash, salt);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Verify_ShouldReturnFalse_ForWrongPassword()
    {
        // Arrange
        var service = new HashService();
        const string password = "correct password";
        const string wrongPassword = "wrong password";
        var (hash, salt) = service.Hash(password);

        // Act
        var result = service.Verify(wrongPassword, hash, salt);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_ShouldBeDeterministic_ForGivenSaltAndPassword()
    {
        // Arrange
        var service = new HashService();
        const string password = "abc123";
        var (hash, salt) = service.Hash(password);

        // Act
        var first = service.Verify(password, hash, salt);
        var second = service.Verify(password, hash, salt);

        // Assert
        Assert.True(first);
        Assert.True(second);
    }

    [Fact]
    public void Hash_ShouldProduceDifferentHashes_ForSamePasswordWithDifferentSalts()
    {
        // Arrange
        var service = new HashService();
        const string password = "repeat";

        // Act
        var (hash1, salt1) = service.Hash(password);
        var (hash2, salt2) = service.Hash(password);

        // Assert
        Assert.False(hash1.SequenceEqual(hash2));
        Assert.False(salt1.SequenceEqual(salt2));

        // And Verify still works for each pair
        Assert.True(service.Verify(password, hash1, salt1));
        Assert.True(service.Verify(password, hash2, salt2));
    }

    [Fact]
    public void Verify_ShouldThrow_OnNullInputs()
    {
        // Arrange
        var service = new HashService();
        const string password = "pw";
        var (hash, salt) = service.Hash(password);

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => service.Verify(password, null!, salt));
        Assert.Throws<ArgumentNullException>(() => service.Verify(password, hash, null!));
        Assert.Throws<ArgumentNullException>(() => service.Verify(null!, hash, salt));
    }

    [Fact]
    public void Verify_ShouldThrow_OnEmptyInputs()
    {
        // Arrange
        var service = new HashService();
        const string password = "pw";
        var (hash, salt) = service.Hash(password);

        // Act + Assert
        Assert.Multiple(
            () => Assert.Throws<ArgumentException>(() => service.Verify("", hash, salt)),
            () => Assert.Throws<ArgumentException>(() => service.Verify(password, Array.Empty<byte>(), salt)),
            () => Assert.Throws<ArgumentException>(() => service.Verify(password, hash, Array.Empty<byte>()))
        );
    }
}
