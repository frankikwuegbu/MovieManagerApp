using Application.Interface;
using AutoMapper;
using Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies.Command;

public record UpdateMovieCommand(
    Guid Id,
    string Genre,
    bool IsShowing
) : IRequest<Result>;
public class UpdateMovieCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateMovieCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {

        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if(movie is null)
        {
            return Result.Failure("movie not found!");
        }

        _mapper.Map(request, movie);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedMovie =  _mapper.Map<MovieDto>(movie);

        return Result.Success("update successful!", updatedMovie);
    }
}