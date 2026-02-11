using Application.Features.Movies;
using Application.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Movies.Query;

public record GetAllMoviesQuery() : IRequest<Result>;
public class GetAllMoviesQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetAllMoviesQuery, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await _context.Movies
            .AsNoTracking().ToListAsync(cancellationToken);

        //.Select(m => new MovieDto
        //{
        //    Id = m.Id,
        //    Title = m.Title
        //})
        //.ToListAsync(cancellationToken);

        if (movies is null)
        {
            return Result.Failure("oops! movies list is empty");
        }

        var moviesDto = _mapper.Map<List<MovieDto>>(movies);

        return Result.Success("success!", moviesDto); 
    }
}