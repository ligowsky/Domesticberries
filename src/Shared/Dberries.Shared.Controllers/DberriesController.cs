using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public abstract class DberriesController : ControllerBase
{
    [NonAction]
    protected void Validate<TDto>(TDto value)
    {
        if (value is null) return;

        var actionType = HttpContext.Request.Method.ToActionType();

        var validator = GetRequiredService<IValidator<TDto>>();
        validator.Validate(value, actionType);
    }

    private TService GetRequiredService<TService>()
        where TService : class =>
        HttpContext.RequestServices.GetRequiredService<TService>()!;
}