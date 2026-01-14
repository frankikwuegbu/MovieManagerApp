using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MovieManager.Data;
using MovieManager.Models.Dtos;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers.Commands.UpdateMovie;

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, Movie?>
{
    private readonly MovieManagerDbContext dbContext;

    public UpdateMovieCommandHandler(MovieManagerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Movie?> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = dbContext.Movies.Find(request.Id);

        if (movie is null)
            return null;

        movie.Genre = request.Genre;
        movie.IsShowing = request.IsShowing;

        await dbContext.SaveChangesAsync(cancellationToken);

        return movie;
    }
}
