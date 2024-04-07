using Arekbor.TouchBase.Application.Common.Models;
using Arekbor.TouchBase.Domain.Entities;

namespace Arekbor.TouchBase.Application.Common.Interfaces;

public interface IContactRepository : IBaseRepository<Contact>
{
    Task<PaginatedList<Contact>> GetUserContacts
        (Guid userId, int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    Task<Contact?> GetUserContactById(Guid contactId, Guid userId, CancellationToken cancellationToken);
    Task<int> GetUserContactsCount(Guid userId, CancellationToken cancellationToken);
    Task<Contact?> GetLastCreatedContact(Guid userId, CancellationToken cancellationToken);
}