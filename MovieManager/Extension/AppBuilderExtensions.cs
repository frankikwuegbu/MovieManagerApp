using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace MovieManager.Extension;

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
