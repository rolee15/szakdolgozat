using KanjiKa.Api.Services;
using KanjiKa.Core.DTOs.User;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Moq;

namespace KanjiKa.UnitTests.Api.Services;

public class UserServiceTest
{
    [Fact]
    public async Task Login_UserNotFound_ReturnsFailure()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
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

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
        LoginDto result = await service.Login("u", "p");

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task Login_Success_ReturnsTokens()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);
        hash.Setup(h => h.Verify("p", user.PasswordHash, user.PasswordSalt)).Returns(true);
        token.Setup(t => t.GenerateToken(1)).Returns(("tkn","rtkn"));

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
        LoginDto result = await service.Login("u", "p");

        Assert.True(result.IsSuccess);
        Assert.Equal("tkn", result.Token);
        Assert.Equal("rtkn", result.RefreshToken);
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

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
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
        token.Setup(t => t.GenerateToken(It.IsAny<int>())).Returns(("t2","rt2"));

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
        RegisterDto result = await service.Register("u", "p");

        Assert.True(result.IsSuccess);
        Assert.Equal("t2", result.Token);
        Assert.Equal("rt2", result.RefreshToken);
        repo.Verify();
    }

    [Fact]
    public async Task ForgotPassword_SendsEmail_ReturnsDto()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        email.Setup(e => e.SendEmail("e@x.com", It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask).Verifiable();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
        ForgotPasswordDto result = await service.ForgotPassword("e@x.com");

        Assert.NotNull(result);
        repo.Verify();
        email.Verify();
    }

    [Fact]
    public async Task ResetPassword_UserNotFound_ReturnsFailure()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync((User?)null);

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
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

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
        ResetPasswordDto result = await service.ResetPassword("u", "99999", "new");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ResetPassword_ValidCode_UpdatesPasswordAndSaves()
    {
        var user = new User { Id = 1, Username = "u", PasswordHash = [1], PasswordSalt = [2] };
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();
        repo.Setup(r => r.GetByUsernameAsync("u")).ReturnsAsync(user);
        hash.Setup(h => h.Hash("new")).Returns((new byte[] { 7 }, new byte[] { 6 }));
        repo.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask).Verifiable();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
        ResetPasswordDto result = await service.ResetPassword("u", "12345", "new");

        Assert.True(result.IsSuccess);
        Assert.Equal(new byte[] { 7 }, user.PasswordHash);
        Assert.Equal(new byte[] { 6 }, user.PasswordSalt);
        repo.Verify();
    }

    [Fact]
    public async Task RefreshToken_ReturnsNewToken()
    {
        var repo = new Mock<IUserRepository>();
        var hash = new Mock<IHashService>();
        var token = new Mock<ITokenService>();
        var email = new Mock<IEmailService>();

        var service = new UserService(repo.Object, hash.Object, token.Object, email.Object);
        RefreshTokenDto result = await service.RefreshToken("old", "refresh");

        Assert.Equal("test23456", result.Token);
    }
}
