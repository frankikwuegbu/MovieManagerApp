using Application.Interface;
using Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Movies.Command;

public record DeleteMovieCommand(Guid Id) : IRequest<Result>;

public class DeleteMovieCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteMovieCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if(movie is not null)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("movie deleted!");
        }
        return Result.Failure("movie not found");
    }
}