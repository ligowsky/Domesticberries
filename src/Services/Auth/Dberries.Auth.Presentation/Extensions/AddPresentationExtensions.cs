using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Presentation;

public static class AddPresentationExtensions
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddValidation();
    }
}