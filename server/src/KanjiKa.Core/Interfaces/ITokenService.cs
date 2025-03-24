namespace KanjiKa.Core.Interfaces;

public interface ITokenService
{
    public (string, string) GenerateToken(int userId);
}