namespace Domain.Entities;

public class Movie
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsShowing { get; set; }
}