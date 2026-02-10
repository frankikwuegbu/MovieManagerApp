using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Entities;
using Application.Features.Movies.Command;
using Application.Features.Movies.Query;
using Application;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        //gets a list of all movies
        [HttpGet]
        public async Task<ActionResult<Result>> GetAllMovies([FromQuery] GetAllMoviesQuery request)
        {
            return await _sender.Send(request);
        }

        //get a movie by its id
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result>> GetMovieById(Guid id)
        {
            return await _sender.Send(new GetMovieByIdQuery(id));
        }

        //add movie
        [HttpPost]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<ActionResult<Result>> AddMovie(AddMovieCommand request)
        {
            return await _sender.Send(request);
        }

        //update movie
        [HttpPut("{id:guid}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<ActionResult<Result>> UpdateMovie(Guid id, UpdateMovieCommand r)
        {
            var request = new UpdateMovieCommand(
                id,
                r.Genre,
                r.IsShowing
                );

            return await _sender.Send(request);
        }

        //delete movie
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<ActionResult<Result>> DeleteMovie(Guid id)
        {
            return await _sender.Send(new DeleteMovieCommand(id));
        }
    }
}