using Application.Features.Movies.Query;
using Application.Interface;
using Application.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using AutoMapper;
using Application.Features.Movies;

namespace Tests;

public class GetMovieByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenMovieExists_ReturnsSuccessResultWithMovieDto()
    {
        // Arrange
        var movieId = Guid.NewGuid();

        var movie = new Movie
        {
            Id = movieId,
            Title = "Movie A",
            Genre = "Drama",
            ReleaseYear = 2000,
            IsShowing = true
        };

        var movies = new List<Movie> { movie };

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.Movies).ReturnsDbSet(movies);

        var movieDto = new MovieByIdDto
        {
            Id = movieId,
            Title = "Movie A",
                Genre = "Drama",
                ReleaseYear = 2000,
                IsShowing = true
        };

        var mockMapper = new Mock<IMapper>();
        mockMapper
            .Setup(m => m.Map<MovieByIdDto>(movie))
            .Returns(movieDto);

        var handler = new GetMovieByIdQueryHandler(
            mockContext.Object,
            mockMapper.Object
        );

        var query = new GetMovieByIdQuery(movieId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("movie found", result.Message);
        Assert.Equal(movieDto, result.Entity);
    }
}