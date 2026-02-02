using Application.Interface;
using Domain;
using MediatR;

namespace Application.Features.Movies.DeleteMovie;

public class DeleteMovieCommandHandler(IMoviesDbContext context) : IRequestHandler<DeleteMovieCommand, ApiResponse>
{
    private readonly IMoviesDbContext _context = context;

    public async Task<ApiResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var deletedMovie = await _context.DeleteMovieAsync(request, cancellationToken);

        return deletedMovie;
    }
}