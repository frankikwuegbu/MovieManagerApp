using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieManager.Models.Dtos;
using MovieManager.Models.Entities;
using MovieManager.Data;

namespace MovieManager.Controllers.Commands.AddMovie;

public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, Movie>
{
    private readonly MovieManagerDbContext dbContext;

    public AddMovieCommandHandler(MovieManagerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Movie> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = new Movie()
        {
            Title = request.Title,
            Genre = request.Genre,
            ReleaseYear = request.ReleaseYear,
            IsShowing = request.IsShowing
        };

        dbContext.Movies.Add(movie);

        await dbContext.SaveChangesAsync(cancellationToken);

        return movie;
    }
}
