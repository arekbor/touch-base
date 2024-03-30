using System.ComponentModel.DataAnnotations;

namespace Arekbor.TouchBase.Infrastructure.Options;

public class PersistenceOptions
{
    public const string Position = "Persistence";
    [Required]
    public string Postgres { get; set; } = "";
}