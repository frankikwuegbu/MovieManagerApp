using Microsoft.AspNetCore.Identity;

namespace MovieManager.Startup;

public static class AppBuilderExtensions
{
    public static async Task IdentityRoleSeedingAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "ADMIN", "USER" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}