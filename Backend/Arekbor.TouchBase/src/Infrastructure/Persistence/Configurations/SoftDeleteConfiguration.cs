using System.Linq.Expressions;
using System.Reflection;
using Ardalis.GuardClauses;
using Arekbor.TouchBase.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Arekbor.TouchBase.Infrastructure.Persistence.Configurations;

public static class SoftDeleteConfiguration 
{
    public static void AddSoftDelete(this IMutableEntityType entityType) 
    {
        var methodInfo = typeof(SoftDeleteConfiguration)
            .GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)?
            .MakeGenericMethod(entityType.ClrType);

        Guard.Against.Null(methodInfo);

        var filter = methodInfo.Invoke(null, [])!;

        Guard.Against.Null(filter);

        entityType.SetQueryFilter((LambdaExpression)filter);

        var deletedProperty = entityType.FindProperty(nameof(AuditEntity.Deleted));

        Guard.Against.Null(deletedProperty);

        entityType.AddIndex(deletedProperty);
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>()
        where TEntity : AuditEntity
    {
        Expression<Func<TEntity, bool>> filter = x => !x.Deleted;
        return filter;
    }
}