using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public class UpdateStockValidator : AbstractValidator<StockDto>
{
    public UpdateStockValidator()
    {
        RuleFor(x => x.Quantity)
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}