using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieManager.Models.Entities;

namespace Infrastructure.Data;

public class MovieManagerDbContext(DbContextOptions<MovieManagerDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Movie> Movies => Set<Movie>();
    public  DbSet<User> Users => Set<User>();
}