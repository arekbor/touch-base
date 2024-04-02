using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) 
        : base(context)
    {}

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => Context.Users
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
}