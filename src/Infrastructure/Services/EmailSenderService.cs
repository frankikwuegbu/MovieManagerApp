using Application.Interface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Services;

public class EmailSenderService(IConfiguration config) : IEmailSenderService
{
    private readonly IConfiguration _config = config;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpSettings = _config.GetSection("Smtp");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            smtpSettings["Username"],
            smtpSettings["From"]
        ));

        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            smtpSettings["Host"],
            int.Parse(smtpSettings["Port"]!),
            bool.Parse(smtpSettings["EnableSsl"]!)
        );

        await client.AuthenticateAsync(
            smtpSettings["Username"],
            smtpSettings["Password"]
        );

        await client.SendAsync(message);

        await client.DisconnectAsync(true);
    }
}