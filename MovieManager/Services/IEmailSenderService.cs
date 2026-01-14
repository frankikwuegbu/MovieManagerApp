namespace MovieManager.Services;

public interface IEmailSenderService
{
    public Task SendEmailAsync(string email, string subject, string message);
}
