using Application.Interface;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Features.Users.Command;
public record LoginUserCommand(
    string UserName,
    string Password
) : IRequest<ApiResponse>;

public class LoginUserCommandHandler(IUsersDbContext context, IValidator<LoginUserCommand> validator) : IRequestHandler<LoginUserCommand, ApiResponse>
{
    private readonly IUsersDbContext _context = context;
    private readonly IValidator<LoginUserCommand> _validator = validator;

    public async Task<ApiResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var loginUser = await _context.LoginUserAsync(request);
        return loginUser;
    }
}
