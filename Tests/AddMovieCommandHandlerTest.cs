using Application.Features.Movies.Command;
using Application.Interface;
using Application.Entities;
using AutoMapper;
using Moq;
using Moq.EntityFrameworkCore;
using Application.Features.Movies;

namespace Tests;

public class AddMovieCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenTitleDoesNotExist_AddsMovieAndReturnsSuccess()
    {
        // Arrange
        var command = new AddMovieCommand
        (
            Title: "New Movie",
            Genre: "Action",
            ReleaseYear: 2024,
            IsShowing: true
        );

        var movies = new List<Movie>
        {
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Movie A",
                Genre = "Drama",
                ReleaseYear = 2000,
                IsShowing = false
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Movie B",
                Genre = "Comedy",
                ReleaseYear = 2005,
                IsShowing = true
            }
        };

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.Movies).ReturnsDbSet(movies);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

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

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<Movie>(command)).Returns(mappedMovie);
        mockMapper.Setup(m => m.Map<MovieDto>(mappedMovie)).Returns(movieDto);

        var handler = new AddMovieCommandHandler(
            mockContext.Object,
            mockMapper.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("movie successfully added!", result.Message);
        Assert.Equal(movieDto, result.Entity);

        mockContext.Verify(c => c.Movies.Add(mappedMovie), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}