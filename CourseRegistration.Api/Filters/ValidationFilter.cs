using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CourseRegistration.Api.Filters;

public class ValidationFilter<T> : IAsyncActionFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var argument = context.ActionArguments.Values
            .FirstOrDefault(x => x is T) as T;

        if (argument is not null)
        {
            var validationResult = await _validator.ValidateAsync(argument);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        await next();
    }
}