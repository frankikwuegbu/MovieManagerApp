using Domain;
using MediatR;
using MovieManager.Models.Entities;

namespace Application.Features.Movies.GetAllMovies;

public record GetAllMoviesQuery() : IRequest<ApiResponse>;
