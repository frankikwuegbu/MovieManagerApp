using Application.Features.Users.LoginUser;
using Application.Features.Users.RegisterUser;

namespace MovieManager.Models.Abstractions;

public interface IMovieManagerUsersRepository
{
    public Task<ApiResponse> LoginUserAsync(LoginUserCommand request);
    public Task<ApiResponse> RegisterUserAsync(RegisterUserCommand request);
}
