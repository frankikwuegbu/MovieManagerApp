using Application.Interface;
using Application.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers;

public class UserRegisteredEventHandler(IEmailSenderService emailSender, ILogger<UserRegisteredEventHandler> logger) : INotificationHandler<UserRegisteredEvent>
{
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly ILogger<UserRegisteredEventHandler> _logger = logger;

    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling registration email for: {notification.User.Email}");

        try
        {
            await _emailSender.SendEmailAsync(
                notification.User.Email,
                "Welcome to Movie Manager!",
                $"Hi {notification.User.UserName}, thanks for joining us!");

            _logger.LogInformation($"Registration email sent to: {notification.User.Email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send registration email to: {notification.User.UserName}");
        }
    }
}