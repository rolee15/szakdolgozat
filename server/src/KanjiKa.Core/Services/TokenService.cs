using KanjiKa.Core.Interfaces;

namespace KanjiKa.Api.Services;

public class TokenService : ITokenService
{
    public (string, string) GenerateToken(int userId)
    {
        return ("test12345", "refreshToken12345");
    }
}