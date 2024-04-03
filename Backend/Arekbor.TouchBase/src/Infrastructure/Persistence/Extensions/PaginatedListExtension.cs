using Arekbor.TouchBase.Application.Common.Models;
using Arekbor.TouchBase.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Extensions;

public static class PaginatedListExtension
{
    private const int MaxPageSize = 10;

    public static async Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>
        (this IQueryable<TDestination> query, int pageNumber, int pageSize, CancellationToken cancellationToken) 
            where TDestination : BaseEntity
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 1 : pageSize;
        pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<TDestination>(items, count, pageNumber, pageSize);
    }
}