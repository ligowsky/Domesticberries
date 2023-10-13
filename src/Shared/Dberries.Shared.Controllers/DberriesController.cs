using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries;

public abstract class DberriesController : ControllerBase
{
    [NonAction]
    protected void ValidateDto<TValidator, TDto>(TDto value) where TValidator : AbstractValidator<TDto>
    {
        if (value is null) return;

        var actionType = HttpContext.Request.Method.ToActionType();

        var validator = GetRequiredService<TValidator>();
        validator.ValidateDto(value, actionType);
    }

    private TService GetRequiredService<TService>()
        where TService : class =>
        HttpContext.RequestServices.GetRequiredService<TService>()!;
}