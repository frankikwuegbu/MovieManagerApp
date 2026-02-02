using Application.Features.Movies.AddMovie;
using Application.Features.Movies.DeleteMovie;
using Application.Features.Movies.GetAllMovies;
using Application.Features.Movies.GetMovieById;
using Application.Features.Movies.UpdateMovie;
using Application.Interface;
using AutoMapper;
using Domain;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieManager.Data;
using MovieManager.Models.Entities;

namespace Infrastructure.DbContext;

public class MoviesDbContext(IMapper mapper,
    ILogger<MoviesDbContext> logger,
    MovieManagerDbContext dbContext) : BaseMovieManagerRepository(mapper, logger), IMoviesDbContext
{
    private readonly MovieManagerDbContext _dbContext = dbContext;

    //get all movies
    public async Task<ApiResponse> GetAllMoviesAsync(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all movies from database.");

        var movies = await _dbContext.Movies.ToListAsync(cancellationToken);

        return ApiResponse.Success("success!", movies);
    }

    //get movie by id
    public async Task<ApiResponse> GetMovieByIdAsync(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching movie with ID from database");

        var movie = await _dbContext.Movies.FirstOrDefaultAsync(a=>a.Id==request.Id, cancellationToken);

        if (movie is null)
        {
            return ApiResponse.Failure("movie not found!");
        }

        return ApiResponse.Success("movie found!", movie);
    }

    //add movie
    public async Task<ApiResponse> AddMovieAsync(AddMovieCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding a new movie to the database.");

        var movie = mapper.Map<Movie>(request);

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Success("movie added!", movie.Title);
    }

    //delete movie
    public async Task<ApiResponse> DeleteMovieAsync(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting movie with ID from database.");

        var movieInDb = _dbContext.Movies.Find(request.Id);

        if (movieInDb is null)
        {
            return ApiResponse.Failure("movie not found!");
        }

        _dbContext.Movies.Remove(movieInDb);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Success("deleted!");
    }

    //update movie
    public async Task<ApiResponse> UpdateMovieAsync(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating movie in database.");

        var movie = _dbContext.Movies.Find(request.Id);

        if (movie is null)
            return ApiResponse.Failure("movie not found!");

        mapper.Map(request, movie);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Success("update successful!");
    }
}