using Application.Interface;
using Application;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailSenderService(IConfiguration config) : IEmailSenderService
    {
        private readonly IConfiguration _config = config;
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpSettings = _config.GetSection("Smtp");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress
                (smtpSettings["Username"],
                smtpSettings["From"]));

            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var client = new SmtpClient();
            client.Connect(smtpSettings["Host"],
                int.Parse(smtpSettings["Port"]!),
                bool.Parse(smtpSettings["EnableSsl"]!));

            client.Authenticate(smtpSettings["Username"],
                smtpSettings["Password"]);

            client.Send(message);
        }
    }
}