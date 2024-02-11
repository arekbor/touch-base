using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Models;
using Arekbor.TouchBase.Domain.Entities;
using Arekbor.TouchBase.Infrastructure.Persistence.Extensions;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Repositories;

public class ContactRepository : BaseRepository<Contact>, IContactRepository
{
    public ContactRepository(ApplicationDbContext context) : base(context)
    {}

    public Task<PaginatedList<Contact>> GetContactsByUser(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return Context.Contacts
            .Where(x => x.UserId == userId)
            .ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
    }
}