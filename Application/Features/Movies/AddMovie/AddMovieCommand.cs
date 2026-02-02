using Domain;
using MediatR;

namespace Application.Features.Movies.AddMovie;

public record AddMovieCommand(
    string Title,
    string Genre,
    int ReleaseYear,
    bool IsShowing
) : IRequest<ApiResponse>;