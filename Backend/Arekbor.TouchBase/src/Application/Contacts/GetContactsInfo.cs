using Arekbor.TouchBase.Application.Common.Dtos;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Mapster;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record GetContactsInfoResult
(
    int CountOfContacts,
    ContactResult? LastCreatedContact
);

public record GetContactsInfoQuery() : IRequest<GetContactsInfoResult>;

internal class GetContactsInfoQueryHandler : IRequestHandler<GetContactsInfoQuery, GetContactsInfoResult>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetContactsInfoQueryHandler(
        IContactRepository contactRepository, 
        ICurrentUserService currentUserService)
    {
        _contactRepository = contactRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetContactsInfoResult> Handle(GetContactsInfoQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetId();

        var countOfCreatedContacts = await _contactRepository
            .GetUserContactsCount(userId, cancellationToken);

        var lastCreatedContact = await _contactRepository
            .GetLastCreatedContact(userId, cancellationToken);

        return new GetContactsInfoResult(countOfCreatedContacts, lastCreatedContact.Adapt<ContactResult>());
    }
}