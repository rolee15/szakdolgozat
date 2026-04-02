using KanjiKa.Api.Services;
using KanjiKa.Core.DTOs.User;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using static KanjiKa.Core.Entities.Users.UserRole;

namespace KanjiKa.UnitTests.Api.Services;

public class UserServiceTest
{
    private static IConfiguration BuildConfig()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:RefreshTokenExpirationDays"] = "7"
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }

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
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
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
    public async Task Login_Success_ReturnsTokensAndUserId()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
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

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task Register_NewUser_HashesAddsSavesAndReturnsTokens()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);
        hash.Setup(h => h.Hash("p")).Returns((new byte[] { 9 }, new byte[] { 8 }));
        repo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();
        token.Setup(t => t.GenerateToken(It.IsAny<int>(), It.IsAny<string>(), UserRole.User, false)).Returns(("t2", "rt2"));
        repo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<int>(), "rt2", It.IsAny<DateTimeOffset>())).Returns(Task.CompletedTask);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());
        RegisterDto result = await service.Register("u", "p");

        Assert.True(result.IsSuccess);
        Assert.Equal("t2", result.Token);
        Assert.Equal("rt2", result.RefreshToken);
        repo.Verify();
    }

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

    [Fact]
    public async Task Login_Success_StoresRefreshToken()
    {
        // Arrange
        var user = new User { Id = 7, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
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

        // Act
        await service.Login("u", "p");

        // Assert
        repo.Verify(r => r.UpdateRefreshTokenAsync(7, "ref", It.IsAny<DateTimeOffset>()), Times.Once);
    }

    [Fact]
    public async Task Register_Success_ReturnsTokenInDto()
    {
        // Arrange
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("newuser")).ReturnsAsync((User?)null);
        hash.Setup(h => h.Hash("pass")).Returns((new byte[] { 3 }, new byte[] { 4 }));
        repo.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        token.Setup(t => t.GenerateToken(It.IsAny<int>(), "newuser", UserRole.User, false)).Returns(("newtkn", "newref"));
        repo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<int>(), "newref", It.IsAny<DateTimeOffset>()))
            .Returns(Task.CompletedTask);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());

        // Act
        RegisterDto result = await service.Register("newuser", "pass");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Token);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task ChangePassword_UserNotFound_ReturnsFailure()
    {
        // Arrange
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());

        // Act
        ChangePasswordDto result = await service.ChangePassword(99, "current", "new");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User not found", result.ErrorMessage);
    }

    [Fact]
    public async Task ChangePassword_WrongCurrentPassword_ReturnsFailure()
    {
        // Arrange
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        hash.Setup(h => h.Verify("wrong", user.PasswordHash, user.PasswordSalt)).Returns(false);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object, BuildConfig());

        // Act
        ChangePasswordDto result = await service.ChangePassword(1, "wrong", "new");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Current password is incorrect", result.ErrorMessage);
    }

    [Fact]
    public async Task ChangePassword_ValidRequest_UpdatesPasswordAndClearsMustChange()
    {
        // Arrange
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

        // Act
        ChangePasswordDto result = await service.ChangePassword(1, "current", "new");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(user.MustChangePassword);
        Assert.Equal(new byte[] { 5 }, user.PasswordHash);
        Assert.Equal(new byte[] { 6 }, user.PasswordSalt);
    }

    [Fact]
    public async Task ChangePassword_ValidRequest_CallsUpdateAndSave()
    {
        // Arrange
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

        // Act
        await service.ChangePassword(2, "current", "new");

        // Assert
        repo.Verify(r => r.UpdateAsync(user), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
