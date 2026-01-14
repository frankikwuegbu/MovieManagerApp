using System.Net;
using System.Net.Mail;

namespace MovieManager.Services
{
    public class EmailSenderService
    {
        private readonly IConfiguration config;

        public EmailSenderService(IConfiguration config)
        {
            this.config = config;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSettings = config.GetSection("Smtp");

            var client = new SmtpClient
            {
                Host = smtpSettings["Host"]!,
                Port = int.Parse(smtpSettings["Port"]!),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                smtpSettings["Username"],
                smtpSettings["Password"]
                )
            };

            return client.SendMailAsync(
                new MailMessage(from: smtpSettings["From"]!,
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
