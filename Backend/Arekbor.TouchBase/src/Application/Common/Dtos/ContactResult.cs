namespace Arekbor.TouchBase.Application.Common.Dtos;

public record ContactResult(
    Guid Id,
    string? Firstname, 
    string? Lastname, 
    string? Phone,
    string? Email
);