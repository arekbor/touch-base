using System.Text.Json.Serialization;
using Arekbor.TouchBase.Domain.Entities;

namespace Arekbor.TouchBase.Application.Common.Dtos;

public record ContactBody
{
    public required string Firstname { get; set; }
    public required string Surname { get; set; }
    public string? Company { get; set; }
    public string? Phone { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContactLabel Label { get; set; }
    public required string Email { get; set; }
    public DateTime? Birthday { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContactRelationship Relationship { get; set; }
    public string? Notes { get; set; }
}