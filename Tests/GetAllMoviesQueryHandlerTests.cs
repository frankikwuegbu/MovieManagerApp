using Application.Features.Movies.Query;
using Application.Interface;
using Application.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;
using Application.Features.Movies;

namespace Tests;

public class GetAllMoviesQueryHandlerTests
{
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

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.Movies).ReturnsDbSet(movies);

        var handler = new GetAllMoviesQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetAllMoviesQuery(), CancellationToken.None);

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