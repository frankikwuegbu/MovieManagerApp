using MediatR;
using MovieManager.Data;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers.Queries.GetAllMovies;

public class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, List<Movie>>
{
    private readonly IMovieManagerRepository _repository;

    public GetAllMoviesHandler(IMovieManagerRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<Movie>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var allMovies = await _repository.GetAllMoviesAsync(request, cancellationToken);

        return allMovies;
    }
}
