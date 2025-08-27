using KanjiKa.Core.DTOs.User;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;

namespace KanjiKa.Api.Services;

// TODO: implement reset code
// TODO: implement token generation for new and refresh tokens
public class UserService : IUserService
{
    private readonly IEmailService _emailService;
    private readonly IHashService _hashService;
    private readonly IUserRepository _repo;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository repo, IHashService hashService, ITokenService tokenService, IEmailService emailService)
    {
        _repo = repo;
        _hashService = hashService;
        _tokenService = tokenService;
        _emailService = emailService;
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

        var (token, refreshToken) = _tokenService.GenerateToken(user.Id);

        return new LoginDto
        {
            IsSuccess = true,
            Token = token,
            RefreshToken = refreshToken
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

        var (token, refreshToken) = _tokenService.GenerateToken(newUser.Id);

        return new RegisterDto
        {
            IsSuccess = true,
            Token = token,
            RefreshToken = refreshToken
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
}
