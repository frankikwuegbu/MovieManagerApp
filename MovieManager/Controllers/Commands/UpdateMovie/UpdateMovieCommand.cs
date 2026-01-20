using MediatR;
using MovieManager.Models;

namespace MovieManager.Controllers.Commands.UpdateMovie;

public record UpdateMovieCommand(
    Guid Id,
    string Genre,
    bool IsShowing
) : IRequest<ApiResponse>;