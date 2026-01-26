using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace MovieManager.Controllers.Queries.GetAllMovies;

public class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, ApiResponse>
{
    private readonly IMovieManagerRepository _repository;

    public GetAllMoviesHandler(IMovieManagerRepository repository)
    {
        _repository = repository;
    }
    public async Task<ApiResponse> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var allMovies = await _repository.GetAllMoviesAsync(request, cancellationToken);

        return allMovies;
    }
}
