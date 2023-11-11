using BitzArt;
using Microsoft.AspNetCore.Http;

namespace Dberries;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext httpContext)
    {
        var contains = httpContext.Items.TryGetValue("UserId", out var value);

        if (!contains || value is null)
            throw ApiException.BadRequest("UserId is required");

        var parsed = Guid.TryParse(value.ToString(), out var userId);

        if (!parsed)
            throw ApiException.BadRequest($"UserId must be {typeof(Guid)}");

        return userId;
    }
}