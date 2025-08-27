using System.Security.Cryptography;
using KanjiKa.Core.Interfaces;

namespace KanjiKa.Core.Services;

public class HashService : IHashService
{
    private const int SaltByteSize = 16;
    private const int HashByteSize = 32;
    private const int Pbkdf2Iterations = 100_000;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    public (byte[], byte[]) Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltByteSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Pbkdf2Iterations, HashAlgorithm, HashByteSize);
        return (hash, salt);
    }

    public bool Verify(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
    {
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));
        ArgumentNullException.ThrowIfNull(userPasswordHash);
        ArgumentNullException.ThrowIfNull(userPasswordSalt);

        if (userPasswordHash.Length == 0)
            throw new ArgumentException("Password hash is empty", nameof(userPasswordHash));
        if (userPasswordSalt.Length == 0)
            throw new ArgumentException("Password salt is empty", nameof(userPasswordSalt));

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, userPasswordSalt, Pbkdf2Iterations, HashAlgorithm, HashByteSize);

        // use CryptographicOperations.FixedTimeEquals instead of inputHash.SequenceEqual(userPasswordHash)
        // so that the comparison is not vulnerable to timing attacks
        return CryptographicOperations.FixedTimeEquals(inputHash, userPasswordHash);
    }
}
