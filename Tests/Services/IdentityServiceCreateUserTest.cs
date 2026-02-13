using Application.Entities;
using Application.Features.Users;
using Application.Interface;
using Application.Users.Command;
using AutoMapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Moq.EntityFrameworkCore;
using Tests.Helpers;

namespace Tests.Services;

public class IdentityServiceCreateUserTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<UserManager<User>> _userManager;
    private readonly Mock<IJwtAuthTokenService> _authTokenService;
    private readonly IdentityService _identityService;

    public IdentityServiceCreateUserTests()
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

        var users = new List<User>();

        _context.Setup(c => c.Users).ReturnsDbSet(users);

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

        _mapper.Setup(m => m.Map<User>(command)).Returns(userEntity);
        _mapper.Setup(m => m.Map<UserDto>(userEntity)).Returns(userDto);

        _userManager.Setup(u => u.CreateAsync(userEntity, command.Password))
                       .ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(u => u.AddToRoleAsync(userEntity, command.Roles.ToString()))
                       .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _identityService.CreateUserAsync(command, command.Password);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("user created!", result.Message);
        Assert.Equal(userDto, result.Entity);

        _userManager.Verify(u => u.CreateAsync(userEntity, command.Password), Times.Once);
        _userManager.Verify(u => u.AddToRoleAsync(userEntity, command.Roles.ToString()), Times.Once);
        _context.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}