using Application.Features.Users.LoginUser;
using Application.Features.Users.RegisterUser;
using AutoMapper;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieManager.Models;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Entities;
using MovieManager.Services;

namespace Infrastructure.Repositories;

public class MovieManagerUsersRepository(IMapper mapper, 
    ILogger<MovieManagerUsersRepository> logger,
    UserManager<User> userManager,
    JwtAuthTokenService authToken,
    IEmailSenderService emailSender) : BaseMovieManagerRepository(mapper, logger), IMovieManagerUsersRepository
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly JwtAuthTokenService _authToken = authToken;
    private readonly IEmailSenderService _emailSender = emailSender;

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
        var user = mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return new ApiResponse(false, "user registration failed!");

        await _userManager.AddToRoleAsync(user, request.Roles.ToString());

        var subject = "REGISTRATION COMPLETE";
        var message = $"{request.FullName} your registration is complete.\n" +
            $"You can now get tickets to see any movie of your choice";

        await _emailSender.SendEmailAsync(request.UserName, subject, message);

        return new ApiResponse(true, "user registration successful!");
    }
}