using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SharedKernel.Concrete;

namespace Application.Behaviors;

internal class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ValidationFailure[] validationFailures = await ValidateAsync(request);

        if (validationFailures.Length == 0)
        { 
            return await next();
        }

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type resultType = typeof(TResponse).GetGenericArguments()[0];

            MethodInfo failureMethod = typeof(Result<>).MakeGenericType(resultType)
                                                       .GetMethod(nameof(Result<object>.ValidationFailure));

            if (failureMethod is not null)
            {
                return (TResponse)failureMethod.Invoke(null, [CreateValidationError(validationFailures)]);
            }
        }
        else if (typeof(TResponse) == typeof(Result))
        { 
            return (TResponse)(object)Result.Failure(CreateValidationError(validationFailures));
        }

        throw new ValidationException("Validation failed", validationFailures);
    }

    private async ValueTask<ValidationFailure[]> ValidateAsync(TRequest request)
    {
        if (validators is null)
        { 
            return [];
        }

        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationResults = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context)));

        ValidationFailure[] validationFailures = validationResults.Where(validationResult => !validationResult.IsValid)
                                                        .SelectMany(validationResult => validationResult.Errors)
                                                        .ToArray();

        return validationFailures;
    }

    private static ValidationError CreateValidationError(ValidationFailure[] validationFailures) =>
    new(validationFailures.Select(
        failure => Error.Problem(
            failure.PropertyName,
            failure.ErrorMessage))
        .ToArray());
}
