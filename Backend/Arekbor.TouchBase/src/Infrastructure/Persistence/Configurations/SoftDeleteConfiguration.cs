using System.Linq.Expressions;
using System.Reflection;
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
        if (methodInfo is null)
        {
            throw new Exception($"Error while getting {nameof(methodInfo)}");
        }

        var filter = methodInfo.Invoke(null, [])!;
        if (filter is null) 
        {
            throw new Exception($"Error while invoking {nameof(methodInfo)}");
        }

        entityType.SetQueryFilter((LambdaExpression)filter);

        var deletedProperty = entityType.FindProperty(nameof(AuditEntity.Deleted));
        if (deletedProperty is null)
        {
            throw new Exception($"Error while getting property: {nameof(AuditEntity.Deleted)} from {nameof(AuditEntity)}");
        } 
        entityType.AddIndex(deletedProperty);
        
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>()
        where TEntity : AuditEntity
    {
        Expression<Func<TEntity, bool>> filter = x => !x.Deleted;
        return filter;
    }
}