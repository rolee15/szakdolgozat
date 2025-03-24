namespace KanjiKa.Core.Interfaces;

public interface IHashService
{
    public (byte[], byte[]) Hash(string password);
    bool Verify(string password, byte[] userPasswordHash, byte[] userPasswordSalt);
}