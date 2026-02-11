using Application.Entities;
using Application.Features.Users;
using Application.Interface;
using Application.Users.Query;
using AutoMapper;
using Moq;
using Moq.EntityFrameworkCore;

namespace Tests;

public class GetUserByIdQueryHandlerTests
{
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

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        var userDto = new UserByIdDto
        {
            Id = userId,
            FullName = "John Doe",
            UserName = "johndoe",
            Email = "j@email.com",
            Password = "password123"
        };

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<UserByIdDto>(users[0]))
                  .Returns(userDto);

        var handler = new GetUserByIdQueryHandler(
            mockContext.Object,
            mockMapper.Object
        );

        // Act
        var result = await handler.Handle(
            new GetUserByIdQuery(userId),
            CancellationToken.None
        );

        // Assert
        Assert.True(result.Status);
        Assert.Equal("user found!", result.Message);
        Assert.Equal(userDto, result.Entity);
    }
}