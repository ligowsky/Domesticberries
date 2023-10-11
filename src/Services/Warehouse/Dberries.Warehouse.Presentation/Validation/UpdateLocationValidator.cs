using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public class UpdateLocationValidator : AbstractValidator<LocationDto>
{
    public UpdateLocationValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .Length(1, 128);

        RuleFor(location => location.Coordinates).SetValidator(new CoordinatesValidator()!);
    }
}