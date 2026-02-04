using Application.Features.Movies.AddMovie;
using Application.Features.Movies.GetAllMovies;
using Application.Features.Movies.UpdateMovie;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Movies.DeleteMovie;
using Application.Features.Movies.GetMovieById;
using MovieManager.Models.Dtos;
using Domain.Entities;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        //gets a list of all movies
        [HttpGet]
        public async Task<IActionResult> GetAllMovies(GetAllMoviesQuery getAllMoviesQuery)
        {
            var allMovies = await _sender.Send(getAllMoviesQuery);
            return Ok(allMovies);
        }

        //get a movie by its id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            var movie = await _sender.Send(new GetMovieByIdQuery(id));
            return Ok(movie);
        }

        //add movie
        [HttpPost]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<IActionResult> AddMovie(AddMovieCommand request)
        {
            var movie = await _sender.Send(request);
            return Ok(movie);
        }

        //update movie
        [HttpPut("{id:guid}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<IActionResult> UpdateMovie(Guid id, UpdateMovieDto updateMovieDto)
        {
            var request = new UpdateMovieCommand(
                id,
                updateMovieDto.Genre,
                updateMovieDto.IsShowing
                );

            var updatedMovie = await _sender.Send(request);
            return Ok(updatedMovie);
        }

        //delete movie
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var command = new DeleteMovieCommand(id);
            var result = await _sender.Send(command);
            return Ok(result);
        }
    }
}