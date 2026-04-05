using KanjiKa.Application.Interfaces;

namespace KanjiKa.Application.Services;

public class DummyEmailService : IEmailService
{
    public Task SendEmail(string email, string subject, string body)
    {
        return Task.CompletedTask;
    }
}
