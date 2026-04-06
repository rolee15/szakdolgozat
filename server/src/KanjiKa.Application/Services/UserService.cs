using KanjiKa.Application.DTOs.User;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Users;
using Microsoft.Extensions.Configuration;

namespace KanjiKa.Application.Services;

public class UserService : IUserService
{
    private readonly IEmailService _emailService;
    private readonly IHashService _hashService;
    private readonly IUserRepository _repo;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public UserService(IUserRepository repo, IHashService hashService, ITokenService tokenService, IEmailService emailService, IConfiguration config)
    {
        _repo = repo;
        _hashService = hashService;
        _tokenService = tokenService;
        _emailService = emailService;
        _config = config;
    }

    public async Task<LoginDto> Login(string username, string password)
    {
        User? user = await _repo.GetByUsernameAsync(username);
        if (user == null)
        {
            return new LoginDto
            {
                IsSuccess = false,
                ErrorMessage = "Username or password is incorrect"
            };
        }

        bool isVerified = _hashService.Verify(password, user.PasswordHash, user.PasswordSalt);
        if (!isVerified)
        {
            return new LoginDto
            {
                IsSuccess = false,
                ErrorMessage = "Username or password is incorrect"
            };
        }

        (string token, string refreshToken) = _tokenService.GenerateToken(user.Id, user.Username, user.Role, user.MustChangePassword);
        int expiryDays = int.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? "7");
        await _repo.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTimeOffset.UtcNow.AddDays(expiryDays));

        return new LoginDto
        {
            IsSuccess = true,
            Token = token,
            RefreshToken = refreshToken,
            UserId = user.Id,
            MustChangePassword = user.MustChangePassword
        };
    }

    public async Task<RegisterDto> Register(string username, string password)
    {
        User? user = await _repo.GetByUsernameAsync(username);
        if (user != null)
        {
            return new RegisterDto
            {
                IsSuccess = false,
                ErrorMessage = "Username already exists"
            };
        }

        (byte[] passwordHash, byte[] passwordSalt) = _hashService.Hash(password);
        var newUser = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _repo.AddAsync(newUser);
        await _repo.SaveChangesAsync();

        (string token, string refreshToken) = _tokenService.GenerateToken(newUser.Id, newUser.Username, UserRole.User);
        int expiryDays = int.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? "7");
        await _repo.UpdateRefreshTokenAsync(newUser.Id, refreshToken, DateTimeOffset.UtcNow.AddDays(expiryDays));

        return new RegisterDto
        {
            IsSuccess = true,
            Token = token,
            RefreshToken = refreshToken,
            UserId = newUser.Id
        };
    }

    public async Task<ForgotPasswordDto> ForgotPassword(string email)
    {
        User? user = await _repo.GetByUsernameAsync(email);
        if (user != null)
        {
            var code = Random.Shared.Next(100000, 1000000).ToString();
            user.PasswordResetCode = code;
            user.PasswordResetExpiry = DateTimeOffset.UtcNow.AddMinutes(15);
            await _repo.UpdateAsync(user);
            await _repo.SaveChangesAsync();
            await _emailService.SendEmail(email, "Forgot Password", $"Reset code: {code}");
        }

        return new ForgotPasswordDto();
    }

    public async Task<ResetPasswordDto> ResetPassword(string username, string resetCode, string newPassword)
    {
        User? user = await _repo.GetByUsernameAsync(username);
        if (user == null)
        {
            return new ResetPasswordDto
            {
                IsSuccess = false,
                ErrorMessage = "User not found"
            };
        }

        if (user.PasswordResetCode == null
            || !string.Equals(user.PasswordResetCode, resetCode, StringComparison.OrdinalIgnoreCase)
            || user.PasswordResetExpiry == null
            || user.PasswordResetExpiry < DateTimeOffset.UtcNow)
        {
            return new ResetPasswordDto
            {
                IsSuccess = false,
                ErrorMessage = "Invalid reset code"
            };
        }

        (byte[] passwordHash, byte[] passwordSalt) = _hashService.Hash(newPassword);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.PasswordResetCode = null;
        user.PasswordResetExpiry = null;
        await _repo.UpdateAsync(user);
        await _repo.SaveChangesAsync();

        return new ResetPasswordDto
        {
            IsSuccess = true
        };
    }

    public async Task<RefreshTokenDto> RefreshToken(string token, string refreshToken)
    {
        User? user = await _repo.GetByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpiry <= DateTimeOffset.UtcNow)
        {
            return new RefreshTokenDto { Token = string.Empty };
        }

        (string newToken, string newRefreshToken) = _tokenService.GenerateToken(user.Id, user.Username, user.Role, user.MustChangePassword);
        int expiryDays = int.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? "7");
        await _repo.UpdateRefreshTokenAsync(user.Id, newRefreshToken, DateTimeOffset.UtcNow.AddDays(expiryDays));

        return new RefreshTokenDto { Token = newToken };
    }

    public async Task<ChangePasswordDto> ChangePassword(int userId, string currentPassword, string newPassword)
    {
        User? user = await _repo.GetByIdAsync(userId);
        if (user == null)
        {
            return new ChangePasswordDto
            {
                IsSuccess = false,
                ErrorMessage = "User not found"
            };
        }

        bool isVerified = _hashService.Verify(currentPassword, user.PasswordHash, user.PasswordSalt);
        if (!isVerified)
        {
            return new ChangePasswordDto
            {
                IsSuccess = false,
                ErrorMessage = "Current password is incorrect"
            };
        }

        (byte[] passwordHash, byte[] passwordSalt) = _hashService.Hash(newPassword);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.MustChangePassword = false;
        await _repo.UpdateAsync(user);
        await _repo.SaveChangesAsync();

        return new ChangePasswordDto { IsSuccess = true };
    }
}
