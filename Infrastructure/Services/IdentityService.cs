using Application.Features.Users.Command;
using Application.Interface;
using AutoMapper;
using Application;
using Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Events;
using Application.Features.Users;

namespace Infrastructure.Services;

public class IdentityService(UserManager<User> userManager,
    IApplicationDbContext context,
    IJwtAuthTokenService authToken,
    IMapper mapper,
    IEmailSenderService emailSender) 
    : IIdentityService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IApplicationDbContext _context = context;
    private readonly IJwtAuthTokenService _authToken = authToken;
    private readonly IMapper _mapper = mapper;
    private readonly IEmailSenderService _emailSender = emailSender;

    public async Task<Result> CreateUserAsync(RegisterUserCommand request, string password)
    {
        var existingUsers = _context.Users
           .AsNoTracking()
           .FirstOrDefault(u => u.Email == request.Email);

        if (existingUsers is not null)
        {
            return Result.Failure("email already exists");
        }

        var user = _mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return Result.Failure("user creation failed!");
        }

        await _userManager.AddToRoleAsync(user, request.Roles.ToString());

        user.AddDomainEvent(new UserRegisteredEvent(user));

        await _context.SaveChangesAsync();

        var userDto = _mapper.Map<UserDto>(user);

        return Result.Success("user created!", userDto);
    }

    public async Task<Result> LoginAsync(LoginUserCommand request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        var passwordCheck = await _userManager.CheckPasswordAsync(user, request.Password);

        if(user is null || passwordCheck is false)
        {
            return Result.Failure("incorrect email or password");
        }

        var token = await _authToken.JwtTokenGenerator(user);

        return Result.Success("login successful", token);
    }
}
