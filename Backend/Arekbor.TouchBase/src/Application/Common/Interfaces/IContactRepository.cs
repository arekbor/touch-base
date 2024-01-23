using Arekbor.TouchBase.Domain.Entities;

namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IContactRepository : IBaseRepository<Contact>
{
    Task<IEnumerable<Contact>> GetContactsByUser(Guid userId, CancellationToken cancellationToken);
}