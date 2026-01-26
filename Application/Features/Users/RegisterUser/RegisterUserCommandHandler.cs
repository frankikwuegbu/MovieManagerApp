using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace Application.Features.Users.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResponse>
{
    private readonly IMovieManagerUsersRepository _repository;

    public RegisterUserCommandHandler(IMovieManagerUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerUser = await _repository.RegisterUserAsync(request);

        return registerUser;
    }
}