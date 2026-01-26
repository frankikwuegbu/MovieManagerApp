using MediatR;
using MovieManager.Models;
using MovieManager.Models.Entities;

namespace Application.Features.Users.RegisterUser;

public record RegisterUserCommand(
    string FullName,
    string UserName,
    string Password,
    UserRoles Roles
) : IRequest<ApiResponse>;
