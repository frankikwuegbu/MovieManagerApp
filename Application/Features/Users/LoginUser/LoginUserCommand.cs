using MediatR;
using MovieManager.Models;

namespace Application.Features.Users.LoginUser;

public record LoginUserCommand(
    string UserName,
    string Password
) : IRequest<ApiResponse>;