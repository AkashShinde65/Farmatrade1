using FarmaTrade_OTP_Service.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FarmaTrade_OTP_Service.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendOtpEmailAsync(string email, string otp)
    {
        if (string.IsNullOrWhiteSpace(_settings.Username) ||
            string.IsNullOrWhiteSpace(_settings.AppPassword))
        {
            throw new InvalidOperationException("Gmail SMTP credentials are not configured.");
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.Username));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = "FarmaTrade OTP Verification";

        message.Body = new TextPart("html")
        {
            Text = $"""
                    <h2>FarmaTrade OTP Verification</h2>
                    <p>Your FarmaTrade verification code is:</p>
                    <h1>{otp}</h1>
                    <p>This OTP will expire in 5 minutes.</p>
                    <p>Please do not share this OTP with anyone.</p>
                    """
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _settings.SmtpServer,
            _settings.Port,
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _settings.Username,
            _settings.AppPassword);

        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
