using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.Interfaces;

public interface ITokenService
{
    (string accessToken, string refreshToken) GenerateToken(int userId, string username, UserRole role, bool mustChangePassword = false);
}
