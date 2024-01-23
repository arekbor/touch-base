using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Repositories;

public class ContactRepository : BaseRepository<Contact>, IContactRepository
{
    public ContactRepository(ApplicationDbContext context) : base(context)
    {}

    public async Task<IEnumerable<Contact>> GetContactsByUser(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Contacts
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}