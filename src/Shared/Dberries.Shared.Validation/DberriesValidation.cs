using System.Reflection;
using System.Text.Json.Serialization;
using BitzArt;
using FluentValidation;

namespace Dberries;

public static class DberriesValidation
{
    public static void ConfigurePropertyNameResolver()
    {
        ValidatorOptions.Global.PropertyNameResolver =
            (_, member, _) =>
                member?.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? member?.Name;
    }
    
    public static void ValidateDto<T>(this IValidator<T> validator, T model, ActionType actionType)
    {
        var result = validator.Validate(model,
            options =>
            {
                options.IncludeRuleSets($"{actionType}")
                    .IncludeRulesNotInRuleSet();
            });

        if (result.IsValid) return;

        var exception = ApiException.BadRequest("Validation failed");
        exception.Payload.AddData(new { errors = result.Errors.Select(x => x.ErrorMessage) });

        throw exception;
    }
}