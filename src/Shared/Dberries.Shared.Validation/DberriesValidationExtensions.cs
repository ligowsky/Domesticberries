using BitzArt;
using FluentValidation;

namespace Dberries;

public static class DberriesValidationExtensions
{
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