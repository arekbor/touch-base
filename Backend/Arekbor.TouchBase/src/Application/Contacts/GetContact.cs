using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using FluentValidation;
using Mapster;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record ContactDetailResult
(
    Guid Id,
    string? Firstname,
    string? Lastname,
    string? Phone,
    string? Email,
    string? Company,
    ContactLabel Label,
    DateOnly? Birthday,
    ContactRelationship Relationship,
    string? Notes
);

public record GetContactQuery(Guid Id): IRequest<ContactDetailResult>;

public class GetContactQueryValidator : AbstractValidator<GetContactQuery>
{
    public GetContactQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal class GetContactQueryHandler : IRequestHandler<GetContactQuery, ContactDetailResult>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetContactQueryHandler(
        IContactRepository contactRepository,
        ICurrentUserService currentUserService)
    {
        _contactRepository = contactRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ContactDetailResult> Handle(GetContactQuery request, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository
            .GetUserContactById(request.Id, _currentUserService.GetId(), cancellationToken)
                ?? throw new NotFoundException($"Contact ${request.Id} not found");

        return contact.Adapt<ContactDetailResult>();
    }
}