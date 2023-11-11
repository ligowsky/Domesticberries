using FluentValidation;

namespace Dberries.Store.Presentation;

public class UpdateRatingRequestValidator : AbstractValidator<UpdateRatingRequestDto>
{
    public UpdateRatingRequestValidator()
    {
        RuleFor(x => x.Value)
            .NotNull()
            .InclusiveBetween(1, 5);
    }
}