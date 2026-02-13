using Application.Entities;
using Application.Features.Users.Command;
using Application.Interface;
using AutoMapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Tests.Helpers;

namespace Tests.Services;

public class IdentityServiceLoginTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<UserManager<User>> _userManager;
    private readonly Mock<IJwtAuthTokenService> _authTokenService;
    private readonly IdentityService _identityService;

    public IdentityServiceLoginTests()
    {
        _context = MockAppDbContextFactory.Create();
        _mapper = new Mock<IMapper>();
        _userManager = MockUserManagerFactory.Create<User>();
        _authTokenService = new Mock<IJwtAuthTokenService>();
        _identityService = new IdentityService
            (_userManager.Object,
            _context.Object,
            _authTokenService.Object,
            _mapper.Object);
    }

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

        _userManager.Setup(um => um.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManager.Setup(um => um.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);

        _authTokenService.Setup(a => a.JwtTokenGenerator(user))
            .ReturnsAsync("fake-jwt-token");

        // Act
        var result = await _identityService.LoginAsync(command);

        // Assert
        Assert.True(true);
        Assert.Equal("login successful", result.Message);
        Assert.Equal("fake-jwt-token", result.Entity);
    }
}