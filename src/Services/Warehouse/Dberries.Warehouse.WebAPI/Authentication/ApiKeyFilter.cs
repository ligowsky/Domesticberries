using BitzArt;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dberries.Warehouse.Authentication;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IApiKeyValidation _apiKeyValidation;

    public ApiKeyAuthFilter(IApiKeyValidation apiKeyValidation)
    {
        _apiKeyValidation = apiKeyValidation;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userApiKey = context.HttpContext.Request.Headers[AuthConstants.ApiKeyHeaderName].ToString();

        if (string.IsNullOrWhiteSpace(userApiKey) || !_apiKeyValidation.IsValidApiKey(userApiKey))
            throw ApiException.Unauthorized($"{AuthConstants.ApiKeyHeaderName} is not valid");
    }
}