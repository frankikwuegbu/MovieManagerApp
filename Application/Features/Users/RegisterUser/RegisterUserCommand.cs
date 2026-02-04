using Domain;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.RegisterUser;

public record RegisterUserCommand(
    string FullName,
    string UserName,
    string Password,
    UserRoles Roles
) : IRequest<ApiResponse>;
