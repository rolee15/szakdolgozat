using KanjiKa.Application.Interfaces;

namespace KanjiKa.IntegrationTests;

public class FakeEmailService : IEmailService
{
    public List<(string Email, string Subject, string Body)> SentEmails { get; } = [];

    public Task SendEmail(string email, string subject, string body)
    {
        SentEmails.Add((email, subject, body));
        return Task.CompletedTask;
    }

    public Task SendActivationEmailAsync(string to, string username, string activationLink)
    {
        SentEmails.Add((to, "Activate your KanjiKa account", activationLink));
        return Task.CompletedTask;
    }
}
