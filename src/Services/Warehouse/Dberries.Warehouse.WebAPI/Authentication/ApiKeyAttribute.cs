using Microsoft.AspNetCore.Mvc;

namespace Dberries.Warehouse.Authentication;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthFilter))
    {
    }
}