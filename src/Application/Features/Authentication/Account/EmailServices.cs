using BookPro.Application.Features.Authentication.Account.Interface;
using BookPro.Common.Config;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BookPro.Application.Features.Authentication.Account;

public class EmailServices : IEmailServices
{
    private readonly SettingsEmail _configuration;

    public EmailServices(IOptions<SettingsEmail> configuration)
    {
        _configuration = configuration.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("BookPro", _configuration.FromEmail));
        email.To.Add(new MailboxAddress("", to));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_configuration.SmtpServer, _configuration.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_configuration.FromEmail, _configuration.Password);
        await client.SendAsync(email);
        await client.DisconnectAsync(true);
    }
}
