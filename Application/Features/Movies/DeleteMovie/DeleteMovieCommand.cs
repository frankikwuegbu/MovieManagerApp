using MediatR;
using MovieManager.Models;

namespace MovieManager.Controllers.Commands.DeleteMovie;

public record DeleteMovieCommand(Guid Id) : IRequest<ApiResponse>;
