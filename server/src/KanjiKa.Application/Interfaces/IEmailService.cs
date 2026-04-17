namespace KanjiKa.Application.Interfaces;

public interface IEmailService
{
    public Task SendEmail(string email, string subject, string body);

    public Task SendActivationEmailAsync(string to, string username, string activationLink);
}
