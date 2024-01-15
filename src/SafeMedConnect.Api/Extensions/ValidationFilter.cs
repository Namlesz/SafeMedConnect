using FluentValidation;
using SafeMedConnect.Api.Attributes;
using System.Net;
using System.Reflection;

namespace SafeMedConnect.Api.Extensions;

internal static class ValidationFilter
{
    public static EndpointFilterDelegate ValidationFilterFactory(
        EndpointFilterFactoryContext context,
        EndpointFilterDelegate next
    )
    {
        var validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices);

        var descriptors = validationDescriptors as ValidationDescriptor[] ?? validationDescriptors.ToArray();
        return descriptors.Length != 0
            ? invocationContext => Validate(descriptors, invocationContext, next)
            : next;
    }

    private static async ValueTask<object?> Validate(
        IEnumerable<ValidationDescriptor> validationDescriptors,
        EndpointFilterInvocationContext invocationContext,
        EndpointFilterDelegate next
    )
    {
        foreach (var descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

            if (argument is null)
            {
                continue;
            }

            var validationResult = await descriptor.Validator.ValidateAsync(
                new ValidationContext<object>(argument)
            );

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary(),
                    statusCode: (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        return await next.Invoke(invocationContext);
    }

    static IEnumerable<ValidationDescriptor> GetValidators(MethodInfo methodInfo, IServiceProvider serviceProvider)
    {
        var parameters = methodInfo.GetParameters();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];

            if (parameter.GetCustomAttribute<ValidateAttribute>() is null)
            {
                continue;
            }
            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

            // Note that FluentValidation validators needs to be registered as singleton
            if (serviceProvider.GetService(validatorType) is IValidator validator)
            {
                yield return new ValidationDescriptor
                    { ArgumentIndex = i, ArgumentType = parameter.ParameterType, Validator = validator };
            }
        }
    }

    private class ValidationDescriptor
    {
        public required int ArgumentIndex { get; init; }
        public required Type ArgumentType { get; init; }
        public required IValidator Validator { get; init; }
    }
}