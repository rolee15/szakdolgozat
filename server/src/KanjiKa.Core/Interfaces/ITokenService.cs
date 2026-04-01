using KanjiKa.Core.Entities.Users;

namespace KanjiKa.Core.Interfaces;

public interface ITokenService
{
    (string accessToken, string refreshToken) GenerateToken(int userId, string username, UserRole role, bool mustChangePassword = false);
}