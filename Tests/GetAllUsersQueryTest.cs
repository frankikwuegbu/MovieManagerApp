using Application.Entities;
using Application.Features.Users;
using Application.Interface;
using Application.Users.Query;
using Moq;
using Moq.EntityFrameworkCore;

namespace Tests;

public class GetAllUsersQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenUsersExist_ReturnsUserDtoList()
    {
        // Arrange
        var userAEntity = new User
        {
            Id = "1",
            FullName = "John Doe",
            UserName = "johndoe",
            Email = "x@email.com",
            Role = UserRoles.USER,
        };
        var userBEntity = new User
        {
            Id = "2",
            FullName = "Jane Smith",
            UserName = "janesmith",
            Email = "j@email.com",
            Role = UserRoles.ADMIN,
        };

        var users = new List<User> { userAEntity, userBEntity };

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        var handler = new GetAllUsersQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetAllUsersQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("success!", result.Message);

        var userDtos = Assert.IsAssignableFrom<List<UserDto>>(result.Entity);
        var list = userDtos.ToList();

        Assert.Equal(2, userDtos.Count);

        var userA = list.First(u => u.FullName == "John Doe");
        Assert.Equal(userAEntity.Id, userA.Id);

        var userB = list.First(u => u.FullName == "Jane Smith");
        Assert.Equal(userBEntity.Id, userB.Id);
    }
}