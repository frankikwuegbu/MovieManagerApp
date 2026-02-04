using Application.Interface;
using Domain;
using MediatR;

namespace Application.Features.Movies.Query;

public record GetAllMoviesQuery() : IRequest<ApiResponse>;
public class GetAllMoviesQueryHandler(IMoviesDbContext context) : IRequestHandler<GetAllMoviesQuery, ApiResponse>
{
    private readonly IMoviesDbContext _context = context;

    public async Task<ApiResponse> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var allMovies = await _context.GetAllMoviesAsync(request, cancellationToken);

        return allMovies;
    }
}