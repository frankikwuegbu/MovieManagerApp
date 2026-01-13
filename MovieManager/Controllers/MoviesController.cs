using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Data;
using MovieManager.Models.Dtos;
using MovieManager.Models.Entities;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieManagerDbContext dbContext;

        public MoviesController(MovieManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //gets a list of all movies
        [HttpGet]
        public IActionResult GetAllMovies()
        {
            var allMovies = dbContext.Movies.ToList();
            return Ok(allMovies);
        }

        //get a movie by its id
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetMovieById(Guid id)
        {
            var movie = dbContext.Movies.Find(id);

            if (movie is null )
            {
                return NotFound();
            }
            return Ok(movie);
        }

        //add movie
        [HttpPost]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public IActionResult AddMovie(AddMovieDto addMovieDto)
        {
            var movie = new Movie()
            {
                Title = addMovieDto.Title,
                Genre = addMovieDto.Genre,
                ReleaseYear = addMovieDto.ReleaseYear,
                IsShowing = addMovieDto.IsShowing,
            };

            dbContext.Movies.Add(movie);
            dbContext.SaveChanges();

            return Ok(movie);
        }

        //update movie
        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public IActionResult UpdateMovie(Guid id, UpdateMovieDto updateMovieDto)
        {
            var movie = dbContext.Movies.Find(id);
            if (movie is null)
            {
                return NotFound();
            }

            movie.Genre = updateMovieDto.Genre;
            movie.IsShowing = updateMovieDto.IsShowing;

            dbContext.SaveChanges();

            return Ok(movie);
        }

        //delete movie
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public IActionResult DeleteMovie(Guid id)
        {
            var movie = dbContext.Movies.Find(id);

            if (movie is null)
            {
                return NotFound();
            }

            dbContext.Movies.Remove(movie);
            dbContext.SaveChanges();

            return Ok();
        }
    }
}
