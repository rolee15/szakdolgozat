using KanjiKa.Core.DTOs.User;

namespace KanjiKa.Core.Interfaces;

public interface IUserService
{
    Task<LoginDto> Login(string username, string password);
    Task<RegisterDto> Register(string username, string password);
    Task<ForgotPasswordDto> ForgotPassword(string email);
    Task<ResetPasswordDto> ResetPassword(string username, string resetCode, string newPassword);
    Task<RefreshTokenDto> RefreshToken(string token, string refreshToken);
}