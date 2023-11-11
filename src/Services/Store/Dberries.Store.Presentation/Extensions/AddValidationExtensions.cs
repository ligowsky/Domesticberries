using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Presentation;

public static class AddValidationExtension
{
    private interface IValidatorAssemblyPointer
    {
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IValidatorAssemblyPointer>();

        DberriesValidation.ConfigurePropertyNameResolver();
    }
}