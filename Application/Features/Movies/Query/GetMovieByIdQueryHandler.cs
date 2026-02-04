using Application.Interface;
using Domain;
using MediatR;

namespace Application.Features.Movies.Query;

public record GetMovieByIdQuery(Guid Id) : IRequest<ApiResponse?>;
public class GetMovieByIdQueryHandler(IMoviesDbContext context) : IRequestHandler<GetMovieByIdQuery, ApiResponse?>
{
    private readonly IMoviesDbContext _context = context;

    public async Task<ApiResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var foundMovie = await _context.GetMovieByIdAsync(request, cancellationToken);

        return foundMovie;
    }
}
