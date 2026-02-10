using Application.Features.Movies.Command;
using Application.Interface;
using Application.Dtos;
using Application.Entities;
using AutoMapper;
using Moq;
using Moq.EntityFrameworkCore;

namespace Tests;

public class UpdateMovieCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenMovieExists_UpdatesMovieAndReturnsSuccess()
    {
        // Arrange
        var movieId = Guid.NewGuid();

        var movie = new Movie
        {
            Id = movieId,
            Title = "Old Title",
            Genre = "Drama",
            ReleaseYear = 2000,
            IsShowing = false
        };

        var movies = new List<Movie> { movie };

        var command = new UpdateMovieCommand
        (
            Id: movieId,
            Genre: "Action",
            IsShowing: true
        );

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.Movies).ReturnsDbSet(movies);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        var updatedDto = new MovieDto
        {
            Id = movieId,
            Title = movie.Title
        };

        var mockMapper = new Mock<IMapper>();
        mockMapper
            .Setup(m => m.Map(command, movie));

        mockMapper
            .Setup(m => m.Map<MovieDto>(movie))
            .Returns(updatedDto);

        var handler = new UpdateMovieCommandHandler(
            mockContext.Object,
            mockMapper.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("update successful!", result.Message);
        Assert.Equal(updatedDto, result.Entity);

        mockMapper.Verify(m => m.Map(command, movie), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}