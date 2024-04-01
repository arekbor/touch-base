using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Application.Contacts;

public record GetContactResult
(
    Guid Id,
    string? Firstname,
    string? Surname,
    string? Phone,
    string? Email,
    string? Company,
    ContactLabel Label,
    DateOnly? Birthday,
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
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetContactQueryHandler(
        IApplicationDbContext applicationDbContext,
        ICurrentUserService currentUserService)
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<GetContactResult> Handle(GetContactQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.Id
            ?? throw new BadRequestException("User is not logged in");

        var contact = await _applicationDbContext.Contacts
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == Guid.Parse(userId), cancellationToken)
                ?? throw new NotFoundException($"Contact ${request.Id} not found");

        return contact.Adapt<GetContactResult>();
    }
}