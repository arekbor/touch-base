using Arekbor.TouchBase.Application.Common.Dtos;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Models;
using Mapster;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record GetContactsQuery(int PageNumber, int PageSize, string? SearchTerm) : IRequest<PaginatedList<ContactResult>>;

internal class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, PaginatedList<ContactResult>>
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

    public async Task<PaginatedList<ContactResult>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
    {
        var pagiantedList = await _contactRepository
            .GetUserContacts
                (_currentUserService.GetId(), request.PageNumber, request.PageSize, request.SearchTerm, cancellationToken);

        return pagiantedList.Adapt<PaginatedList<ContactResult>>();
    }
}