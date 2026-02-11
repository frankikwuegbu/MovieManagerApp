using Application.Interface;
using AutoMapper;
using Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies.Query;

public record GetMovieByIdQuery(Guid Id) : IRequest<Result>;
public class GetMovieByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMovieByIdQuery, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if(movie is null)
        {
            return Result.Failure("movie not found");
        }

        var movieDto = _mapper.Map<MovieByIdDto>(movie);

        return Result.Success("movie found", movieDto);
    }
}