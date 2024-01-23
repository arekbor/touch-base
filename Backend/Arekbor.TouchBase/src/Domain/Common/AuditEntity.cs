namespace Arekbor.TouchBase.Domain.Common;

public abstract class AuditEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
    public bool Deleted { get; set; } = false; 
}