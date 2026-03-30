namespace KanjiKa.Core.Interfaces;

public interface ITokenService
{
    (string accessToken, string refreshToken) GenerateToken(int userId, string username);
}