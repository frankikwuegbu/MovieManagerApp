using Microsoft.AspNetCore.Identity;

namespace MovieManager.Models.Entities;

public enum UserRoles
{
    ADMIN = 0,
    USER = 1
}

public class User : IdentityUser
{
    public string FullName { get; set; }
    public UserRoles Role { get; set; }
}
