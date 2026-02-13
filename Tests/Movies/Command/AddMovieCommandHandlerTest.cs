using Application.Features.Movies;
using Application.Features.Movies.Command;
using Application.Interface;
using AutoMapper;
using Domain.Entities;
using Moq;
using Tests.Helpers;

namespace Tests.Movies.Command;

public class AddMovieCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IMapper> _mapper;
    private readonly AddMovieCommandHandler _handler;

    public AddMovieCommandHandlerTests()
    {
        _context = MockAppDbContextFactory.Create();
        _mapper = new Mock<IMapper>();

        _handler = new AddMovieCommandHandler(
            _context.Object,
            _mapper.Object
        );
    }

    [Fact]
    public async Task Handle_WhenTitleDoesNotExist_AddsMovieAndReturnsSuccess()
    {
        // Arrange
        var command = new AddMovieCommand(
            Title: "New Movie",
            Genre: "Action",
            ReleaseYear: 2024,
            IsShowing: true
        );

        var movies = new List<Movie>
        {
            new Movie { Id = Guid.NewGuid(), Title = "Movie A" },
            new Movie { Id = Guid.NewGuid(), Title = "Movie B" }
        };

        MockAppDbContextFactory.SetupDbSet(
            _context,
            c => c.Movies,
            movies
        );

        var mappedMovie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Genre = command.Genre,
            ReleaseYear = command.ReleaseYear,
            IsShowing = command.IsShowing
        };

        var movieDto = new MovieDto
        {
            Id = mappedMovie.Id,
            Title = mappedMovie.Title
        };

        _mapper.Setup(m => m.Map<Movie>(command))
               .Returns(mappedMovie);

        _mapper.Setup(m => m.Map<MovieDto>(mappedMovie))
               .Returns(movieDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("movie successfully added!", result.Message);
        Assert.Equal(movieDto, result.Entity);

        _context.Verify(c => c.Movies.Add(mappedMovie), Times.Once);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}