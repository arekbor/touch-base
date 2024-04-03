using Arekbor.TouchBase.Application.Common.Interfaces;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task CommitAsync(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);
}