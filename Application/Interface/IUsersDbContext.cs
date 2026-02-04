using Application.Features.Users.Command;
using Domain;

namespace Application.Interface;

public interface IUsersDbContext
{
    public Task<ApiResponse> LoginUserAsync(LoginUserCommand request);
    public Task<ApiResponse> RegisterUserAsync(RegisterUserCommand request, CancellationToken cancellationToken);
}
