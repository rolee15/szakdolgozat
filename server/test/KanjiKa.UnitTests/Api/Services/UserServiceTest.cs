using KanjiKa.Application.Services;
using KanjiKa.Application.DTOs.User;
using KanjiKa.Domain.Entities.Users;
using KanjiKa.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using static KanjiKa.Domain.Entities.Users.UserRole;

namespace KanjiKa.UnitTests.Api.Services;

public class UserServiceTest
{
    private static IConfiguration BuildConfig(string? frontendBaseUrl = "http://localhost:5173")
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:RefreshTokenExpirationDays"] = "7",
            ["App:FrontendBaseUrl"] = frontendBaseUrl
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

    // -------------------------------------------------------------------------
    // Login tests
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Login_UserNotFound_ReturnsFailure()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        LoginDto result = await service.Login("u", "p");

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsFailure()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2], IsActive = true };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);
        hash.Setup(h => h.Verify("p", user.PasswordHash, user.PasswordSalt)).Returns(false);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        LoginDto result = await service.Login("u", "p");

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task Login_InactiveUser_ReturnsFailureWithActivationMessage()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2], IsActive = false };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);
        hash.Setup(h => h.Verify("p", user.PasswordHash, user.PasswordSalt)).Returns(true);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        LoginDto result = await service.Login("u", "p");

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
        Assert.Contains("activation", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Login_Success_ReturnsTokensAndUserId()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2], IsActive = true };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);
        hash.Setup(h => h.Verify("p", user.PasswordHash, user.PasswordSalt)).Returns(true);
        token.Setup(t => t.GenerateToken(1, "u", UserRole.User, false)).Returns(("tkn", "rtkn"));
        repo.Setup(r => r.UpdateRefreshTokenAsync(1, "rtkn", It.IsAny<DateTimeOffset>())).Returns(Task.CompletedTask);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        LoginDto result = await service.Login("u", "p");

        Assert.True(result.IsSuccess);
        Assert.Equal("tkn", result.Token);
        Assert.Equal("rtkn", result.RefreshToken);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public async Task Login_Success_StoresRefreshToken()
    {
        var user = new User { Id = 7, Username = "u", PasswordHash = [1], PasswordSalt = [2], IsActive = true };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);
        hash.Setup(h => h.Verify("p", user.PasswordHash, user.PasswordSalt)).Returns(true);
        token.Setup(t => t.GenerateToken(7, "u", UserRole.User, false)).Returns(("acc", "ref"));
        repo.Setup(r => r.UpdateRefreshTokenAsync(7, "ref", It.IsAny<DateTimeOffset>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        await service.Login("u", "p");

        repo.Verify(r => r.UpdateRefreshTokenAsync(7, "ref", It.IsAny<DateTimeOffset>()), Times.Once);
    }

    // -------------------------------------------------------------------------
    // Register tests
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Register_AlreadyExists_ReturnsFailure()
    {
        var existing = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(existing);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        RegisterDto result = await service.Register("u", "p");

        Assert.False(result.Success);
        Assert.NotEmpty(result.Message);
    }

    [Fact]
    public async Task Register_NewUser_SetsIsActiveToFalse()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);
        hash.Setup(h => h.Hash("p")).Returns((new byte[] { 9 }, new byte[] { 8 }));
        repo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        email.Setup(e => e.SendActivationEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        User? capturedUser = null;
        repo.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => capturedUser = u)
            .Returns(Task.CompletedTask);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        await service.Register("u", "p");

        Assert.NotNull(capturedUser);
        Assert.False(capturedUser!.IsActive);
    }

    [Fact]
    public async Task Register_NewUser_SetsActivationTokenAndExpiry()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);
        hash.Setup(h => h.Hash("p")).Returns((new byte[] { 9 }, new byte[] { 8 }));
        email.Setup(e => e.SendActivationEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        User? capturedUser = null;
        repo.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => capturedUser = u)
            .Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        await service.Register("u", "p");

        Assert.NotNull(capturedUser);
        Assert.NotNull(capturedUser!.ActivationToken);
        Assert.NotEmpty(capturedUser.ActivationToken);
        Assert.NotNull(capturedUser.ActivationTokenExpiry);
        Assert.True(capturedUser.ActivationTokenExpiry > DateTime.UtcNow);
    }

    [Fact]
    public async Task Register_NewUser_SendsActivationEmail()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);
        hash.Setup(h => h.Hash("p")).Returns((new byte[] { 9 }, new byte[] { 8 }));
        repo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        email.Setup(e => e.SendActivationEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        await service.Register("u", "p");

        email.Verify(e => e.SendActivationEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Register_NewUser_HashesAddsSavesAndReturnsSuccess()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);
        hash.Setup(h => h.Hash("p")).Returns((new byte[] { 9 }, new byte[] { 8 }));
        repo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
        email.Setup(e => e.SendActivationEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        RegisterDto result = await service.Register("u", "p");

        Assert.True(result.Success);
        Assert.NotEmpty(result.Message);
        repo.Verify();
    }

    [Fact]
    public async Task Register_EmailFailure_StillReturnsSuccess()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);
        hash.Setup(h => h.Hash("p")).Returns((new byte[] { 9 }, new byte[] { 8 }));
        repo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        email.Setup(e => e.SendActivationEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("SMTP failure"));

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        RegisterDto result = await service.Register("u", "p");

        Assert.True(result.Success);
    }

    // -------------------------------------------------------------------------
    // ActivateAccount tests
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Activate_EmptyToken_ReturnsFailure()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ActivateDto result = await service.ActivateAccount(string.Empty);

        Assert.False(result.Success);
        Assert.NotEmpty(result.Message);
    }

    [Fact]
    public async Task Activate_InvalidToken_ReturnsFailure()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByActivationTokenAsync("badtoken")).ReturnsAsync((User?)null);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ActivateDto result = await service.ActivateAccount("badtoken");

        Assert.False(result.Success);
        Assert.NotEmpty(result.Message);
    }

    [Fact]
    public async Task Activate_ExpiredToken_ReturnsExpiredMessage()
    {
        var user = new User
        {
            Id = 1,
            Username = "u",
            PasswordHash = [1],
            PasswordSalt = [2],
            IsActive = false,
            ActivationToken = "expiredtoken",
            ActivationTokenExpiry = DateTime.UtcNow.AddHours(-1)
        };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByActivationTokenAsync("expiredtoken")).ReturnsAsync(user);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ActivateDto result = await service.ActivateAccount("expiredtoken");

        Assert.False(result.Success);
        Assert.Contains("expired", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Activate_AlreadyActive_ReturnsIdempotentSuccess()
    {
        var user = new User
        {
            Id = 1,
            Username = "u",
            PasswordHash = [1],
            PasswordSalt = [2],
            IsActive = true,
            ActivationToken = "validtoken",
            ActivationTokenExpiry = DateTime.UtcNow.AddHours(23)
        };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByActivationTokenAsync("validtoken")).ReturnsAsync(user);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ActivateDto result = await service.ActivateAccount("validtoken");

        Assert.True(result.Success);
    }

    [Fact]
    public async Task Activate_ValidToken_ReturnsSuccessAndClearsToken()
    {
        var user = new User
        {
            Id = 1,
            Username = "u",
            PasswordHash = [1],
            PasswordSalt = [2],
            IsActive = false,
            ActivationToken = "goodtoken",
            ActivationTokenExpiry = DateTime.UtcNow.AddHours(23)
        };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByActivationTokenAsync("goodtoken")).ReturnsAsync(user);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ActivateDto result = await service.ActivateAccount("goodtoken");

        Assert.True(result.Success);
        Assert.True(user.IsActive);
        Assert.Null(user.ActivationToken);
        Assert.Null(user.ActivationTokenExpiry);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    // -------------------------------------------------------------------------
    // ForgotPassword tests
    // -------------------------------------------------------------------------

    [Fact]
    public async Task ForgotPassword_UserFound_SendsEmailWithCode()
    {
        var user = new User { Id = 1, Username = "e@x.com", PasswordHash = [1], PasswordSalt = [2] };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("e@x.com")).ReturnsAsync(user);
        repo.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        email.Setup(e => e.SendEmail("e@x.com", It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask).Verifiable();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ForgotPasswordDto result = await service.ForgotPassword("e@x.com");

        Assert.NotNull(result);
        email.Verify(e => e.SendEmail("e@x.com", It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ForgotPassword_UserNotFound_ReturnsWithoutSendingEmail()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ForgotPasswordDto result = await service.ForgotPassword("unknown@x.com");

        Assert.NotNull(result);
        email.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    // -------------------------------------------------------------------------
    // ResetPassword tests
    // -------------------------------------------------------------------------

    [Fact]
    public async Task ResetPassword_UserNotFound_ReturnsFailure()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ResetPasswordDto result = await service.ResetPassword("u", "12345", "new");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ResetPassword_InvalidCode_ReturnsFailure()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ResetPasswordDto result = await service.ResetPassword("u", "99999", "new");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ResetPassword_ValidCode_UpdatesPasswordAndSaves()
    {
        var user = new User
        {
            Id = 1,
            Username = "u",
            PasswordHash = [1],
            PasswordSalt = [2],
            PasswordResetCode = "12345",
            PasswordResetExpiry = DateTimeOffset.UtcNow.AddMinutes(10)
        };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);
        hash.Setup(h => h.Hash("new")).Returns((new byte[] { 7 }, new byte[] { 6 }));
        repo.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ResetPasswordDto result = await service.ResetPassword("u", "12345", "new");

        Assert.True(result.IsSuccess);
        Assert.Equal(new byte[] { 7 }, user.PasswordHash);
        Assert.Equal(new byte[] { 6 }, user.PasswordSalt);
        Assert.Null(user.PasswordResetCode);
        repo.Verify();
    }

    // -------------------------------------------------------------------------
    // RefreshToken tests
    // -------------------------------------------------------------------------

    [Fact]
    public async Task RefreshToken_ValidToken_ReturnsNewAccessToken()
    {
        var user = new User
        {
            Id = 1,
            Username = "u",
            PasswordHash = [1],
            PasswordSalt = [2],
            RefreshToken = "refresh",
            RefreshTokenExpiry = DateTimeOffset.UtcNow.AddDays(7)
        };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByRefreshTokenAsync("refresh")).ReturnsAsync(user);
        token.Setup(t => t.GenerateToken(1, "u", UserRole.User, false)).Returns(("newtoken", "newrefresh"));
        repo.Setup(r => r.UpdateRefreshTokenAsync(1, "newrefresh", It.IsAny<DateTimeOffset>())).Returns(Task.CompletedTask);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        RefreshTokenDto result = await service.RefreshToken("old", "refresh");

        Assert.Equal("newtoken", result.Token);
    }

    [Fact]
    public async Task RefreshToken_InvalidToken_ReturnsEmptyToken()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        RefreshTokenDto result = await service.RefreshToken("old", "invalid");

        Assert.Equal(string.Empty, result.Token);
    }

    [Fact]
    public async Task RefreshToken_ExpiredToken_ReturnsEmptyToken()
    {
        var user = new User
        {
            Id = 1,
            Username = "u",
            PasswordHash = [1],
            PasswordSalt = [2],
            RefreshToken = "expired",
            RefreshTokenExpiry = DateTimeOffset.UtcNow.AddDays(-1)
        };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByRefreshTokenAsync("expired")).ReturnsAsync(user);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        RefreshTokenDto result = await service.RefreshToken("old", "expired");

        Assert.Equal(string.Empty, result.Token);
    }

    // -------------------------------------------------------------------------
    // ChangePassword tests
    // -------------------------------------------------------------------------

    [Fact]
    public async Task ChangePassword_UserNotFound_ReturnsFailure()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ChangePasswordDto result = await service.ChangePassword(99, "current", "new");

        Assert.False(result.IsSuccess);
        Assert.Equal("User not found", result.ErrorMessage);
    }

    [Fact]
    public async Task ChangePassword_WrongCurrentPassword_ReturnsFailure()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        hash.Setup(h => h.Verify("wrong", user.PasswordHash, user.PasswordSalt)).Returns(false);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ChangePasswordDto result = await service.ChangePassword(1, "wrong", "new");

        Assert.False(result.IsSuccess);
        Assert.Equal("Current password is incorrect", result.ErrorMessage);
    }

    [Fact]
    public async Task ChangePassword_ValidRequest_UpdatesPasswordAndClearsMustChange()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2], MustChangePassword = true };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        hash.Setup(h => h.Verify("current", user.PasswordHash, user.PasswordSalt)).Returns(true);
        hash.Setup(h => h.Hash("new")).Returns((new byte[] { 5 }, new byte[] { 6 }));
        repo.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        ChangePasswordDto result = await service.ChangePassword(1, "current", "new");

        Assert.True(result.IsSuccess);
        Assert.False(user.MustChangePassword);
        Assert.Equal(new byte[] { 5 }, user.PasswordHash);
        Assert.Equal(new byte[] { 6 }, user.PasswordSalt);
    }

    [Fact]
    public async Task ChangePassword_ValidRequest_CallsUpdateAndSave()
    {
        var user = new User { Id = 2, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(user);
        hash.Setup(h => h.Verify("current", user.PasswordHash, user.PasswordSalt)).Returns(true);
        hash.Setup(h => h.Hash("new")).Returns((new byte[] { 7 }, new byte[] { 8 }));
        repo.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        await service.ChangePassword(2, "current", "new");

        repo.Verify(r => r.UpdateAsync(user), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
