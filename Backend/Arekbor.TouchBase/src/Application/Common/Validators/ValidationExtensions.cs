using FluentValidation;

namespace Arekbor.TouchBase.Application.Common.Validators;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> PersonName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .MaximumLength(40)
            .WithMessage("{PropertyName} cannot contain more than 40 characters.")
            .Matches("^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ]*$")
            .WithMessage("{PropertyName} can only contain letters.");
    }

    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .NotNull()
            .MinimumLength(8)
            .WithMessage("{PropertyName} must be at least 8 characters long.")
            .MaximumLength(40)
            .WithMessage("{PropertyName} cannot contain more than 40 characters.")
            .Matches(@"[A-Z]+")
            .WithMessage("{PropertyName} must contain at least one uppercase letter.")
            .Matches(@"[a-z]+")
            .WithMessage("{PropertyName} must contain at least one lowercase letter.")
            .Matches(@"[0-9]+")
            .WithMessage("{PropertyName} must contain at least one number.")
            .Matches(@"[][""!@#$%^&*(){}:;<>,.?/+_=|'~\\-]")
            .WithMessage("{PropertyName} must contain at least one special character.");
    }

    public static IRuleBuilderOptions<T, string> Username<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .MaximumLength(40)
            .WithMessage("{PropertyName} cannot contain more than 40 characters.");
    }

    public static IRuleBuilderOptions<T, string?> MaxLengthIfNotEmptyOrNull<T>
        (this IRuleBuilder<T, string?> ruleBuilder, int maxLength)
    {
        return (IRuleBuilderOptions<T, string?>)ruleBuilder.Custom((obj, ctx) => {
            if (!string.IsNullOrEmpty(obj) && obj.Length > maxLength)
            {
                ctx.AddFailure($"{ctx.DisplayName} cannot contain more than {maxLength} characters.");
            }
        });
    }
}