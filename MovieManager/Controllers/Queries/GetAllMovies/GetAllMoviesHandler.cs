using MediatR;
using MovieManager.Data;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers.Queries.GetAllMovies;

public class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, List<Movie>?>
{
    private readonly MovieManagerDbContext dbContext;

    public GetAllMoviesHandler(MovieManagerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<List<Movie>?> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var allMovies = dbContext.Movies.ToList();

        return allMovies;
    }
}
