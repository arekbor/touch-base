using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record CreateContactCommand : IRequest<Unit>
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

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactCommandValidator()
    {
        RuleFor(x => x.Firstname)
            .NotNull()
            .NotEmpty()
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.")
            .Matches("^[a-zA-Z]*$")
            .WithMessage("{PropertyName} can only contain letters.");
        
        RuleFor(x => x.Surname)
            .NotNull()
            .NotEmpty()
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.")
            .Matches("^[a-zA-Z]*$")
            .WithMessage("{PropertyName} can only contain letters.");

        RuleFor(x => x.Company)
            .Custom(ValidateCompany);

        RuleFor(x => x.Phone)
            .Custom(ValidatePhone);

        RuleFor(x => x.Label)
            .IsInEnum();

        RuleFor(x => x.Birthday)
            .Custom(ValidateBirthday);
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.");

        RuleFor(x => x.Relationship)
            .IsInEnum();

        RuleFor(x => x.Notes)
            .Custom(ValidateNotes);
    }

    private void ValidateBirthday(DateTime? date, ValidationContext<CreateContactCommand> ctx)
    {
        if(date.HasValue && date >= DateTime.Now)
        {
            ctx.AddFailure($"{nameof(Contact.Birthday)} is invalid.");
        }
    }

    private void ValidateNotes(string? notes, ValidationContext<CreateContactCommand> ctx)
    {
        if (!string.IsNullOrEmpty(notes) && notes.Length > 15)
        {
            ctx.AddFailure($"{nameof(Contact.Notes)} length must be at most 15.");
        }
    }

    private void ValidateCompany(string? company, ValidationContext<CreateContactCommand> ctx)
    {
        if (!string.IsNullOrEmpty(company) && company.Length > 40)
        {
            ctx.AddFailure($"{nameof(Contact.Company)} length must be at most 40.");
        }
    }

    private void ValidatePhone(string? phone, ValidationContext<CreateContactCommand> ctx)
    {
        if (!string.IsNullOrEmpty(phone) && !Regex.Match(phone, @"^\d{9}$").Success)
        {
            ctx.AddFailure($"{nameof(Contact.Phone)} must be a valid phone number.");
        }
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