namespace KanjiKa.Application.Interfaces;

public interface IEmailService
{
    public Task SendEmail(string email, string subject, string body);
}
