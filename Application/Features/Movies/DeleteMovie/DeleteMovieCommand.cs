using Domain;
using MediatR;

namespace Application.Features.Movies.DeleteMovie;

public record DeleteMovieCommand(Guid Id) : IRequest<ApiResponse>;
