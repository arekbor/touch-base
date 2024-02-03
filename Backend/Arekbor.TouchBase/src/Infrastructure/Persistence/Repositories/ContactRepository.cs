using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Repositories;

public class ContactRepository : BaseRepository<Contact>, IContactRepository
{
    public ContactRepository(ApplicationDbContext context) : base(context)
    {}

    public Task<List<Contact>> GetContactsByUser(Guid userId, CancellationToken cancellationToken)
    {
        return Context.Contacts
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}