using Application.Features.Movies.Query;
using Application.Interface;
using Moq;
using Moq.EntityFrameworkCore;
using AutoMapper;
using Application.Features.Movies;
using Domain.Entities;
using Tests.Helpers;

namespace Tests.Movies.Query;

public class GetMovieByIdQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IMapper> _mapper;
    private readonly GetMovieByIdQueryHandler _handler;

    public GetMovieByIdQueryHandlerTests()
    {
        _context = MockAppDbContextFactory.Create();
        _mapper = new Mock<IMapper>();
        _handler = new GetMovieByIdQueryHandler(_context.Object, _mapper.Object);
    }

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

        _context.Setup(c => c.Movies).ReturnsDbSet(movies);

        var movieDto = new MovieByIdDto
        {
            Id = movieId,
            Title = "Movie A",
                Genre = "Drama",
                ReleaseYear = 2000,
                IsShowing = true
        };

        _mapper.Setup(m => m.Map<MovieByIdDto>(movie))
            .Returns(movieDto);

        var query = new GetMovieByIdQuery(movieId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("movie found", result.Message);
        Assert.Equal(movieDto, result.Entity);
    }
}