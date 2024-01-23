using Arekbor.TouchBase.Domain.Common;

namespace Arekbor.TouchBase.Domain.Entities;

public class RefreshToken : AuditEntity
{
    public Guid UserId { get; set; }
    public string? Token { get; set; }
    public DateTime Expires { get; set; }
}