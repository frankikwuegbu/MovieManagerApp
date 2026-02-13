using Application.Interface;
using Application.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<User>(options), IApplicationDbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    public  DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}