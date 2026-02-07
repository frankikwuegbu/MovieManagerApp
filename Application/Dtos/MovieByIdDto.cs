namespace Application.Dtos;

public class MovieByIdDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsShowing { get; set; }
}