using Application.Features.Users.Command;
using Application.Interface;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Events;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieManager.Services;

namespace Infrastructure.Data;

public class UsersDbContext(IMapper mapper, 
    ILogger<UsersDbContext> logger,
    UserManager<User> userManager,
    JwtAuthTokenService authToken,
    IEmailSenderService emailSender,
    MovieManagerDbContext dbContext) : BaseMovieManagerRepository(mapper, logger), IUsersDbContext
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly JwtAuthTokenService _authToken = authToken;
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly MovieManagerDbContext _dbContext = dbContext;

    public async Task<ApiResponse> LoginUserAsync(LoginUserCommand request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, request.Password))

            return ApiResponse.Failure("invalid email address or password");

        var token = await _authToken.JwtTokenGenerator(user);

        return ApiResponse.Success("login successful", token);
    }

    public async Task<ApiResponse> RegisterUserAsync(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return ApiResponse.Failure("user registration failed!");

        await _userManager.AddToRoleAsync(user, request.Roles.ToString());

        user.AddDomainEvent(new UserRegisteredEvent(user.FullName, user.UserName, user.Role));

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Success("user registration successful!");
    }
}