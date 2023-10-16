using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public class LocationValidator : AbstractValidator<LocationDto>
{
    public LocationValidator()
    {
        RuleSet($"{ActionType.Create}", () =>
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Coordinates).NotNull();
        });

        RuleFor(x => x.Name)
            .MaximumLength(128);

        RuleFor(location => location.Coordinates).SetValidator(new CoordinatesValidator()!);
    }
}