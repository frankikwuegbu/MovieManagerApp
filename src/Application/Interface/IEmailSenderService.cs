using Application;

namespace Application.Interface;

public interface IEmailSenderService
{
    public Task SendEmailAsync(string to, string subject, string body);
}
