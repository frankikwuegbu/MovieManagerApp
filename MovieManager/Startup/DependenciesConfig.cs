using Application.Features.Users.Command;
using Application.Interface;
using Application.Profiles;
using Application.Validators;
using Application.Entities;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieManager.ExceptionHandling;

namespace MovieManager.Startup;

public static class DependenciesConfig
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserCommandValidator).Assembly);

        builder.Services.AddControllers();

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddScoped<IIdentityService, IdentityService>();

        builder.Services.AddIdentityCore<User>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.UseJwtAuthenticationToken(builder.Configuration);
        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IJwtAuthTokenService, JwtAuthTokenService>();

        builder.Services.AddMediatR(configuration =>{
            configuration.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly);});

        builder.Services.AddAutoMapper(typeof(MappingProfiles));

        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandling>();
        builder.Services.AddProblemDetails();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGenServices();

        builder.Services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<DispatchDomainEventsInterceptor>();
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(interceptor);
        });
        builder.Services.AddScoped<DispatchDomainEventsInterceptor>();

        builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
    }
}