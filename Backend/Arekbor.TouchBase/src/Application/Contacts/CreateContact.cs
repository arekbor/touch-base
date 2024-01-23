using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record CreateContactCommand(
    string Firstname, 
    string Surname, 
    string Company, 
    string Phone,
    ContactLabel Label,
    string Email,
    DateTime Birthday,
    ContactRelationship Relationship,
    string Notes
) : IRequest<Unit>;

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactCommandValidator()
    {
        RuleFor(x => x.Firstname)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Surname)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Label)
            .IsInEnum();
        
        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Relationship)
            .IsInEnum();
    }
}

internal class CreateContactHandler : IRequestHandler<CreateContactCommand, Unit>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUserService;
    public CreateContactHandler(
        IContactRepository contactRepository,
        ICurrentUserService currentUserService) 
    {
        _contactRepository = contactRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var id = _currentUserService.Id 
            ?? throw new BadRequestException("User is not logged in");

        var currentId = Guid.Parse(id);
        
        var contact = new Contact
        {
            UserId = currentId,
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