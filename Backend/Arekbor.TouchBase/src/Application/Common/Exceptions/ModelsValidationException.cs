using FluentValidation.Results;

namespace Arekbor.TouchBase.Application.Common.Exceptions;

public class ModelsValidationException : Exception
{
    public IDictionary<string, object> Errors { get; private set; }

    public ModelsValidationException(IEnumerable<ValidationResult> validationResults)
        : base("One or more validation errors occurred")
    {
        Errors = new Dictionary<string, object>();
        foreach(var validationResult in validationResults)
        {
            Errors["errors"] = validationResult.Errors.Select(x => x.ErrorMessage);
        }
    }
}