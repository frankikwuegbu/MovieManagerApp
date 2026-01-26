namespace MovieManager.Models.Dtos;

public class UpdateMovieDto
{
    public required string Genre { get; set; }
    public bool IsShowing { get; set; }
}