using System.Linq.Expressions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Models;
using Arekbor.TouchBase.Domain.Entities;
using Arekbor.TouchBase.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Repositories;

public class ContactRepository : BaseRepository<Contact>, IContactRepository
{
    public ContactRepository(ApplicationDbContext context) : base(context)
    {}

    public Task<PaginatedList<Contact>> GetUserContacts
        (Guid userId, int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
    {
        Expression<Func<Contact, bool>> searchContactExpression = c =>
            (c.Firstname != null && c.Firstname.Contains(searchTerm ?? string.Empty)) ||
            (c.Lastname != null && c.Lastname.Contains(searchTerm ?? string.Empty)) ||
            (c.Email != null && c.Email.Contains(searchTerm ?? string.Empty)) ||
            (c.Company != null && c.Company.Contains(searchTerm ?? string.Empty)) ||
            (c.Notes != null && c.Notes.Contains(searchTerm ?? string.Empty)) ||
            (c.Phone != null && c.Phone.Contains(searchTerm ?? string.Empty));

        return Context.Contacts
            .Where(x => x.UserId == userId)
            .WhereIf(searchContactExpression, !string.IsNullOrWhiteSpace(searchTerm))
            .OrderByDescending(x => x.CreatedAt)
            .ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
    }

    public Task<Contact?> GetUserContactById(Guid contactId, Guid userId, CancellationToken cancellationToken)
        => Context.Contacts
            .FirstOrDefaultAsync(x => x.Id == contactId && x.UserId == userId, cancellationToken);

    public Task<int> GetUserContactsCount(Guid userId, CancellationToken cancellationToken)
        => Context.Contacts
            .Where(c => c.UserId == userId).CountAsync(cancellationToken);

    public Task<Contact?> GetLastCreatedContact(Guid userId, CancellationToken cancellationToken)
    {
        return Context.Contacts
            .Where(c => c.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}