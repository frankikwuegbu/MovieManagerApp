using Application.Interface;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Movies.GetAllMovies;

public class GetAllMoviesQueryHandler(IMoviesDbContext context) : IRequestHandler<GetAllMoviesQuery, ApiResponse>
{
    private readonly IMoviesDbContext _context = context;

    public async Task<ApiResponse> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var allMovies = await _context.GetAllMoviesAsync(request, cancellationToken);

        return allMovies;
    }
}