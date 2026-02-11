using Application.Interface;
using AutoMapper;
using Application;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Features.Movies.Command;
public record AddMovieCommand(
    string Title,
    string Genre,
    int ReleaseYear,
    bool IsShowing
) : IRequest<Result>;
public class AddMovieCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<AddMovieCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var existingMovie = _context.Movies
            .AsNoTracking()
            .FirstOrDefault(m => m.Title == request.Title);

        if(existingMovie is not null)
        {
            return Result.Failure("oops! movie title already exists");
        }

        var movie = _mapper.Map<Movie>(request);
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync(cancellationToken);

        var movieDto = _mapper.Map<MovieDto>(movie);

        return Result.Success("movie successfully added!", movieDto);
    }
}