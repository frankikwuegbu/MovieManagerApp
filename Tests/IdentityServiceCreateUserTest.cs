using Application.Entities;
using Application.Features.Users;
using Application.Features.Users.Command;
using Application.Interface;
using AutoMapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Moq.EntityFrameworkCore;
using Tests.TestingHelpers;

namespace Tests;

public class IdentityServiceCreateUserTests
{
    [Fact]
    public async Task CreateUserAsync_WhenEmailDoesNotExist_CreatesUserAndReturnsSuccess()
    {
        // Arrange
        var command = new RegisterUserCommand
        (
            FullName: "Test User",
            UserName: "testuser",
            Email: "test@example.com",
            Password: "Password123!",
            Roles: UserRoles.USER
        );
        var password = "Password123!";

        // No existing users so, email is available
        var users = new List<User>();

        var mockContext = new Mock<IApplicationDbContext>();
        mockContext.Setup(c => c.Users).ReturnsDbSet(users);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        var userEntity = new User
        {
            FullName = command.FullName,
            Email = command.Email
        };

        var userDto = new UserDto
        {
            Id = Guid.NewGuid().ToString(),
            FullName = command.FullName
        };

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<User>(command)).Returns(userEntity);
        mockMapper.Setup(m => m.Map<UserDto>(userEntity)).Returns(userDto);

        var userManagerMock = MockUserManagerFactory.Create<User>();
        userManagerMock.Setup(u => u.CreateAsync(userEntity, password))
                       .ReturnsAsync(IdentityResult.Success);
        userManagerMock.Setup(u => u.AddToRoleAsync(userEntity, command.Roles.ToString()))
                       .ReturnsAsync(IdentityResult.Success);

        var emailSenderMock = new Mock<IEmailSenderService>();
        var tokenServiceMock = new Mock<IJwtAuthTokenService>();

        var service = new IdentityService(
            userManagerMock.Object,
            mockContext.Object,
            tokenServiceMock.Object,
            mockMapper.Object,
            emailSenderMock.Object
        );

        // Act
        var result = await service.CreateUserAsync(command, password);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("user created!", result.Message);
        Assert.Equal(userDto, result.Entity);

        userManagerMock.Verify(u => u.CreateAsync(userEntity, password), Times.Once);
        userManagerMock.Verify(u => u.AddToRoleAsync(userEntity, command.Roles.ToString()), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}