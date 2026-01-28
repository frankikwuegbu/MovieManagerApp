using FluentValidation;
using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace Application.Features.Users.RegisterUser;

public class RegisterUserCommandHandler(IMovieManagerUsersRepository repository, IValidator<RegisterUserCommand> validator) : IRequestHandler<RegisterUserCommand, ApiResponse>
{
    private readonly IMovieManagerUsersRepository _repository = repository;
    private readonly IValidator<RegisterUserCommand> _validator = validator;

    public async Task<ApiResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var registerUser = await _repository.RegisterUserAsync(request);
        return registerUser;
    }
}