using Microsoft.AspNetCore.Identity;

namespace Application.Entities;

public enum UserRoles
{
    ADMIN = 0,
    USER = 1
}

public class User : IdentityUser
{
    public required string FullName { get; set; }
    public UserRoles Role { get; set; } = UserRoles.USER;
}