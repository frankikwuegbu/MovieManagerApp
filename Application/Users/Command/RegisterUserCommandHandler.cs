using Application.Entities;
using Application.Interface;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Command;
public record RegisterUserCommand(
    string FullName,
    string UserName,
    string Email,
    string Password,
    UserRoles Roles
) : IRequest<Result>;

public class RegisterUserCommandHandler(IValidator<RegisterUserCommand> validator,
    IIdentityService identityService,
    IEmailSenderService emailSender,
    ILogger<RegisterUserCommandHandler> logger)
    : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IValidator<RegisterUserCommand> _validator = validator;
    private readonly IIdentityService _identityService = identityService;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly ILogger<RegisterUserCommandHandler> _logger = logger;

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        try
        {
            await _emailSender.SendEmailAsync(
                request.Email,
                "Welcome to Movie Manager!",
                $"Hi {request.UserName}, thanks for joining us!");

            _logger.LogInformation($"Registration email sent to: {request.Email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send registration email to: {request.Email}");
        }

        return await _identityService.CreateUserAsync(request, request.Password);
    }
}