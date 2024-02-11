using Arekbor.TouchBase.Application.Common.Models;
using Arekbor.TouchBase.Domain.Entities;

namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IContactRepository : IBaseRepository<Contact>
{
    Task<PaginatedList<Contact>> GetContactsByUser(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken);
}