using Application.Interface;
using Domain;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.Users.Command;
public record RegisterUserCommand(
    string FullName,
    string UserName,
    string Email,
    string Password,
    UserRoles Roles
) : IRequest<Result>;

public class RegisterUserCommandHandler(IApplicationDbContext context,
    IValidator<RegisterUserCommand> validator,
    IIdentityService identityService)
    : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IValidator<RegisterUserCommand> _validator = validator;
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        return await _identityService.CreateUserAsync(request, request.Password);
    }
}