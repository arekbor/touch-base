using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Models;
using Arekbor.TouchBase.Domain.Entities;
using Mapster;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record UserContactResult(
    string? Firstname, 
    string? Surname, 
    string? Company, 
    string? Phone,
    ContactLabel Label,
    string? Email,
    DateTime? Birthday,
    ContactRelationship Relationship,
    string? Notes
);

public record GetContactsQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<UserContactResult>>;

internal class GetUserContactsHandler : IRequestHandler<GetContactsQuery, PaginatedList<UserContactResult>>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUserService;
    public GetUserContactsHandler(
        IContactRepository contactRepository, 
        ICurrentUserService currentUserService)
    {
        _contactRepository = contactRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<UserContactResult>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
    {
        var id = _currentUserService.Id 
            ?? throw new BadRequestException("User is not logged in");

        var pagiantedList = await _contactRepository
            .GetContactsByUser(Guid.Parse(id), request.PageNumber, request.PageSize, cancellationToken);

        return pagiantedList.Adapt<PaginatedList<UserContactResult>>();
    }
}