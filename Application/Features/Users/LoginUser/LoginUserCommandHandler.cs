using FluentValidation;
using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace Application.Features.Users.LoginUser;

public class LoginUserCommandHandler(IMovieManagerUsersRepository repository, IValidator<LoginUserCommand> validator) : IRequestHandler<LoginUserCommand, ApiResponse>
{
    private readonly IMovieManagerUsersRepository _repository = repository;
    private readonly IValidator<LoginUserCommand> _validator = validator;

    public async Task<ApiResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var loginUser = await _repository.LoginUserAsync(request);
        return loginUser;
    }
}
