using Application.Features.Users.RegisterUser;
using Application.Interface;
using Application.Profiles;
using Application.Validators;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieManager.ExceptionHandling;
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
        builder.Services.AddScoped<IMoviesDbContext, MoviesDbContext>();
        builder.Services.AddScoped<IUsersDbContext, UsersDbContext>();

        builder.Services.AddScoped<DispatchDomainEventsInterceptor>();
        builder.Services.AddDbContext<MovieManagerDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<DispatchDomainEventsInterceptor>();
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(interceptor);
        });
    }
}