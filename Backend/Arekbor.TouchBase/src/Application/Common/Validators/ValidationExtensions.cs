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
            .Matches("^[a-zA-Z]*$")
            .WithMessage("{PropertyName} can only contain letters.");
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