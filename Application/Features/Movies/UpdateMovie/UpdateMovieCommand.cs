using Domain;
using MediatR;

namespace Application.Features.Movies.UpdateMovie;

public record UpdateMovieCommand(
    Guid Id,
    string Genre,
    bool IsShowing
) : IRequest<ApiResponse>;