using Application.Interface;
using Moq;
using Moq.EntityFrameworkCore;
using Application.Features.Movies;
using Application.Movies.Query;
using Domain.Entities;
using Tests.Helpers;

namespace Tests.Movies.Query;

public class GetAllMoviesQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly GetAllMoviesQueryHandler _handler;

    public GetAllMoviesQueryHandlerTests()
    {
        _context = MockAppDbContextFactory.Create();
        _handler = new GetAllMoviesQueryHandler(_context.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllMovies_AsMovieDtoList()
    {
        // Arrange
            var movieAEntity = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Movie A",
                Genre = "Drama",
                ReleaseYear = 2000,
                IsShowing = false
            };
            var movieBEntity = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Movie B",
                Genre = "Comedy",
                ReleaseYear = 2005,
                IsShowing = true
            };

        var movies = new List<Movie> { movieAEntity, movieBEntity };

        _context.Setup(c => c.Movies).ReturnsDbSet(movies);

        // Act
        var result = await _handler.Handle(new GetAllMoviesQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Status);

        var movieDtos = Assert.IsAssignableFrom<IEnumerable<MovieDto>>(result.Entity);
        var list = movieDtos.ToList();

        Assert.Equal(2, list.Count);

        var movieA = list.First(m => m.Title == "Movie A");
        Assert.Equal(movieAEntity.Id, movieA.Id);

        var movieB = list.First(m => m.Title == "Movie B");
        Assert.Equal(movieBEntity.Id, movieB.Id);
    }
}