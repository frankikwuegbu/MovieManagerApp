using Application.Interface;
using Domain;
using MediatR;

namespace Application.Features.Movies.Command;
public record AddMovieCommand(
    string Title,
    string Genre,
    int ReleaseYear,
    bool IsShowing
) : IRequest<ApiResponse>;
public class AddMovieCommandHandler(IMoviesDbContext context) : IRequestHandler<AddMovieCommand, ApiResponse>
{
    private readonly IMoviesDbContext _context = context;

    public async Task<ApiResponse> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var addedMovie = await _context.AddMovieAsync(request, cancellationToken);

        return addedMovie;
    }
}