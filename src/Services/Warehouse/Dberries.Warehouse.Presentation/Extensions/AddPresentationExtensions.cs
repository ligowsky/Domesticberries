using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Presentation;

public static class AddPresentationExtensions
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddValidation();
    }
}