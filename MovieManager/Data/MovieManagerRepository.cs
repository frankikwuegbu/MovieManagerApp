using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieManager.Controllers.Commands.AddMovie;
using MovieManager.Controllers.Commands.DeleteMovie;
using MovieManager.Controllers.Commands.UpdateMovie;
using MovieManager.Controllers.Queries.GetAllMovies;
using MovieManager.Controllers.Queries.GetMovieById;
using MovieManager.Models;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Entities;

namespace MovieManager.Data;

public class MovieManagerRepository : IMovieManagerRepository
{
    private readonly MovieManagerDbContext _dbContext;

    public MovieManagerRepository(MovieManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //get all movies
    public async Task<List<Movie>> GetAllMoviesAsync(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = _dbContext.Movies.ToList();

        return movies;
    }

    //get movie by id
    public async Task<Movie> GetMovieByIdAsync(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.FindAsync(request.Id, cancellationToken);

        return movie;
    }
    //add movie
    public async Task<ApiResponse> AddMovieAsync(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = new Movie()
        {
            Title = request.Title,
            Genre = request.Genre,
            ReleaseYear = request.ReleaseYear,
            IsShowing = request.IsShowing
        };

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse
        {
            Success = true,
            Message = "Movie added successfully"
        };
    }
    //delete movie
    public async Task<ApiResponse> DeleteMovieAsync(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movieInDb = _dbContext.Movies.Find(request.Id);

        if (movieInDb is null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Movie not found!"
            };
        }

        _dbContext.Movies.Remove(movieInDb);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse
        {
            Success = true,
            Message = "Movie deleted!"
        };
    }
    //update movie
    public async Task<ApiResponse> UpdateMovieAsync(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = _dbContext.Movies.Find(request.Id);

        if (movie is null)
            return new ApiResponse
            {
                Success = false,
                Message = "Movie not found!"
            };

        movie.Genre = request.Genre;
        movie.IsShowing = request.IsShowing;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse
        {
            Success = true,
            Message = "Movie update successful!"
        };
    }
}
