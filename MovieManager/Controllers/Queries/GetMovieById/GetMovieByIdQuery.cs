using MediatR;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers.Queries.GetMovieById;

public record GetMovieByIdQuery(Guid Id) : IRequest<Movie?>;
