namespace MovieManager.Models.Abstractions;

public interface IEmailSenderService
{
    public Task SendEmailAsync(string email, string subject, string message);
}
