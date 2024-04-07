namespace Arekbor.TouchBase.Application.Common.Dtos;

public record ContactResult(
    Guid Id,
    string? Firstname, 
    string? Surname, 
    string? Phone,
    string? Email
);