using System.ComponentModel.DataAnnotations;

namespace Arekbor.TouchBase.Infrastructure.Options;

public class PaginationOptions
{
    public const string Position = "Pagination";
    [Required]
    [Range(5, 100)]
    public int MaxPageSize { get; set; }
}