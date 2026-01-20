using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieManager.Data;
using MovieManager.Models.Abstractions;
using MovieManager.Models.Entities;
using MovieManager.Services;

namespace MovieManager.Startup;

public static class DependenciesConfig
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGenServices();

        builder.Services.AddAuthorization();
        builder.Services.UseJwtAuthenticationToken(builder.Configuration);
        builder.Services.AddScoped<JwtAuthTokenService>();

        builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
        builder.Services.AddScoped<IMovieManagerRepository, MovieManagerRepository>();

        builder.Services.AddDbContext<MovieManagerDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddMediatR(configuration =>{
        configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);});

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<MovieManagerDbContext>()
            .AddDefaultTokenProviders();
    }
}
