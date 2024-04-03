namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IUnitOfWork 
{
    Task CommitAsync(CancellationToken cancellationToken);
}