using Domain;
using MediatR;

namespace Application.Features.Users.LoginUser;

public record LoginUserCommand(
    string UserName,
    string Password
) : IRequest<ApiResponse>;