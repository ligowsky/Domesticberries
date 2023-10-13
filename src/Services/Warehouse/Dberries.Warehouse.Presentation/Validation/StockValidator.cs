using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public class StockValidator : AbstractValidator<StockDto>
{
    public StockValidator()
    {
        RuleFor(x => x.Quantity)
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}