using Application.Interface;
using Application;
using FluentValidation;
using MediatR;

namespace Application.Features.Users.Command;
public record LoginUserCommand(
    string Email,
    string Password
) : IRequest<Result>;

public class LoginUserCommandHandler(IApplicationDbContext context,
    IValidator<LoginUserCommand> validator,
    IIdentityService identityService) 
    : IRequestHandler<LoginUserCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<LoginUserCommand> _validator = validator;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        return await _identityService.LoginAsync(request);
    }
}
