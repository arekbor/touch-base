using Arekbor.TouchBase.Application.Common.Dtos;
using FluentValidation;

namespace Arekbor.TouchBase.Application.Common.Validators;

public class ContactBodyValidator : AbstractValidator<ContactBody>
{
    public ContactBodyValidator()
    {

        RuleFor(x => x.Firstname)
            .PersonName();
        
        RuleFor(x => x.Lastname)
            .PersonName();

        RuleFor(x => x.Company)
            .MaxLengthIfNotEmptyOrNull(40);

        RuleFor(x => x.Phone)
            .Matches(@"^\d{9}$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.Label)
            .IsInEnum();

        RuleFor(x => x.Birthday)
            .Must(x => x <= DateOnly.FromDateTime(DateTime.Now))
            .When(x => x.Birthday.HasValue)
            .WithMessage("Invalid date of birth.");
        
        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Relationship)
            .IsInEnum();

        RuleFor(x => x.Notes)
            .MaxLengthIfNotEmptyOrNull(15);
    }
}