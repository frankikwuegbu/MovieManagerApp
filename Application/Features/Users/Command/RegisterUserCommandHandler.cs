using Application.Interface;
using Domain;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Users.Command;
public record RegisterUserCommand(
    string FullName,
    string UserName,
    string Password,
    UserRoles Roles
) : IRequest<ApiResponse>;

public class RegisterUserCommandHandler(IUsersDbContext context, IValidator<RegisterUserCommand> validator) : IRequestHandler<RegisterUserCommand, ApiResponse>
{
    private readonly IUsersDbContext _context = context;
    private readonly IValidator<RegisterUserCommand> _validator = validator;

    public async Task<ApiResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var registerUser = await _context.RegisterUserAsync(request, cancellationToken);
        return registerUser;
    }
}