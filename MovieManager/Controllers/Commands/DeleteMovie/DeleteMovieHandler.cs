using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace MovieManager.Controllers.Commands.DeleteMovie;

public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, ApiResponse>
{
    private readonly IMovieManagerRepository _repository;

    public DeleteMovieHandler(IMovieManagerRepository repository)
    {
        _repository = repository;
    }
    public async Task<ApiResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var deletedMovie = await _repository.DeleteMovieAsync(request, cancellationToken);

        return deletedMovie;
    }
}