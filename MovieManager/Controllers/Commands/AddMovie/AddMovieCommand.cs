using MediatR;
using MovieManager.Models.Dtos;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers.Commands.AddMovie;

public record AddMovieCommand(
    string Title,
    string Genre,
    int ReleaseYear,
    bool IsShowing
) : IRequest<Movie>;