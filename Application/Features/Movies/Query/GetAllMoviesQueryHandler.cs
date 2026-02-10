using Application.Dtos;
using Application.Interface;
using Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies.Query;

public record GetAllMoviesQuery() : IRequest<Result>;
public class GetAllMoviesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAllMoviesQuery, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await _context.Movies
            .AsNoTracking()
            .Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title
            })
            .ToListAsync(cancellationToken);
        
        if(movies is null)
        {
            return Result.Failure("oops! movies list is empty");
        }

        return Result.Success("success!", movies);
    }
}