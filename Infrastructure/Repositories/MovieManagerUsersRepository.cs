using Application.Features.Users.LoginUser;
using Application.Features.Users.RegisterUser;
using Microsoft.AspNetCore.Identity;
using MovieManager.Models;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Entities;
using MovieManager.Services;

namespace MovieManager.Data;

public class MovieManagerUsersRepository : IMovieManagerUsersRepository
{
    private readonly MovieManagerDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly JwtAuthTokenService _authToken;
    private readonly IEmailSenderService _emailSender;

    public MovieManagerUsersRepository(MovieManagerDbContext dbContext,
        UserManager<User> userManager,
        JwtAuthTokenService authToken,
        IEmailSenderService emailSender)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _authToken = authToken;
        _emailSender = emailSender;
    }

    public async Task<ApiResponse> LoginUserAsync(LoginUserCommand request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, request.Password))

            return new ApiResponse(false, "invalid email address or password");

        var token = await _authToken.JwtTokenGenerator(user);

        return new ApiResponse(true, $"authentication token: {token}");
    }

    public async Task<ApiResponse> RegisterUserAsync(RegisterUserCommand request)
    {
        var user = new User
        {
            FullName = request.FullName,
            UserName = request.UserName,
            PasswordHash = request.Password,
            Role = request.Roles
        };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return new ApiResponse(false, "user registration failed!");

        await _userManager.AddToRoleAsync(user, request.Roles.ToString());

        var subject = "REGISTRATION COMPLETE";
        var message = $"{request.FullName} your registration is complete.\n" +
            $"You can now get tickets to see any movie of your choice";

        var emailSent = _emailSender.SendEmailAsync(request.UserName, subject, message);

        return new ApiResponse(true, "user registration successful!");
    }
}