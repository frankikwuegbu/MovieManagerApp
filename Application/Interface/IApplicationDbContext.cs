using Application.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.Interface;

public interface IApplicationDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
