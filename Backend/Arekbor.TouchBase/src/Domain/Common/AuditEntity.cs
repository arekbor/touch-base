namespace Arekbor.TouchBase.Domain.Common;

public abstract class AuditEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    public bool Deleted { get; set; } = false; 
}