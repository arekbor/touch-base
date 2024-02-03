using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Context;
    public BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => Task.FromResult(Context.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask());

    public void Delete(TEntity entity)
        => Context.Set<TEntity>().Remove(entity);

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        => Context.Set<TEntity>().ToListAsync(cancellationToken);

    public Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken)
        => Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Update(TEntity entity)
        => Context.Set<TEntity>().Update(entity);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => Context.SaveChangesAsync(cancellationToken);
}