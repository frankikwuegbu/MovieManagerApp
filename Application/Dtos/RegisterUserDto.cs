using Domain.Entities;

namespace MovieManager.Models.Dtos;

public class RegisterUserDto
{
    public required string FullName { get; set; }
    public required string UserName { get; set; }
    public required string Password  { get; set; }
    public UserRoles Roles { get; set; }
}
