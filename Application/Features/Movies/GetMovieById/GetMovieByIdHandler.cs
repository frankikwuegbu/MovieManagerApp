using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace MovieManager.Controllers.Queries.GetMovieById;

public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, ApiResponse?>
{
    private readonly IMovieManagerRepository _repository;

    public GetMovieByIdHandler(IMovieManagerRepository repository)
    {
        _repository = repository;
    }
    public async Task<ApiResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var foundMovie = await _repository.GetMovieByIdAsync(request, cancellationToken);

        return foundMovie;
    }
}
