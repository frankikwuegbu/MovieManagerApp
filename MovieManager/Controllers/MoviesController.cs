using Application.Features.Movies.UpdateMovie;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Controllers.Commands.AddMovie;
using MovieManager.Controllers.Commands.DeleteMovie;
using MovieManager.Controllers.Queries.GetAllMovies;
using MovieManager.Controllers.Queries.GetMovieById;
using MovieManager.Models.Dtos;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public MoviesController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        //gets a list of all movies
        [HttpGet]
        public async Task<IActionResult> GetAllMovies(GetAllMoviesQuery getAllMoviesQuery)
        {
            var allMovies = await _sender.Send(getAllMoviesQuery);
            return Ok(allMovies);
        }

        //get a movie by its id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            var movie = await _sender.Send(new GetMovieByIdQuery(id));

            if(movie is null)
            {
                return NotFound("movie not found");
            }

            return Ok(movie);
        }

        //add movie
        [HttpPost]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<IActionResult> AddMovie(AddMovieDto addMovieDto)
        {
            var request = _mapper.Map<AddMovieCommand>(addMovieDto);
            var movie = await _sender.Send(request);

            return Ok(movie);
        }

        //update movie
        [HttpPut]
        [Route("{id:guid}")]
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
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var command = new DeleteMovieCommand(id);
            var result = await _sender.Send(command);

            return Ok(result);
        }
    }
}