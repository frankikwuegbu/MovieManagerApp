using MediatR;
using MovieManager.Data;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers.Queries.GetMovieById;

public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, Movie?>
{
    private readonly MovieManagerDbContext dbContext;

    public GetMovieByIdHandler(MovieManagerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<Movie?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = dbContext.Movies.Find(request.Id);

        return movie;
    }
}
