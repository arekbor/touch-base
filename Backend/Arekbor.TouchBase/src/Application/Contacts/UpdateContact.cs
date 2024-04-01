using Arekbor.TouchBase.Application.Common.Dtos;
using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Validators;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Application.Contacts;

public record UpdateContactCommand(Guid Id, ContactBody ContactBody) : IRequest<Unit>;

public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
{
    public UpdateContactCommandValidator()
    {
        RuleFor(x => x.ContactBody)
            .SetValidator(new ContactBodyValidator());
    }
}

internal class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Unit>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    public UpdateContactCommandHandler(
        IApplicationDbContext applicationDbContext,
        ICurrentUserService currentUserService) 
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.Id 
            ?? throw new BadRequestException("User is not logged in");

        var contact = await _applicationDbContext.Contacts
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == Guid.Parse(userId), cancellationToken)
                ?? throw new NotFoundException($"Contact ${request.Id} not found");

        contact.Firstname = request.ContactBody.Firstname;
        contact.Surname = request.ContactBody.Surname;
        contact.Company = request.ContactBody.Company;
        contact.Phone = request.ContactBody.Phone;
        contact.Label = request.ContactBody.Label;
        contact.Email = request.ContactBody.Email;
        contact.Birthday = request.ContactBody.Birthday;
        contact.Relationship = request.ContactBody.Relationship;
        contact.Notes = request.ContactBody.Notes;

        _applicationDbContext.Contacts.Update(contact);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}