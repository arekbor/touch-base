using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Models;
using Mapster;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record GetContactsResult(
    Guid Id,
    string? Firstname, 
    string? Surname, 
    string? Phone,
    string? Email
);

public record GetContactsQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<GetContactsResult>>;

internal class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, PaginatedList<GetContactsResult>>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUserService;
    public GetContactsQueryHandler(
        IContactRepository contactRepository, 
        ICurrentUserService currentUserService)
    {
        _contactRepository = contactRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<GetContactsResult>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
    {
        var pagiantedList = await _contactRepository
            .GetUserContacts(_currentUserService.GetId(), request.PageNumber, request.PageSize, cancellationToken);

        return pagiantedList.Adapt<PaginatedList<GetContactsResult>>();
    }
}