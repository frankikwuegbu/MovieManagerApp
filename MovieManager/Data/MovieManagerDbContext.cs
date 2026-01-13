using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieManager.Models.Entities;

namespace MovieManager.Data;

public class MovieManagerDbContext : IdentityDbContext<User>
{
    public MovieManagerDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public  DbSet<User> Users => Set<User>();
}
