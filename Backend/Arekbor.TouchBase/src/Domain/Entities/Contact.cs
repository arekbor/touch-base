using Arekbor.TouchBase.Domain.Common;

namespace Arekbor.TouchBase.Domain.Entities;

public enum ContactLabel
{
    NoLabel = 0,
    Mobile = 1,
    Work = 2,
    Home = 3,
    Main = 4,
    WorkFax = 5,
    HomeFax = 6,
    Pager = 7,
    Other = 8
}

public enum ContactRelationship
{
    NoRelation = 0,
    Assistant = 1,
    Brother = 2,
    Child = 3,
    DomesticPartner = 4,
    Father = 5,
    Friend = 6,
    Manager = 7,
    Mother = 8,
    Partner = 9,
    Parent = 10,
    Relative = 11,
    Sister = 12,
    Spouse = 13
}

public class Contact: AuditEntity
{
    public string? Firstname { get; set; }
    public string? Surname { get; set; }
    public string? Company { get; set; }
    public string? Phone { get; set; }
    public ContactLabel Label { get; set; }
    public string? Email { get; set; }
    public DateOnly? Birthday { get; set; }
    public ContactRelationship Relationship { get; set; }
    public string? Notes { get; set; }
    public Guid UserId { get; set; }
    public virtual User? User { get; set; }
}