using MediatR;
using MovieManager.Data;

namespace MovieManager.Controllers.Commands.DeleteMovie;

public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, string?>
{
    private readonly MovieManagerDbContext dbContext;

    public DeleteMovieHandler(MovieManagerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<string?> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = dbContext.Movies.Find(request.Id);

        if (movie is null)
        {
            return "movie not found";
        }

        dbContext.Movies.Remove(movie);
        await dbContext.SaveChangesAsync(cancellationToken);

        return "movie deleted";
    }
}
