using Application.Features.Movies.UpdateMovie;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieManager.Controllers.Commands.AddMovie;
using MovieManager.Controllers.Commands.DeleteMovie;
using MovieManager.Controllers.Queries.GetAllMovies;
using MovieManager.Controllers.Queries.GetMovieById;
using MovieManager.Data;
using MovieManager.Models;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Dtos;
using MovieManager.Models.Entities;

namespace Infrastructure.Repositories;

public class MovieManagerRepository : IMovieManagerRepository
{
    private readonly MovieManagerDbContext _dbContext;
    private readonly IMapper _mapper;

    public MovieManagerRepository(MovieManagerDbContext dbContext, IMapper maper)
    {
        _dbContext = dbContext;
        _mapper = maper;
    }

    //get all movies
    public async Task<ApiResponse> GetAllMoviesAsync(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await _dbContext.Movies.ToListAsync(cancellationToken);

        return new ApiResponse(true, "success!", movies);
    }

    //get movie by id
    public async Task<ApiResponse> GetMovieByIdAsync(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.FirstOrDefaultAsync(a=>a.Id==request.Id, cancellationToken);

        return new ApiResponse(true, "movie found!", movie);
    }

    //add movie
    public async Task<ApiResponse> AddMovieAsync(AddMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = _mapper.Map<Movie>(request);

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse(true, "movie added!");
    }

    //delete movie
    public async Task<ApiResponse> DeleteMovieAsync(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
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
        var movie = _dbContext.Movies.Find(request.Id);

        if (movie is null)
            return new ApiResponse(false, "movie not found!");

        _mapper.Map(request, movie);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse(true, "update successful!");
    }
}