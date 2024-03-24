using Arekbor.TouchBase.Application.Common.Dtos;
using FluentValidation;

namespace Arekbor.TouchBase.Application.Common.Validators;

public class ContactBodyValidator : AbstractValidator<ContactBody>
{
    public ContactBodyValidator()
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
            .MaximumLength(40)
            .When(x => !string.IsNullOrEmpty(x.Company))
            .WithMessage("{PropertyName} length must be at most 40.");

        RuleFor(x => x.Phone)
            .Matches(@"^\d{9}$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("{PropertyName} must be a valid phone number.");

        RuleFor(x => x.Label)
            .IsInEnum();

        RuleFor(x => x.Birthday)
            .Must(x => x <= DateTime.Now)
            .When(x => x.Birthday.HasValue)
            .WithMessage("{PropertyName} is invalid.");
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.");

        RuleFor(x => x.Relationship)
            .IsInEnum();

        RuleFor(x => x.Notes)
            .MaximumLength(15)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("{PropertyName} length must be at most 15.");
    }
}