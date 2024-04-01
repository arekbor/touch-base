using Arekbor.TouchBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}