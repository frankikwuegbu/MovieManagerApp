using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace MovieManager.Controllers.Commands.DeleteMovie;

public class DeleteMovieHandler(IMovieManagerRepository repository) : IRequestHandler<DeleteMovieCommand, ApiResponse>
{
    private readonly IMovieManagerRepository _repository = repository;

    public async Task<ApiResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var deletedMovie = await _repository.DeleteMovieAsync(request, cancellationToken);

        return deletedMovie;
    }
}