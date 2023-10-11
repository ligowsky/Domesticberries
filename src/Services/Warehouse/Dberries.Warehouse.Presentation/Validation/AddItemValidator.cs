using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public class AddItemValidator : AbstractValidator<ItemDto>
{
    public AddItemValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 128);
        
        RuleFor(x => x.Description).NotEmpty();
    }
}