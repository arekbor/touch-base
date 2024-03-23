using System.Text.Json.Serialization;
using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record ContactBody 
{
    public required string Firstname { get; set; }
    public required string Surname { get; set; }
    public string? Company { get; set; }
    public string? Phone { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContactLabel Label { get; set; }
    public required string Email { get; set; }
    public DateTime? Birthday { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContactRelationship Relationship { get; set; }
    public string? Notes { get; set; }
}

public record CreateOrUpdateContactCommand(ContactBody Body, Guid? Id = null) : IRequest<Unit>;

public class CreateOrUpdateContactCommandValidator : AbstractValidator<CreateOrUpdateContactCommand>
{
    public CreateOrUpdateContactCommandValidator()
    {
        RuleFor(x => x.Body.Firstname)
            .NotNull()
            .NotEmpty()
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.")
            .Matches("^[a-zA-Z]*$")
            .WithMessage("{PropertyName} can only contain letters.");
        
        RuleFor(x => x.Body.Surname)
            .NotNull()
            .NotEmpty()
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.")
            .Matches("^[a-zA-Z]*$")
            .WithMessage("{PropertyName} can only contain letters.");

        RuleFor(x => x.Body.Company)
            .MaximumLength(40)
            .When(x => !string.IsNullOrEmpty(x.Body.Company))
            .WithMessage("{PropertyName} length must be at most 40.");

        RuleFor(x => x.Body.Phone)
            .Matches(@"^\d{9}$")
            .When(x => !string.IsNullOrEmpty(x.Body.Phone))
            .WithMessage("{PropertyName} must be a valid phone number.");

        RuleFor(x => x.Body.Label)
            .IsInEnum();

        RuleFor(x => x.Body.Birthday)
            .Must(x => x <= DateTime.Now)
            .When(x => x.Body.Birthday.HasValue)
            .WithMessage("{PropertyName} is invalid.");
        
        RuleFor(x => x.Body.Email)
            .EmailAddress()
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.");

        RuleFor(x => x.Body.Relationship)
            .IsInEnum();

        RuleFor(x => x.Body.Notes)
            .MaximumLength(15)
            .When(x => !string.IsNullOrEmpty(x.Body.Notes))
            .WithMessage("{PropertyName} length must be at most 15.");
    }
}

internal class CreateOrUpdateContactCommandHandler : IRequestHandler<CreateOrUpdateContactCommand, Unit>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUserService;
    public CreateOrUpdateContactCommandHandler(
        IContactRepository contactRepository,
        ICurrentUserService currentUserService) 
    {
        _contactRepository = contactRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(CreateOrUpdateContactCommand request, CancellationToken cancellationToken)
    {
        var id = _currentUserService.Id 
            ?? throw new BadRequestException("User is not logged in");

        var currentUserId = Guid.Parse(id);
        
        if (request.Id is not null && request.Id != Guid.Empty)
        {
            await UpdateContact(request.Body, (Guid)request.Id, currentUserId, cancellationToken);
        } 
        else 
        {
            await CreateContact(request.Body, currentUserId, cancellationToken);
        }
        
        await _contactRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private Task CreateContact(ContactBody body, Guid currentUserId, CancellationToken cancellationToken)
    {
        var contact = new Contact
        {
            UserId = currentUserId,
            Firstname = body.Firstname,
            Surname = body.Surname,
            Company = body.Company,
            Phone = body.Phone,
            Label = body.Label,
            Email = body.Email,
            Birthday = body.Birthday,
            Relationship = body.Relationship,
            Notes = body.Notes
        };

        return Task.FromResult(_contactRepository.AddAsync(contact, cancellationToken));
    }

    private async Task UpdateContact(ContactBody body, Guid contactId, Guid currentUserId, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository
            .GetUserContactById(contactId, currentUserId, cancellationToken)
            ?? throw new NotFoundException($"Contact ${contactId} not found");

        contact.Firstname = body.Firstname;
        contact.Surname = body.Surname;
        contact.Company = body.Company;
        contact.Phone = body.Phone;
        contact.Label = body.Label;
        contact.Email = body.Email;
        contact.Birthday = body.Birthday;
        contact.Relationship = body.Relationship;
        contact.Notes = body.Notes;

        _contactRepository.Update(contact);
    }
}