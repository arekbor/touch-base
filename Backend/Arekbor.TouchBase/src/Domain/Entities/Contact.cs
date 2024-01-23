using Arekbor.TouchBase.Domain.Common;

namespace Arekbor.TouchBase.Domain.Entities;

public enum ContactLabel
{
    NoLabel,
    Mobile,
    Work,
    Home,
    Main,
    WorkFax,
    HomeFax,
    Pager,
    Other
}

public enum ContactRelationship
{
    Assistant,
    Brother,
    Child,
    DomesticPartner,
    Father,
    Friend,
    Manager,
    Mother,
    Partner,
    Parent,
    Relative,
    Sister,
    Spouse
}

public class Contact: AuditEntity
{
    public string? Firstname { get; set; }
    public string? Surname { get; set; }
    public string? Company { get; set; }
    public string? Phone { get; set; }
    public ContactLabel Label { get; set; }
    public string? Email { get; set; }
    public DateTime Birthday { get; set; }
    public ContactRelationship Relationship { get; set; }
    public string? Notes { get; set; }
    public Guid UserId { get; set; }
    public virtual User? User { get; set; }
}