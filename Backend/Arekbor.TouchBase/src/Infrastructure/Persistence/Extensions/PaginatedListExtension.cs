using Arekbor.TouchBase.Application.Common.Models;
using Arekbor.TouchBase.Domain.Common;
using Arekbor.TouchBase.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Extensions;

public static class PaginatedListExtension
{
    private static IOptions<PaginationOptions>? _paginationOptions;

    public static void Configure(IOptions<PaginationOptions> paginationOptions)
    {
        _paginationOptions = paginationOptions;
    }

    public static async Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>
        (this IQueryable<TDestination> query, int pageNumber, int pageSize, CancellationToken cancellationToken) 
            where TDestination : BaseEntity
    {
        var options = _paginationOptions 
            ?? throw new Exception($"Could not resolve {nameof(PaginationOptions)}");
            
        var maxPageSize = options.Value.MaxPageSize;

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 1 : pageSize;
        pageSize = pageSize > maxPageSize ? maxPageSize : pageSize;

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);

        return new PaginatedList<TDestination>(items, count, pageNumber, pageSize);
    }
}