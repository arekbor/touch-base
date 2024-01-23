using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {}

    public async Task<RefreshToken?> GetRefreshTokenByToken(string token, CancellationToken cancellationToken)
    {
        return await Context.RefreshTokens
            .FirstOrDefaultAsync(x => 
                x.Token == token && 
                x.Expires >= DateTime.Now, cancellationToken
        );
    }
}