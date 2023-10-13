using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public class ItemValidator : AbstractValidator<ItemDto>
{
    public ItemValidator()
    {
        RuleSet($"{ActionType.Create}", () =>
        {
            RuleFor(x => x.Name).NotEmpty();
            
            RuleFor(x => x.Description).NotEmpty();
        });

        RuleFor(x => x.Name)
            .MaximumLength(128);
    }
}