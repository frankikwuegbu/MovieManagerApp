using Application.Features.Movies.UpdateMovie;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieManager.Controllers.Commands.AddMovie;
using MovieManager.Controllers.Commands.DeleteMovie;
using MovieManager.Controllers.Queries.GetAllMovies;
using MovieManager.Controllers.Queries.GetMovieById;
using MovieManager.Data;
using MovieManager.Models;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Entities;

namespace Infrastructure.Repositories;

public class MovieManagerRepository : IMovieManagerRepository
{
    private readonly MovieManagerDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<MovieManagerRepository> _logger;

    public MovieManagerRepository(MovieManagerDbContext dbContext, IMapper maper, ILogger<MovieManagerRepository> logger)
    {
        _dbContext = dbContext;
        _mapper = maper;
        _logger = logger;
    }

    //get all movies
    public async Task<ApiResponse> GetAllMoviesAsync(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all movies from database.");

        var movies = await _dbContext.Movies.ToListAsync(cancellationToken);

        return new ApiResponse(true, "success!", movies);
    }

    //get movie by id
    public async Task<ApiResponse> GetMovieByIdAsync(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching movie with ID from database");

        var movie = await _dbContext.Movies.FirstOrDefaultAsync(a=>a.Id==request.Id, cancellationToken);

        if (movie is null)
        {
            return new ApiResponse(false, "movie not found!");
        }

        return new ApiResponse(true, "movie found!", movie);
    }

    //add movie
    public async Task<ApiResponse> AddMovieAsync(AddMovieCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding a new movie to the database.");

        var movie = _mapper.Map<Movie>(request);

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse(true, "movie added!");
    }

    //delete movie
    public async Task<ApiResponse> DeleteMovieAsync(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting movie with ID from database.");

        var movieInDb = _dbContext.Movies.Find(request.Id);

        if (movieInDb is null)
        {
            return new ApiResponse(false, "movie not found!");
        }

        _dbContext.Movies.Remove(movieInDb);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse(true, "deleted!");
    }

    //update movie
    public async Task<ApiResponse> UpdateMovieAsync(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating movie in database.");

        var movie = _dbContext.Movies.Find(request.Id);

        if (movie is null)
            return new ApiResponse(false, "movie not found!");

        _mapper.Map(request, movie);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse(true, "update successful!");
    }
}