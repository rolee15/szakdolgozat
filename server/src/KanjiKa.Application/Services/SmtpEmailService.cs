using KanjiKa.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace KanjiKa.Application.Services;

public class SmtpEmailService : IEmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _fromAddress;

    public SmtpEmailService(IConfiguration configuration)
    {
        _host = configuration["Smtp:Host"]!;
        _port = int.Parse(configuration["Smtp:Port"]!);
        _fromAddress = configuration["Smtp:FromAddress"]!;
    }

    public async Task SendEmail(string email, string subject, string body)
    {
        MimeMessage message = new();
        message.From.Add(MailboxAddress.Parse(_fromAddress));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using SmtpClient client = new();
        await client.ConnectAsync(_host, _port, MailKit.Security.SecureSocketOptions.None);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
