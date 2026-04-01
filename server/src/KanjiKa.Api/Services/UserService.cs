using KanjiKa.Core.DTOs.User;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace KanjiKa.Api.Services;

// TODO: implement reset code
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
        var user = await _repo.GetByUsernameAsync(username);
        if (user == null)
        {
            return new LoginDto()
            {
                IsSuccess = false,
                ErrorMessage = "Username or password is incorrect"
            };
        }

        var isVerified = _hashService.Verify(password, user.PasswordHash, user.PasswordSalt);
        if (!isVerified)
        {
            return new LoginDto()
            {
                IsSuccess = false,
                ErrorMessage = "Username or password is incorrect"
            };
        }

        var (token, refreshToken) = _tokenService.GenerateToken(user.Id, user.Username, user.Role, user.MustChangePassword);
        var expiryDays = int.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? "7");
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
        var user = await _repo.GetByUsernameAsync(username);
        if (user != null)
        {
            return new RegisterDto
            {
                IsSuccess = false,
                ErrorMessage = "Username already exists"
            };
        }

        var (passwordHash, passwordSalt) = _hashService.Hash(password);
        var newUser = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _repo.AddAsync(newUser);
        await _repo.SaveChangesAsync();

        var (token, refreshToken) = _tokenService.GenerateToken(newUser.Id, newUser.Username, UserRole.User);
        var expiryDays = int.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? "7");
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
        await _emailService.SendEmail(email, "Forgot Password", "Reset code: 12345");
        return new ForgotPasswordDto();
    }

    public async Task<ResetPasswordDto> ResetPassword(string username, string resetCode, string newPassword)
    {
        var user = await _repo.GetByUsernameAsync(username);
        if (user == null)
        {
            return new ResetPasswordDto
            {
                IsSuccess = false,
                ErrorMessage = "User not found"
            };
        }

        // check reset code
        if (resetCode != "12345")
        {
            return new ResetPasswordDto
            {
                IsSuccess = false,
                ErrorMessage = "Invalid reset code"
            };
        }

        var (passwordHash, passwordSalt) = _hashService.Hash(newPassword);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        await _repo.UpdateAsync(user);
        await _repo.SaveChangesAsync();

        return new ResetPasswordDto
        {
            IsSuccess = true
        };
    }

    public async Task<RefreshTokenDto> RefreshToken(string token, string refreshToken)
    {
        var newToken = "test23456";
        var result = new RefreshTokenDto
        {
            Token = newToken
        };
        return await Task.FromResult(result);
    }

    public async Task<ChangePasswordDto> ChangePassword(int userId, string currentPassword, string newPassword)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null)
        {
            return new ChangePasswordDto
            {
                IsSuccess = false,
                ErrorMessage = "User not found"
            };
        }

        var isVerified = _hashService.Verify(currentPassword, user.PasswordHash, user.PasswordSalt);
        if (!isVerified)
        {
            return new ChangePasswordDto
            {
                IsSuccess = false,
                ErrorMessage = "Current password is incorrect"
            };
        }

        var (passwordHash, passwordSalt) = _hashService.Hash(newPassword);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.MustChangePassword = false;
        await _repo.UpdateAsync(user);
        await _repo.SaveChangesAsync();

        return new ChangePasswordDto { IsSuccess = true };
    }
}
