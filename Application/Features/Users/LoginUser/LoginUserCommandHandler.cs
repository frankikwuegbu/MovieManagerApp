using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace Application.Features.Users.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApiResponse>
{
    private readonly IMovieManagerUsersRepository _repository;

    public LoginUserCommandHandler(IMovieManagerUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var loginUser = await _repository.LoginUserAsync(request);

        return loginUser;
    }
}
