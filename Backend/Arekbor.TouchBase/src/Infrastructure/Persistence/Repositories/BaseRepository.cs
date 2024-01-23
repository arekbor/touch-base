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

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Delete(TEntity entity)
        => Context.Set<TEntity>().Remove(entity);

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        => await Context.Set<TEntity>().ToArrayAsync(cancellationToken);

    public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken)
        => await Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Update(TEntity entity)
        => Context.Set<TEntity>().Update(entity);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await Context.SaveChangesAsync(cancellationToken);
}