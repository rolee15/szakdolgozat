using KanjiKa.Core.Interfaces;

namespace KanjiKa.Api.Services;

public class DummyEmailService : IEmailService
{
    public Task SendEmail(string email, string subject, string body)
    {
        return Task.CompletedTask;
    }
}