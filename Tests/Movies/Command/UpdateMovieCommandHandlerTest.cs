using Application.Features.Movies;
using Application.Features.Movies.Command;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Tests.Helpers;

namespace Tests.Movies.Command;

public class UpdateMovieCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IMapper> _mapper;
    private readonly UpdateMovieCommandHandler _handler;

    public UpdateMovieCommandHandlerTests()
    {
        _context = MockAppDbContextFactory.Create();
        _mapper = new Mock<IMapper>();

        _handler = new UpdateMovieCommandHandler(
            _context.Object,
            _mapper.Object
        );
    }

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

        _context.Setup(c => c.Movies).ReturnsDbSet(movies);

        var updatedDto = new MovieDto
        {
            Id = movieId,
            Title = movie.Title
        };

        _mapper.Setup(m => m.Map(command, movie));

        _mapper.Setup(m => m.Map<MovieDto>(movie))
            .Returns(updatedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("update successful!", result.Message);
        Assert.Equal(updatedDto, result.Entity);

        _mapper.Verify(m => m.Map(command, movie), Times.Once);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}