using Application.Entities;
using Application.Features.Users;
using Application.Interface;
using Application.Users.Query;
using AutoMapper;
using Moq;
using Moq.EntityFrameworkCore;
using Tests.Helpers;

namespace Tests.Users.Query;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IMapper> _mapper;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _context = MockAppDbContextFactory.Create();
        _mapper = new Mock<IMapper>();
        _handler = new GetUserByIdQueryHandler(_context.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ReturnsUserDto()
    {
        // Arrange
        var userId = "1";

        var users = new List<User>
        {
            new User
            {
                Id = userId,
                FullName = "John Doe",
                Email = "john@example.com"
            }
        };

        _context.Setup(c => c.Users).ReturnsDbSet(users);

        var userDto = new UserByIdDto
        {
            Id = userId,
            FullName = "John Doe",
            UserName = "johndoe",
            Email = "j@email.com",
            Password = "password123"
        };

        _mapper.Setup(m => m.Map<UserByIdDto>(users[0]))
                  .Returns(userDto);

        // Act
        var result = await _handler.Handle(
            new GetUserByIdQuery(userId),
            CancellationToken.None
        );

        // Assert
        Assert.True(result.Status);
        Assert.Equal("user found!", result.Message);
        Assert.Equal(userDto, result.Entity);
    }
}