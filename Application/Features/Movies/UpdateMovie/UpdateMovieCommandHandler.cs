using Application.Features.Movies.UpdateMovie;
using MediatR;
using MovieManager.Models;
using MovieManager.Models.Abstractions;

namespace MovieManager.Controllers.Commands.UpdateMovie;

public class UpdateMovieCommandHandler(IMovieManagerRepository repository) : IRequestHandler<UpdateMovieCommand, ApiResponse>
{
    private readonly IMovieManagerRepository _repository = repository;

    public async Task<ApiResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var updatedMovie = await _repository.UpdateMovieAsync(request, cancellationToken);

        return updatedMovie;
    }
}