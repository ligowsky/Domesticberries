using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public class UpdateItemValidator : AbstractValidator<ItemDto>
{
    public UpdateItemValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .Length(1, 128);
        
        RuleFor(x => x.Description).NotNull();
    }
}