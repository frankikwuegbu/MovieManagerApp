using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace MovieManager.Controllers.Commands.AddMovie;

public class AddMovieCommandHandler(IMovieManagerRepository repository) : IRequestHandler<AddMovieCommand, ApiResponse>
{
    private readonly IMovieManagerRepository _repository = repository;

    public async Task<ApiResponse> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var addedMovie = await _repository.AddMovieAsync(request, cancellationToken);

        return addedMovie;
    }
}