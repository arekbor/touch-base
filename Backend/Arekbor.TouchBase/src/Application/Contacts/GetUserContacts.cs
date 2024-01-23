using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using Mapster;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record UserContactResult(
    string Firstname, 
    string Surname, 
    string Company, 
    string Phone,
    ContactLabel Label,
    string Email,
    DateTime Birthday,
    ContactRelationship Relationship,
    string Notes
);

public record GetUserContactsQuery(Guid UserId) : IRequest<IEnumerable<UserContactResult>>;

internal class GetUserContactsHandler : IRequestHandler<GetUserContactsQuery, IEnumerable<UserContactResult>>
{
    private readonly IContactRepository _contactRepository;
    public GetUserContactsHandler(
        IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IEnumerable<UserContactResult>> Handle(GetUserContactsQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _contactRepository
            .GetContactsByUser(request.UserId, cancellationToken);

        return contacts.Adapt<IEnumerable<UserContactResult>>();
    }
}