using Application.Features.Users.Command;
using Application.Interface;
using Application.Entities;
using Moq;
using Infrastructure.Services;

namespace Tests;

public class IdentityServiceLoginTests
{
    [Fact]
    public async Task LoginAsync_Should_Return_Success_When_Credentials_Are_Valid()
    {
        // Arrange
        var user = new User 
        { 
            FullName = "Test User",
            Email = "test@example.com"
        };
        var command = new LoginUserCommand
        (
            Email: "test@example.com",
            Password: "Password123!"
        );

        var userManagerMock = MockUserManagerFactory.Create<User>(user);

        userManagerMock
            .Setup(um => um.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(um => um.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);

        var authTokenMock = new Mock<IJwtAuthTokenService>();

        authTokenMock
            .Setup(a => a.JwtTokenGenerator(user))
            .ReturnsAsync("fake-jwt-token");

        var service = new IdentityService(
            userManagerMock.Object,
            null!,
            authTokenMock.Object,
            null!, null!
        );

        // Act
        var result = await service.LoginAsync(command);

        // Assert
        Assert.True(true);
        Assert.Equal("login successful", result.Message);
        Assert.Equal("fake-jwt-token", result.Entity);
    }
}
