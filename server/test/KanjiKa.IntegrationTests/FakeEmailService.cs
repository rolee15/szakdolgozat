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
}
