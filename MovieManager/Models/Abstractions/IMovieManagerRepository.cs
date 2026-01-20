using MovieManager.Controllers.Commands.AddMovie;
using MovieManager.Controllers.Commands.DeleteMovie;
using MovieManager.Controllers.Commands.UpdateMovie;
using MovieManager.Controllers.Queries.GetAllMovies;
using MovieManager.Controllers.Queries.GetMovieById;
using MovieManager.Models.Entities;

namespace MovieManager.Models.Abstractions;

public interface IMovieManagerRepository
{
    public Task<ApiResponse> AddMovieAsync(AddMovieCommand request, CancellationToken cancellationToken);
    public Task<ApiResponse> DeleteMovieAsync(DeleteMovieCommand request, CancellationToken cancellationToken);
    public Task<ApiResponse> UpdateMovieAsync(UpdateMovieCommand request, CancellationToken cancellationToken);
    public Task<List<Movie>> GetAllMoviesAsync(GetAllMoviesQuery request, CancellationToken cancellationToken);
    public Task<Movie> GetMovieByIdAsync(GetMovieByIdQuery request, CancellationToken cancellationToken);
}