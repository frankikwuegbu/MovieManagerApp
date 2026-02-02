using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieManager.Models.Entities;

namespace MovieManager.Data;

public class MovieManagerDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<Movie> Movies => Set<Movie>();
    public  DbSet<User> Users => Set<User>();
}
