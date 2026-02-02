using Application.Interface;
using Domain;
using MediatR;

namespace Application.Features.Movies.UpdateMovie;

public class UpdateMovieCommandHandler(IMoviesDbContext context) : IRequestHandler<UpdateMovieCommand, ApiResponse>
{
    private readonly IMoviesDbContext _context = context;

    public async Task<ApiResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var updatedMovie = await _context.UpdateMovieAsync(request, cancellationToken);

        return updatedMovie;
    }
}