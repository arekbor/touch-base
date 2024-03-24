using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using FluentValidation;
using Mapster;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record GetContactResult(
    Guid Id,
    string? Firstname,
    string? Surname,
    string? Phone,
    string? Email,
    string? Company,
    ContactLabel Label,
    DateTime? Birthday,
    ContactRelationship Relationship,
    string? Notes
);

public record GetContactQuery(Guid Id): IRequest<GetContactResult>;

public class GetContactQueryValidator : AbstractValidator<GetContactQuery>
{
    public GetContactQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal class GetContactQueryHandler : IRequestHandler<GetContactQuery, GetContactResult>
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

    public async Task<GetContactResult> Handle(GetContactQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.Id
            ?? throw new BadRequestException("User is not logged in");

        var contact = await _contactRepository
            .GetUserContactById(request.Id, Guid.Parse(userId), cancellationToken)
            ?? throw new NotFoundException($"Contact ${request.Id} not found");

        return contact.Adapt<GetContactResult>();
    }
}