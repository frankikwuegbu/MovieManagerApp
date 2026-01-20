using MediatR;
using MovieManager.Data;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers.Queries.GetMovieById;

public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, Movie?>
{
    private readonly IMovieManagerRepository _repository;

    public GetMovieByIdHandler(IMovieManagerRepository repository)
    {
        _repository = repository;
    }
    public async Task<Movie?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var foundMovie = await _repository.GetMovieByIdAsync(request, cancellationToken);

        return foundMovie;
    }
}
