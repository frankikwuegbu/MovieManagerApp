using Application.Interface;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Features.Users.RegisterUser;

public class RegisterUserCommandHandler(IUsersDbContext context, IValidator<RegisterUserCommand> validator) : IRequestHandler<RegisterUserCommand, ApiResponse>
{
    private readonly IUsersDbContext _context = context;
    private readonly IValidator<RegisterUserCommand> _validator = validator;

    public async Task<ApiResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var registerUser = await _context.RegisterUserAsync(request);
        return registerUser;
    }
}