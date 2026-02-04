using Domain.Common;
using Domain.Interfaces;

namespace MovieManager.Models.Entities;

public class Movie : BaseEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsShowing { get; set; }
}
