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
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_fromAddress));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using SmtpClient client = new();
        await client.ConnectAsync(_host, _port, MailKit.Security.SecureSocketOptions.None);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendActivationEmailAsync(string to, string username, string activationLink)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_fromAddress));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = "Activate your KanjiKa account";
        message.Body = new TextPart("html")
        {
            Text = $"<p>Hi {username},</p>" +
                   $"<p>Please activate your account by clicking the link below:</p>" +
                   $"<p><a href=\"{activationLink}\">{activationLink}</a></p>" +
                   $"<p>This link expires in 24 hours.</p>"
        };

        using SmtpClient client = new();
        await client.ConnectAsync(_host, _port, MailKit.Security.SecureSocketOptions.None);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
