using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Controllers.Commands.AddMovie;
using MovieManager.Controllers.Commands.DeleteMovie;
using MovieManager.Controllers.Commands.UpdateMovie;
using MovieManager.Controllers.Queries.GetAllMovies;
using MovieManager.Controllers.Queries.GetMovieById;
using MovieManager.Data;
using MovieManager.Models.Dtos;
using MovieManager.Models.Entities;
using MovieManager.Services;
using System.Threading.Tasks;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IEmailSenderService emailSender;
        private readonly ISender sender;

        public MoviesController(ISender sender, IEmailSenderService emailSender)
        {
            this.emailSender = emailSender;
            this.sender = sender;
        }

        //gets a list of all movies
        [HttpGet]
        public async Task<IActionResult> GetAllMovies(GetAllMoviesQuery getAllMoviesQuery)
        {
            var allMovies = await sender.Send(getAllMoviesQuery);
            return Ok(allMovies);
        }

        //get a movie by its id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            var movie = await sender.Send(new GetMovieByIdQuery(id));

            if(movie is null)
            {
                return NotFound("movie not found");
            }

            return Ok(movie);
        }

        //add movie
        [HttpPost]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<IActionResult> AddMovie(AddMovieCommand addMovieCommand)
        {
            var movie = await sender.Send(addMovieCommand);

            return Ok(movie);
        }

        //update movie
        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<IActionResult> UpdateMovie(Guid id, UpdateMovieDto updateMovieDto)
        {
            var command = new UpdateMovieCommand(
                id, 
                updateMovieDto.Genre,
                updateMovieDto.IsShowing
                );

            var updatedMovie = await sender.Send(command);

            if (updatedMovie is null)
            {
                return NotFound("movie not found");
            }

            return Ok(updatedMovie);
        }

        //delete movie
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var command = new DeleteMovieCommand(id);
            var result = await sender.Send(command);

            if (result is null)
            {
                return NotFound("movie not found");
            }

            return Ok();
        }
    }
}
