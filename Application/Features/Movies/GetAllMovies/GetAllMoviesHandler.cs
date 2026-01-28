using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace MovieManager.Controllers.Queries.GetAllMovies;

public class GetAllMoviesHandler(IMovieManagerRepository repository) : IRequestHandler<GetAllMoviesQuery, ApiResponse>
{
    private readonly IMovieManagerRepository _repository = repository;

    public async Task<ApiResponse> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var allMovies = await _repository.GetAllMoviesAsync(request, cancellationToken);

        return allMovies;
    }
}
