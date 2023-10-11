using BitzArt;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Presentation;

public static class AddValidationExtension
{
    private interface IValidatorAssemblyPointer
    {
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IValidatorAssemblyPointer>();
    }

    public static void ValidateDto<T>(this AbstractValidator<T> validator, T model)
    {
        var result = validator.Validate(model);
        if (!result.IsValid)
        {
            var exception = ApiException.BadRequest("Validation failed");
            exception.Payload.AddData(new { errors = result.Errors });
            throw exception;
        }
    }
}