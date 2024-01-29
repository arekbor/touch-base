using Arekbor.TouchBase.Domain.Common;
using Arekbor.TouchBase.Domain.Entities;
using Arekbor.TouchBase.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {}

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach(var entry in ChangeTracker.Entries<AuditEntity>()) 
        {
            switch(entry.State) 
            {
                case EntityState.Deleted:
                {
                    entry.Entity.Deleted = true;
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    entry.State = EntityState.Modified;
                    break;
                }
                case EntityState.Modified:
                {
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach(var entityType in modelBuilder.Model.GetEntityTypes()) 
        {
            if (typeof(AuditEntity).IsAssignableFrom(entityType.ClrType))
            {
                entityType.AddSoftDelete();
            }
        }
        base.OnModelCreating(modelBuilder);
    }
}