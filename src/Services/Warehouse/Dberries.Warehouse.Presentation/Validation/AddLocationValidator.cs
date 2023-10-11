using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public class AddLocationValidator : AbstractValidator<LocationDto>
{
    public AddLocationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 128);

        RuleFor(location => location.Coordinates).SetValidator(new CoordinatesValidator()!);
    }
}