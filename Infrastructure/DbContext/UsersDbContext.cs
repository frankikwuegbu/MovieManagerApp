using Application.Features.Users.LoginUser;
using Application.Features.Users.RegisterUser;
using Application.Interface;
using AutoMapper;
using Domain;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Entities;
using MovieManager.Services;

namespace Infrastructure.DbContext;

public class UsersDbContext(IMapper mapper, 
    ILogger<UsersDbContext> logger,
    UserManager<User> userManager,
    JwtAuthTokenService authToken,
    IEmailSenderService emailSender) : BaseMovieManagerRepository(mapper, logger), IUsersDbContext
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly JwtAuthTokenService _authToken = authToken;
    private readonly IEmailSenderService _emailSender = emailSender;

    public async Task<ApiResponse> LoginUserAsync(LoginUserCommand request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, request.Password))

            return ApiResponse.Failure("invalid email address or password");

        var token = await _authToken.JwtTokenGenerator(user);

        return ApiResponse.Success("login successful", token);
    }

    public async Task<ApiResponse> RegisterUserAsync(RegisterUserCommand request)
    {
        var user = mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return ApiResponse.Failure("user registration failed!");

        await _userManager.AddToRoleAsync(user, request.Roles.ToString());

        var subject = "REGISTRATION COMPLETE";
        var message = $"{request.FullName} your registration is complete.\n" +
            $"You can now get tickets to see any movie of your choice";

        await _emailSender.SendEmailAsync(request.UserName, subject, message);

        return ApiResponse.Success("user registration successful!");
    }
}