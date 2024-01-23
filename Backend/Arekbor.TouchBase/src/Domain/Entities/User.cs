using Arekbor.TouchBase.Domain.Common;

namespace Arekbor.TouchBase.Domain.Entities;

public enum UserRole
{
    User,
    Admin,
}

public class User : AuditEntity
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public ICollection<string> Claims = [];
}