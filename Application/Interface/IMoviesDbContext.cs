using Application.Features.Movies.Command;
using Application.Features.Movies.Query;
using Domain;
using MovieManager.Models.Entities;

namespace Application.Interface;

public interface IMoviesDbContext
{
    public Task<ApiResponse> AddMovieAsync(AddMovieCommand request, CancellationToken cancellationToken);
    public Task<ApiResponse> DeleteMovieAsync(DeleteMovieCommand request, CancellationToken cancellationToken);
    public Task<ApiResponse> UpdateMovieAsync(UpdateMovieCommand request, CancellationToken cancellationToken);
    public Task<ApiResponse> GetAllMoviesAsync(GetAllMoviesQuery request, CancellationToken cancellationToken);
    public Task<ApiResponse> GetMovieByIdAsync(GetMovieByIdQuery request, CancellationToken cancellationToken);
}