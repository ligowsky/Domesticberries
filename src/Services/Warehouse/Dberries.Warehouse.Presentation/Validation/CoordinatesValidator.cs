using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public class CoordinatesValidator : AbstractValidator<CoordinatesDto>
{
    public CoordinatesValidator()
    {
        RuleFor(coordinates => coordinates.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.");

        RuleFor(coordinates => coordinates.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.");
    }
}