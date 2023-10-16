using FluentValidation;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Dberries;

public static class DberriesValidation
{
    public static void ConfigurePropertyNameResolver()
    {
        ValidatorOptions.Global.PropertyNameResolver =
            (_, member, _) =>
                member?.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? member?.Name;
    }
}