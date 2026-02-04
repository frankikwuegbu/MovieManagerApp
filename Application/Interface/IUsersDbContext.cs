using Application.Features.Users.LoginUser;
using Application.Features.Users.RegisterUser;
using Domain;

namespace Application.Interface;

public interface IUsersDbContext
{
    public Task<ApiResponse> LoginUserAsync(LoginUserCommand request);
    public Task<ApiResponse> RegisterUserAsync(RegisterUserCommand request, CancellationToken cancellationToken);
}
