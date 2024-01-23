using Arekbor.TouchBase.Application.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace Arekbor.TouchBase.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var failures = _validators
            .Select(x => x.Validate(request))
            .Where(x => x.IsValid == false)
            .AsEnumerable();

        if (failures.Any())
        {
            throw new ModelsValidationException(failures);
        }
        return await next();
    }
}