using BitzArt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dberries;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TokenAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private ITokenConsumerService? _tokenService;
    private static TokenAuthConsumerOptions? _tokenAuthOptions;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        _tokenService ??= context.HttpContext.RequestServices
            .GetRequiredService<ITokenConsumerService>();
        
        _tokenAuthOptions ??= context.HttpContext.RequestServices
            .GetRequiredService<TokenAuthConsumerOptions>();

        var accessToken = GetAccessToken(context.HttpContext.Request);
        
        var accessTokenData = _tokenService.GetAccessTokenData(accessToken);
        
        context.HttpContext.Items["UserId"] = accessTokenData.UserId;
    }

    private static string GetAccessToken(HttpRequest request)
    {
        var authHeaders = request.Headers["Authorization"].ToList();

        if (!authHeaders.Any())
            throw ApiException.Unauthorized("Authorization header is not found");
        
        if (authHeaders.Count > 1)
            throw ApiException.Unauthorized("Multiple Authorization headers are not allowed");

        var header = authHeaders.First()!;

        return header.Split(" ").Last();
    }
}