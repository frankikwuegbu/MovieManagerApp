using Application.Features.Users.RegisterUser;
using Application.Profiles;
using Application.Validators;
using FluentValidation;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieManager.Data;
using MovieManager.ExceptionHandling;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Entities;
using MovieManager.Services;

namespace MovieManager.Startup;

public static class DependenciesConfig
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserCommandValidator).Assembly);

        builder.Services.AddControllers();

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<MovieManagerDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.UseJwtAuthenticationToken(builder.Configuration);
        builder.Services.AddAuthorization();

        builder.Services.AddScoped<JwtAuthTokenService>();

        builder.Services.AddMediatR(configuration =>{
            configuration.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);});

        builder.Services.AddAutoMapper(typeof(MappingProfiles));

        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandling>();
        builder.Services.AddProblemDetails();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGenServices();

        builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
        builder.Services.AddScoped<IMovieManagerRepository, MovieManagerRepository>();
        builder.Services.AddScoped<IMovieManagerUsersRepository, MovieManagerUsersRepository>();

        builder.Services.AddDbContext<MovieManagerDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }
}