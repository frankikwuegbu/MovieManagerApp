using MediatR;

namespace MovieManager.Controllers.Commands.DeleteMovie;

public record DeleteMovieCommand(Guid Id) : IRequest<string?>;
