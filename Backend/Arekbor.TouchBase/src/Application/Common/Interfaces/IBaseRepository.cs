using Arekbor.TouchBase.Domain.Common;

namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IBaseRepository<TEntity>
    where TEntity: BaseEntity
{
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
