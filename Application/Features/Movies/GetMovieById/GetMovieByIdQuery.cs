using Domain;
using MediatR;
using MovieManager.Models.Entities;

namespace Application.Features.Movies.GetMovieById;

public record GetMovieByIdQuery(Guid Id) : IRequest<ApiResponse?>;