using Arekbor.TouchBase.Domain.Entities;

namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IContactRepository : IBaseRepository<Contact>
{
    Task<List<Contact>> GetContactsByUser(Guid userId, CancellationToken cancellationToken);
}