using BitzArt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dberries;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private static AuthOptions? _authOptions;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _authOptions ??= context.HttpContext.RequestServices
            .GetRequiredService<IOptions<AuthOptions>>().Value;

        var key = GetApiKey(context.HttpContext.Request);
        ValidateApiKey(key);
    }

    private static string GetApiKey(HttpRequest request)
    {
        var authHeaders = request.Headers["X-API-Key"];

        if (!authHeaders.Any())
            throw ApiException.Unauthorized("X-API-Key not found");

        if (authHeaders.Count > 1)
            throw ApiException.Unauthorized("Multiple X-API-Keys not allowed");

        var header = authHeaders.First()!;

        if (string.IsNullOrWhiteSpace(header))
            throw ApiException.Unauthorized("Invalid X-API-KEY");

        return header.Split(" ").Last();
    }

    private void ValidateApiKey(string userXApiKey)
    {
        if (userXApiKey != _authOptions?.XApiKey)
            throw ApiException.Unauthorized("Invalid X-API-KEY");
    }
}