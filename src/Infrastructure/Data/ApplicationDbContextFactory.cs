using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = "Server=(localdb)\\MSSQLLocalDB;" +
            "Database=MovieManagerDb;" +
            "Trusted_Connection=true;" +
            "TrustServerCertificate=true;";

        optionsBuilder.UseSqlServer(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}