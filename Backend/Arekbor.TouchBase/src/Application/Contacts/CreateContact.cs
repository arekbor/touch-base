using Arekbor.TouchBase.Application.Common.Dtos;
using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Validators;
using Arekbor.TouchBase.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record CreateContactCommand(): ContactBody, IRequest<Unit>;

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactCommandValidator()
    {
        Include(new ContactBodyValidator());
    }
}

internal class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Unit>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUserService;
    public CreateContactCommandHandler(
        IContactRepository contactRepository,
        ICurrentUserService currentUserService) 
    {
        _contactRepository = contactRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.Id 
            ?? throw new BadRequestException("User is not logged in");

        var contact = new Contact
        {
            UserId = Guid.Parse(userId),
            Firstname = request.Firstname,
            Surname = request.Surname,
            Company = request.Company,
            Phone = request.Phone,
            Label = request.Label,
            Email = request.Email,
            Birthday = request.Birthday,
            Relationship = request.Relationship,
            Notes = request.Notes
        };

        await _contactRepository.AddAsync(contact, cancellationToken);
        await _contactRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}