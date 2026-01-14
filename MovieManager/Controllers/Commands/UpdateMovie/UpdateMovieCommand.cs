using MediatR;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers.Commands.UpdateMovie;

public record UpdateMovieCommand(
    Guid Id,
    string Genre,
    bool IsShowing
) : IRequest<Movie?>;
