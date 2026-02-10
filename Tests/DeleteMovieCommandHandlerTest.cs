using Application.Features.Movies.Command;
using Application.Interface;
using Application.Entities;
using Moq;
using Moq.EntityFrameworkCore;

namespace Tests;

public class DeleteMovieCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenMovieExists_DeletesMovieAndReturnsSuccess()
    {
        // Arrange
        var movieId = Guid.NewGuid();

        var movie = new Movie
        {
            Id = movieId,
            Title = "Movie A"
        };

        var movies = new List<Movie> { movie };

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.Movies).ReturnsDbSet(movies);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        var handler = new DeleteMovieCommandHandler(mockContext.Object);
        var command = new DeleteMovieCommand(movieId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("movie deleted!", result.Message);

        mockContext.Verify(c => c.Movies.Remove(movie), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}