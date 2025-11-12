using ErrorOr;
using FluentValidation;
using MediatR;

namespace CitizensPortal.Api.Infrastructure.Behaviors;

/// <summary>
/// MediatR pipeline behavior that validates requests using FluentValidation.
/// Automatically runs all validators registered for the request type.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            var errors = failures
                .Select(f => Error.Validation(
                    code: f.PropertyName,
                    description: f.ErrorMessage))
                .ToList();

            return (dynamic)errors;
        }

        return await next();
    }
}
