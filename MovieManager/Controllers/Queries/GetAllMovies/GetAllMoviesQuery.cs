using MediatR;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers.Queries.GetAllMovies;

public record GetAllMoviesQuery() : IRequest<List<Movie>>;
