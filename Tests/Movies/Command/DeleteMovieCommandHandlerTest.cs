using Application.Features.Movies.Command;
using Application.Interface;
using Moq;
using Moq.EntityFrameworkCore;
using Domain.Entities;
using Tests.Helpers;

namespace Tests.Movies.Command;

public class DeleteMovieCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly DeleteMovieCommandHandler _handler;

    public DeleteMovieCommandHandlerTests()
    {
        _context = MockAppDbContextFactory.Create();
        _handler = new DeleteMovieCommandHandler(_context.Object);
    }

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

        _context.Setup(c => c.Movies).ReturnsDbSet(movies);

        var command = new DeleteMovieCommand(movieId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("movie deleted!", result.Message);

        _context.Verify(c => c.Movies.Remove(movie), Times.Once);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}