using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Presentation;

public static class AddPresentationExtensions
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddValidation();
    }
}