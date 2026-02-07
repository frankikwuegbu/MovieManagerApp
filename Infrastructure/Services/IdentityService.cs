using Application.Dtos;
using Application.Features.Users.Command;
using Application.Interface;
using AutoMapper;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieManager.Services;

namespace Infrastructure.Services;

public class IdentityService(UserManager<User> userManager,
    IApplicationDbContext context,
    JwtAuthTokenService authToken,
    IMapper mapper) 
    : IIdentityService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IApplicationDbContext _context = context;
    private readonly JwtAuthTokenService _authToken = authToken;
    private readonly IMapper _mapper = mapper;

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

        await _userManager.CreateAsync(user, password);
        await _userManager.AddToRoleAsync(user, request.Roles.ToString());
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
