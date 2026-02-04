using Application.Interface;
using Microsoft.Extensions.Configuration;
using MovieManager.Models;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class EmailSenderService(IConfiguration config) : IEmailSenderService
    {
        private readonly IConfiguration _config = config;
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpSettings = _config.GetSection("Smtp");

            var client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Host = smtpSettings["Host"]!,
                Port = int.Parse(smtpSettings["Port"]!),
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"]!),
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
                )
            );
        }
    }
}