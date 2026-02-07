namespace Application.Dtos;

public class UserByIdDto
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
