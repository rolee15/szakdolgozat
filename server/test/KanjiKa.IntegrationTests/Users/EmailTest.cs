#if INTEGRATION_TESTS_ENABLED
using System.Net.Http.Json;

namespace KanjiKa.IntegrationTests.Users;

[Collection("TestContainer")]
public class EmailTest(CustomWebApplicationFactory factory) : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        factory.ResetEmails();
        await factory.TearDownDatabaseAsync();
        await factory.SeedDatabaseAsync();
    }

    public async Task DisposeAsync() => await Task.CompletedTask;

    [Fact(Skip = "Integration tests are temporarily disabled until TestContainers is ready")]
    public async Task Register_SendsWelcomeEmail()
    {
        // Arrange
        var request = new { Email = "newuser@example.com", Password = "Test1234!" };

        // Act
        await factory.HttpClient.PostAsJsonAsync("/api/users/register", request);

        // Assert
        Assert.Single(factory.FakeEmail.SentEmails);
        var (email, subject, _) = factory.FakeEmail.SentEmails[0];
        Assert.Equal("newuser@example.com", email);
        Assert.Contains("Welcome", subject);
    }

    [Fact(Skip = "Integration tests are temporarily disabled until TestContainers is ready")]
    public async Task ForgotPassword_SendsResetCodeEmail()
    {
        // Arrange — register a user first so the account exists
        var registerRequest = new { Email = "resetuser@example.com", Password = "Test1234!" };
        await factory.HttpClient.PostAsJsonAsync("/api/users/register", registerRequest);
        factory.ResetEmails();

        var forgotRequest = new { Email = "resetuser@example.com" };

        // Act
        await factory.HttpClient.PostAsJsonAsync("/api/users/forgotPassword", forgotRequest);

        // Assert
        Assert.Single(factory.FakeEmail.SentEmails);
        var (_, subject, body) = factory.FakeEmail.SentEmails[0];
        Assert.Equal("Forgot Password", subject);
        Assert.Contains("Reset code:", body);
    }
}
#endif
