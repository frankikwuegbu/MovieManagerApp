namespace Application.Features.Users;

public class UserByIdDto
{
    public string Id { get; set; }
    public required string FullName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
