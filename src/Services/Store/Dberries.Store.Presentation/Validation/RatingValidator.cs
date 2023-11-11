using FluentValidation;

namespace Dberries.Store.Presentation;

public class RatingValidator : AbstractValidator<RatingDto>
{
    public RatingValidator()
    {
        RuleFor(x => x.Value)
            .NotNull()
            .InclusiveBetween(1, 5);
    }
}