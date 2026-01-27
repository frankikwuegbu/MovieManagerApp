using MediatR;
using MovieManager.Models;

namespace Application.Features.Movies.UpdateMovie;

public record UpdateMovieCommand(
    Guid Id,
    string Genre,
    bool IsShowing
) : IRequest<ApiResponse>;