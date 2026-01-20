using MediatR;
using MovieManager.Models;

namespace MovieManager.Controllers.Commands.AddMovie;

public record AddMovieCommand(
    string Title,
    string Genre,
    int ReleaseYear,
    bool IsShowing
) : IRequest<ApiResponse>;