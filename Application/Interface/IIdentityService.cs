using Application.Features.Users.Command;
using Application;
using Application.Entities;

namespace Application.Interface;

public interface IIdentityService
{
    Task<Result> CreateUserAsync(RegisterUserCommand request, string password);
    Task<Result> LoginAsync(LoginUserCommand request);
}
